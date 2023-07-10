using Standard.API.PSQL.Domain.Repository;

namespace Standard.API.PSQL.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        ISampleRepository SampleRepository { get; set; }
        Task<int> SaveAsync();
    }
}
