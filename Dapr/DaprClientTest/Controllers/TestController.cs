using DaprTestInterface;
using Microsoft.AspNetCore.Mvc;
using Senparc.Xncf.Dapr;
using Senparc.Xncf.Dapr.Blocks.ServiceInvoke;
using System;
using System.Net.Http;
using System.Text.Json;

namespace DaprClientTest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;
        private readonly DaprClient _daprClient;
        public TestController(ILogger<TestController> logger,DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        //返回传入的数据
        [HttpPost]
        public TestDto ReturnInputData(TestDto dto)
        {
            return dto;
        }

        //存储管理测试
        [HttpGet]
        public async Task<TestDto> StateStoreTest()
        {
            await _daprClient.SetStateAsync("test-statestore", "TestDto001", new TestDto(100,"test"));
            return await _daprClient.GetStateAsync<TestDto>("test-statestore", "TestDto001");
        }

        //本地服务调用测试
        [HttpGet]
        public async Task<TestDto> LocalServiceTest()
        {
            return await _daprClient.InvokeMethodAsync<TestDto>(InvokeType.Post, "testclient", "Test/ReturnInputData", new TestDto(999, "ncf"));
        }

        //远程服务调用测试
        [HttpGet]
        public async Task<TestDto> RemoteServiceTest()
        {
            var data = new TestDto(100, "dapr");
            var res = await _daprClient.InvokeMethodAsync<TestDto>(InvokeType.Get, "testserver", "Test/GetData", data);
            return res;
        }

        //事件发布测试
        [HttpPost]
        public async Task PublishEventTest()
        {
            var data = new TestDto(888, "testEvent");
            await _daprClient.PublishEventAsync("test-pubsub","testTopic", data);
        }

        //边车健康检查
        [HttpGet]
        public async Task<string> SidecarHealthCheck()
        {
            if (await _daprClient.HealthCheckAsync())
            {
                return "dapr is healthy";
            }
            else
            {
                return "dapr is not healthy";
            }
        }
    }
}
