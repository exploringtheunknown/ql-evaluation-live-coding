using Microsoft.EntityFrameworkCore;
using Standard.API.PSQL.Domain.Entities;
using Standard.API.PSQL.Domain.Repository;

namespace Standard.API.PSQL.Infra.Data.Repositories
{
    public class SampleRepository : Repository<Sample>, ISampleRepository
    {
        public SampleRepository(DbSet<Sample> samples) : base(samples)
        {
        }
    }
}