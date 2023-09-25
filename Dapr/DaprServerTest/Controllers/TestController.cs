using DaprTestInterface;
using Microsoft.AspNetCore.Mvc;

namespace DaprServerTest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestDto GetData()
        {
            _logger.LogInformation("服务被调用");
            return new TestDto(666, "tonwin");
        }
    }
}
