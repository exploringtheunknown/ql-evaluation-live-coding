using Standard.API.PSQL.Domain;
using Standard.API.PSQL.Domain.Repository;
using Standard.API.PSQL.Infra.Data.Context;

namespace Standard.API.PSQL.Infra.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public ISampleRepository SampleRepository { get; set; }

        private readonly DatabaseContext _databaseContext;

        public UnitOfWork(DatabaseContext databaseContext,
            ISampleRepository sampleRepository)
        {
            _databaseContext = databaseContext;

            SampleRepository = sampleRepository;
        }

        public async Task<int> SaveAsync() => await _databaseContext.SaveChangesAsync();

        public void Dispose() => _databaseContext.Dispose();
    }

}
