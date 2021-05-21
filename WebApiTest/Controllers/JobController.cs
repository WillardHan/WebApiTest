using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Controller;
using Hangfire;
using System;
using System.Collections.Generic;

namespace WebApiTest.Controllers
{
    public class JobController : ApiController
    {
        public JobController()
        {
        }

        [HttpGet("fireandforget")]
        public async Task<IActionResult> Enqueue()
        {
            var result = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                var jobId = BackgroundJob.Enqueue(() => SendSmsForTest());
                var continueJobId = BackgroundJob.ContinueJobWith(jobId, () => SendEmailForTest());
                result.Add(jobId);
                result.Add(continueJobId);
            }

            return Ok(await Task.FromResult(result));
        }

        public async Task SendSmsForTest()
        {
            // await service, retry in dashboard when fail
            var test = await Task.FromResult(DateTime.Now);
        }

        public async Task SendEmailForTest()
        {
            // await service, retry in dashboard when fail
            var test = await Task.FromResult(DateTime.Now);
        }

        [HttpGet("delayed ")]
        public async Task<IActionResult> Schedule()
        {
            var jobId = BackgroundJob.Schedule(() => SendDelayedEmail(), TimeSpan.FromSeconds(30));
            return Ok(await Task.FromResult(jobId));
        }

        public async Task SendDelayedEmail()
        {
            // await service, retry in dashboard when fail
            var test = await Task.FromResult(DateTime.Now);
        }
    }
}
