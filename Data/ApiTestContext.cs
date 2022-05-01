using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiTest.Models;

namespace ApiTest.Data
{
    public class ApiTestContext : DbContext
    {
        public ApiTestContext (DbContextOptions<ApiTestContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBeer>().HasKey(sc => new { sc.BeerDataId, sc.UserModelId});
        }

        public DbSet<ApiTest.Models.BeerData> BeerData { get; set; }
        public DbSet<ApiTest.Models.UserBeer> UserBeer{ get; set; }
        public DbSet<ApiTest.Models.UserModel> UserModel { get; set; }
    }
}
