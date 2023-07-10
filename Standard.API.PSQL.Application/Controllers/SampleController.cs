using Microsoft.AspNetCore.Mvc;
using Standard.API.PSQL.Domain.Common;
using Standard.API.PSQL.Domain.DTOs;
using Standard.API.PSQL.Domain.Services;

namespace Standard.API.PSQL.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : Controller
    {
        private readonly ISampleService _sampleService;

        public SampleController(ISampleService sampleService) => _sampleService = sampleService;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _sampleService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _sampleService.Get(id));

        [HttpGet("pagination")]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination) => Ok(await _sampleService.Get(pagination));

        [HttpPost]
        public async Task<IActionResult> Post(SampleDto dto) => Ok(await _sampleService.Add(dto));

        [HttpPut]
        public async Task<IActionResult> Put(SampleDto dto) => Ok(await _sampleService.Update(dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _sampleService.Delete(id));
    }
}
