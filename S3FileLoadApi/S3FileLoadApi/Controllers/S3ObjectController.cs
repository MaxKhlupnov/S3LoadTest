using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S3FileLoadApi.Models;
using Amazon.S3;
using Amazon.Auth;
using Amazon.S3.Model;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;
using System.Globalization;
using S3FileLoadApi.Framework;

namespace S3FileLoadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3ObjectController : ControllerBase
    {

        private readonly IAmazonS3 yandexS3;

        private readonly RequestStatDbContext statDbContext;

        private readonly string bucketName;

        private readonly MD5 md5;

        public S3ObjectController(IAmazonS3 amazonS3, RequestStatDbContext statDbContext, IConfiguration config)
        {

            this.yandexS3 = amazonS3;
            this.statDbContext = statDbContext;
            this.bucketName = config.GetSection("S3")["bucket"];
            md5 = MD5.Create();
        }

        [HttpGet]
        [Route("ReadRandom")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ObjectGetRequestStat>> ReadRandom()
        {
            var rand = new Random();
            int nextRandom = rand.Next(1000000);
            string fileName = nextRandom.ToString("D6");

            var req = new GetObjectRequest
            {
                BucketName = this.bucketName,
                Key = fileName
            };

            ObjectGetRequestStat requestStat = new ObjectGetRequestStat()
            {
                RequestId = Guid.NewGuid().ToString(),
                ObjectName = req.Key,
                ProcessingHost = HttpContext.Request.Host.Host,
                RemoteClientHost = HttpContext.Connection.RemoteIpAddress.ToString(),
                RequestStartDateTime = DateTime.UtcNow
                 
            };
            var watch = new System.Diagnostics.Stopwatch();

            try
            {
                watch.Start();
                GetObjectResponse res = await this.yandexS3.GetObjectAsync(req);
                watch.Stop();
                requestStat.RequestId = res.ResponseMetadata.RequestId; // rewrite request id with actual s3 request id
                requestStat.ElapsedMs = watch.ElapsedMilliseconds;
                requestStat.ResponseCode = (int) res.HttpStatusCode;
                requestStat.ContentMd5Hash = res.Metadata["X-Amz-Meta-Md5"];
                requestStat.ContentLength = res.ContentLength;                           
                requestStat.Md5HashMatch = ComapreHash(res.Metadata["X-Amz-Meta-Md5"], res.ResponseStream);
            }
            catch (Exception ex)
            {
                requestStat.ErrMsg = $"{ex.Message}\n\r {ex.StackTrace}";
                requestStat.ResponseCode = -1;
                return StatusCode(StatusCodes.Status500InternalServerError, $"{req.BucketName} {req.Key} {ex.Message}");
            }
            finally
            {
                requestStat.RequestCompleateDateTime = DateTime.UtcNow;
                await this.statDbContext.RequestStat.AddAsync(requestStat);
                await this.statDbContext.SaveChangesAsync();
            }
            

            return Ok(requestStat);
        }

        private bool ComapreHash(string Hash, Stream Content)
        {
            byte[] hashBytes = md5.ComputeHash(Content);
            string calcHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            return String.Compare(Hash, calcHash, StringComparison.InvariantCultureIgnoreCase) == 0;
        }  
    }
}
