using Microsoft.AspNetCore.Mvc;
using Moq;
using Standard.API.PSQL.Application.Controllers;
using Standard.API.PSQL.Domain.Common;
using Standard.API.PSQL.Domain.DTOs;
using Standard.API.PSQL.Domain.Services;

namespace Standard.API.PSQL.Tests.Controllers
{
    public class SampleControllerTests
    {
        private readonly SampleController _sampleController;
        private readonly Mock<ISampleService> _sampleServiceMock;

        public SampleControllerTests()
        {
            _sampleServiceMock = new Mock<ISampleService>();
            _sampleController = new SampleController(_sampleServiceMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var sampleDtoList = new List<SampleDto>()
            {
                new SampleDto { Id = Guid.NewGuid(), Name = "Test 1" },
                new SampleDto { Id = Guid.NewGuid(), Name = "Test 2" }
            };

            _sampleServiceMock.Setup(service => service.GetAll()).ReturnsAsync(sampleDtoList);

            // Act
            var result = await _sampleController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<SampleDto>>(okResult.Value);
            Assert.Equal(sampleDtoList, returnValue);
            _sampleServiceMock.Verify(service => service.GetAll(), Times.Once());
        }

        [Fact]
        public async Task GetWithId_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var id = Guid.NewGuid();
            var sampleDto = new SampleDto { Id = id, Name = "Test" };
            _sampleServiceMock.Setup(service => service.Get(id)).ReturnsAsync(sampleDto);

            // Act
            var result = await _sampleController.Get(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<SampleDto>(okResult.Value);
            Assert.Equal(sampleDto, returnValue);
            _sampleServiceMock.Verify(service => service.Get(id), Times.Once());
        }

        [Fact]
        public async Task Get_ShouldReturn_OkResult()
        {
            // Arrange
            var pagination = new Pagination { Skip = 0, Limit = 5 };
            var sampleDtoList = new List<SampleDto> { new SampleDto { Id = Guid.NewGuid(), Name = "Test Sample" } };
            _sampleServiceMock.Setup(s => s.Get(pagination)).ReturnsAsync(sampleDtoList);

            // Act
            var result = await _sampleController.Get(pagination);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SampleDto>>(viewResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task Post_ShouldReturn_OkResult()
        {
            // Arrange
            var sampleDto = new SampleDto { Id = Guid.NewGuid(), Name = "Test Sample" };
            _sampleServiceMock.Setup(s => s.Add(sampleDto)).ReturnsAsync(sampleDto);

            // Act
            var result = await _sampleController.Post(sampleDto);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<SampleDto>(viewResult.Value);
            Assert.Equal(sampleDto.Id, model.Id);
        }

        [Fact]
        public async Task Put_ShouldReturn_OkResult()
        {
            // Arrange
            var sampleDto = new SampleDto { Id = Guid.NewGuid(), Name = "Test Update" };
            _sampleServiceMock.Setup(s => s.Update(sampleDto)).ReturnsAsync(sampleDto);

            // Act
            var result = await _sampleController.Put(sampleDto);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<SampleDto>(viewResult.Value);
            Assert.Equal(sampleDto.Id, model.Id);
        }

        [Fact]
        public async Task Delete_ShouldReturn_OkResult()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            _sampleServiceMock.Setup(s => s.Delete(id)).ReturnsAsync(true);

            // Act
            var result = await _sampleController.Delete(id);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<bool>(viewResult.Value);
            Assert.True(response);
        }
    }
}