using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApiTest.Application.Services;

namespace WebApiTest.Application.Jobs
{
    public class RecordJob
    {
        private readonly ICompanyService companyService;
        private readonly ILogger<RecordJob> logger;
        public RecordJob(
            ICompanyService companyService,
            ILogger<RecordJob> logger
            )
        {
            this.companyService = companyService;
            this.logger = logger;
        }

        public async Task Execute()
        {
            var companies = await companyService.GetAll();
            logger.LogInformation($"record at {DateTime.Now}");
        }
    }
}
