using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nexum_Tech.Domain.Interfaces;
using NexumTech.Infra.API;
using NexumTech.Infra.Models;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ITest _testService;

        public TestController(ITest testService) 
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<Object> EndpointTest(TestViewModel test)
        {
            return _testService.Teste(); 
        }
    }
}