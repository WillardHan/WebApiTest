using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Controller;
using WebApi.Infrastructure.Redis;
using WebApi.Infrastructure.Utility;
using WebApiTest.Application.Models;
using WebApiTest.Domain.Models;

namespace WebApiTest.Controllers
{
    public class StoreController : ApiController
    {
        private readonly IRedisService redisService;
        private readonly IOperationHistoryRepository mongoRepository;

        public StoreController(
            IRedisService redisService,
            IOperationHistoryRepository mongoRepository
            )
        {
            this.redisService = redisService;
            this.mongoRepository = mongoRepository;
        }

        [HttpPut("redis")]
        public async Task<IActionResult> RedisSave([FromBody]RedisStoreModel request)
        {
            redisService.Set(request.Key, request.Value);
            return await Task.FromResult(Ok(true));
        }

        [HttpPut("mongo")]
        public async Task<IActionResult> MongoSave([FromBody]MongoStoreModel request)
        {
            await mongoRepository.AddAsync(new OperationHistory { 
                Content = request.Content
            });
            return await Task.FromResult(Ok(true));
        }

        [HttpGet("mongo")]
        public async Task<IActionResult> MongoGet()
        {
            var result = await mongoRepository.FindAsync();
            return Ok(result);
        }
    }
}
