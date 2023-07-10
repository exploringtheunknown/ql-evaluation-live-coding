using AutoMapper;
using Standard.API.PSQL.Domain;
using Standard.API.PSQL.Domain.Common;
using Standard.API.PSQL.Domain.DTOs;
using Standard.API.PSQL.Domain.Entities;
using Standard.API.PSQL.Domain.Exceptions;
using Standard.API.PSQL.Domain.Services;
using System.Security;

namespace Standard.API.PSQL.Service.Services
{
    public class SampleService : ISampleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SampleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<SampleDto> Get(Guid id)
        {
            var sampleEntity = await _unitOfWork.SampleRepository.GetByIdAsync(id);
            return _mapper.Map<SampleDto>(sampleEntity);
        }


        public async Task<IEnumerable<SampleDto>> GetAll()
        {
            var sampleEntityList = await _unitOfWork.SampleRepository.GetAllAsync();
            return _mapper.Map<List<SampleDto>>(sampleEntityList);
        }
        public async Task<IEnumerable<SampleDto>> Get(Pagination pagination)
        {
            var sampleEntityList = await _unitOfWork.SampleRepository.GetAllAsync(pagination.Skip, pagination.Limit);
            return _mapper.Map<IEnumerable<SampleDto>>(sampleEntityList);
        }

        public async Task<SampleDto> Add(SampleDto sampleDto)
        {
            var entity = _mapper.Map<Sample>(sampleDto);

            var response = await _unitOfWork.SampleRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SampleDto>(response);
        }
        public async Task<SampleDto> Update(SampleDto sampleDto)
        {
            var exists = await _unitOfWork.SampleRepository.ExistAsync(x => x.Id == sampleDto.Id);
            if (!exists) throw new ConflictException("Not found");

            var entity = _mapper.Map<Sample>(sampleDto);

            var response = await _unitOfWork.SampleRepository.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SampleDto>(response);
        }

        public async Task<bool> Delete(Guid id)
        {
            var exists = await _unitOfWork.SampleRepository.ExistAsync(x => x.Id == id);
            if (!exists) throw new NotFoundException("Not found");

            var entity = await _unitOfWork.SampleRepository.GetByIdAsync(id);

            await _unitOfWork.SampleRepository.DeleteAsync(entity);
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}