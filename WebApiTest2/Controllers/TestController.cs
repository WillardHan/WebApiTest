using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Controller;
using WebApi.Infrastructure.ZooKeepr;

namespace WebApiTest2.Controllers
{
    public class TestController : ApiController
    {
        public TestController()
        {
        }

        [HttpGet("lock")]
        public IActionResult Lock()
        {
            Parallel.For(1, 10, async (i) =>
            {
                await new ZooKeeprLock().Lock("locks", () =>
                {
                    Console.WriteLine($"第{i}个请求,获取锁成功:{DateTime.Now},线程Id:{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(1000); // 业务逻辑...
                }, () =>
                {
                    Console.WriteLine($"第{i}个请求,释放锁成功:{DateTime.Now},线程Id:{Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine("-------------------------------");
                });
            });
            return Ok();
        }
    }
}
