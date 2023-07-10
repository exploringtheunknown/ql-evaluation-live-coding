using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Standard.API.PSQL.Domain.Entities;
using Standard.API.PSQL.Infra.Data.Mapping;

namespace Standard.API.PSQL.Infra.Data.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) : base(options) { }

        public DbSet<Sample> Samples { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sample>(new SampleMap().Configure);
        }
    }
}
