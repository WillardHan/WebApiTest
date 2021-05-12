using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Controller;
using WebApi.Infrastructure.Redis;

namespace WebApiTest2.Controllers
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

        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromRoute] string key)
        {
            var guid = redisService.Get<string>(key);
            return await Task.FromResult(Ok(guid));
        }
    }
}
