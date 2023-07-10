using Standard.API.PSQL.Domain.Common;
using Standard.API.PSQL.Domain.DTOs;

namespace Standard.API.PSQL.Domain.Services
{
    public interface ISampleService
    {
        Task<SampleDto> Get(Guid id);
        Task<IEnumerable<SampleDto>> Get(Pagination pagination);
        Task<IEnumerable<SampleDto>> GetAll();
        Task<SampleDto> Add(SampleDto sampleDto);
        Task<SampleDto> Update(SampleDto sampleDto);
        Task<bool> Delete(Guid id);
    }
}
