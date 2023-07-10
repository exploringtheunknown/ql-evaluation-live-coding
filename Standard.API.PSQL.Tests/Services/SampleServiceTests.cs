using AutoMapper;
using Moq;
using Standard.API.PSQL.Domain.DTOs;
using Standard.API.PSQL.Domain;
using Standard.API.PSQL.Service.Services;
using Standard.API.PSQL.Domain.Entities;
using Standard.API.PSQL.Domain.Common;
using Standard.API.PSQL.Domain.Exceptions;

namespace Standard.API.PSQL.Tests.Services
{
    public class SampleServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private SampleService _sampleService;

        public SampleServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _sampleService = new SampleService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturn_ValidSampleDto()
        {
            // Arrange
            Guid sampleId = Guid.NewGuid();
            Sample sampleEntity = new Sample { Id = sampleId, Name = "Test Sample" };
            SampleDto sampleDto = new SampleDto { Id = sampleId, Name = "Test Sample" };
            _unitOfWorkMock.Setup(uow => uow.SampleRepository.GetByIdAsync(sampleId, null)).ReturnsAsync(sampleEntity);
            _mapperMock.Setup(mapper => mapper.Map<SampleDto>(sampleEntity)).Returns(sampleDto);

            // Act
            var result = await _sampleService.Get(sampleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sampleId, result.Id);
        }

        [Fact]
        public async Task GetAll_ShouldReturn_ValidSampleDtoList()
        {
            // Arrange
            List<Sample> sampleEntities = new List<Sample>
            {
                new Sample{ Id = Guid.NewGuid(), Name = "Test Sample 1" },
                new Sample{ Id = Guid.NewGuid(), Name = "Test Sample 2" }
            };

            List<SampleDto> sampleDtos = new List<SampleDto>
            {
                new SampleDto { Id = sampleEntities[0].Id, Name = "Test Sample 1" },
                new SampleDto { Id = sampleEntities[1].Id, Name = "Test Sample 2" }
            };

            _unitOfWorkMock.Setup(uow => uow.SampleRepository.GetAllAsync(null, null)).ReturnsAsync(sampleEntities);
            _mapperMock.Setup(mapper => mapper.Map<List<SampleDto>>(sampleEntities)).Returns(sampleDtos);

            // Act
            var result = await _sampleService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetWithPagination_ShouldReturn_ValidSampleDtoList()
        {
            // Arrange
            var pagination = new Pagination { Skip = 0, Limit = 5 };
            List<Sample> sampleEntities = new List<Sample>
        {
            new Sample { Id = Guid.NewGuid(), Name = "Test Sample 1" },
            new Sample { Id = Guid.NewGuid(), Name = "Test Sample 2" }
        };

            _unitOfWorkMock.Setup(uow => uow.SampleRepository.GetAllAsync(pagination.Skip, pagination.Limit,null,null)).ReturnsAsync(sampleEntities);
            var sampleDtos = sampleEntities.Select(e => new SampleDto { Id = e.Id, Name = e.Name });
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<SampleDto>>(sampleEntities)).Returns(sampleDtos);

            // Act
            var result = await _sampleService.Get(pagination);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Add_ShouldReturn_ValidSampleDto()
        {
            // Arrange
            var sampleDto = new SampleDto { Id = Guid.NewGuid(), Name = "Test Sample" };
            var sampleEntity = new Sample { Id = sampleDto.Id.Value, Name = sampleDto.Name };
            _mapperMock.Setup(mapper => mapper.Map<Sample>(sampleDto)).Returns(sampleEntity);
            _mapperMock.Setup(mapper => mapper.Map<SampleDto>(sampleEntity)).Returns(sampleDto);
            _unitOfWorkMock.Setup(uow => uow.SampleRepository.AddAsync(It.IsAny<Sample>())).ReturnsAsync(sampleEntity);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _sampleService.Add(sampleDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sampleDto.Id, result.Id);
        }

        [Fact]
        public async Task Update_ShouldReturn_ValidSampleDto()
        {
            // Arrange
            var sampleDto = new SampleDto { Id = Guid.NewGuid(), Name = "Test Sample" };
            var sampleEntity = new Sample { Id = sampleDto.Id.Value, Name = sampleDto.Name };
            _unitOfWorkMock.Setup(uow => uow.SampleRepository.ExistAsync(x => x.Id == sampleDto.Id)).ReturnsAsync(true);
            _mapperMock.Setup(mapper => mapper.Map<Sample>(sampleDto)).Returns(sampleEntity);
            _mapperMock.Setup(mapper => mapper.Map<SampleDto>(sampleEntity)).Returns(sampleDto);
            _unitOfWorkMock.Setup(uow => uow.SampleRepository.UpdateAsync(It.IsAny<Sample>())).ReturnsAsync(sampleEntity);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _sampleService.Update(sampleDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sampleDto.Id, result.Id);
        }

        [Fact]
        public async Task Update_ShouldThrow_ConflictException()
        {
            // Arrange
            var sampleDto = new SampleDto { Id = Guid.NewGuid(), Name = "Test Sample" };
            _unitOfWorkMock.Setup(uow => uow.SampleRepository.ExistAsync(x => x.Id == sampleDto.Id)).ReturnsAsync(false);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<ConflictException>(() => _sampleService.Update(sampleDto));
            Assert.Equal("Not found", exception.Message);
        }

        [Fact]
        public async Task Delete_Returns_True_When_Success()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var sampleEntity = new Sample { Id = id, Name = "Test Sample" };
            _unitOfWorkMock.Setup(uow => uow.SampleRepository.ExistAsync(x => x.Id == id)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(uow => uow.SampleRepository.GetByIdAsync(id,null)).ReturnsAsync(sampleEntity);
            _unitOfWorkMock.Setup(uow => uow.SampleRepository.DeleteAsync(sampleEntity)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _sampleService.Delete(id);

            // Assert
            Assert.True(result);
        }
    }
}
