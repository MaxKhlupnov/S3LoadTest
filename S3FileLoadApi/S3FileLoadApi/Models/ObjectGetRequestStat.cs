using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace S3FileLoadApi.Models
{
   
    public class ObjectGetRequestStat
    {

        /**
            Request UUID
        */
        [Key]
        public Guid RequestUuid { get; set; }
        /**
            Request start DateTime
        */
        public DateTime RequestStartDateTime { get; set; }
        /**
            Request compleate DateTime
        */
        public DateTime RequestCompleateDateTime { get; set; }
        /**
        S3 object name
        */
        public string ObjectName { get; set; }

        /**
         * Content length
         */
        public long ContentLength { get; set; }

        /** 
         * Время выполнения зарпоса (мл. сек.)
         */
        public long ElapsedMs { get; set; }

        /**
         * HTTP Response HTTP code
         */
        public int ResponseCode { get; set; }
        
        /**
         * Response error message
         */
        public string ErrMsg { get; set; }

        /**
         * Content md5 hash
         */
        public string ContentMd5Hash { get; set; }

        /**
         * Md5hash match
         */
        public bool Md5HashMatch { get; set; }

        /**
         * ProcessingHost
         */
        public string ProcessingHost { get; set; }

        /**
         * Refferer host
         */
        public string RemoteClientHost { get; set; }
    }
}
