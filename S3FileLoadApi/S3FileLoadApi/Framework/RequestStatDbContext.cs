using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using S3FileLoadApi.Models;

namespace S3FileLoadApi.Framework
{
    public class RequestStatDbContext : DbContext
    {
        public RequestStatDbContext(DbContextOptions<RequestStatDbContext> options) : base(options)
        {
        }

        public DbSet<ObjectGetRequestStat> RequestStat { get; set; }
    }
}
