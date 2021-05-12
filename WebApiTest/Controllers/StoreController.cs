using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Controller;
using WebApi.Infrastructure.Redis;
using WebApiTest.Application.Models;

namespace WebApiTest.Controllers
{
    public class StoreController : ApiController
    {
        private readonly IRedisService redisService;

        public StoreController(
            IRedisService redisService
            )
        {
            this.redisService = redisService;
        }

        [HttpPut()]
        public async Task<IActionResult> Save([FromBody]StoreModel request)
        {
            redisService.Set(request.Key, request.Value);
            return await Task.FromResult(Ok(true));
        }
    }
}
