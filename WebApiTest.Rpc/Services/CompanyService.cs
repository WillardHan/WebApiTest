using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Infrastructure.Attributes;
using WebApi.Infrastructure.Exceptions;
using WebApi.Infrastructure.LifetimeInterfaces;
using WebApiTest.Rpc.DTOs;

namespace WebApiTest.Rpc.Services
{
    [Service(ServiceLifetime.Scoped)]
    public class CompanyService : ICompanyService, IScopedInterface
    {
        private readonly HttpClient httpClient;
        //private string BaseUrl 
        //{
        //    get 
        //    {
        //        return "http://10.0.75.1:55001/";
        //    }
        //}
        public CompanyService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient("token");
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllAsync()
        {
            var uri = $"{httpClient.BaseAddress}Company";
            var responseObj = await httpClient.GetAsync(uri);
            if (!responseObj.IsSuccessStatusCode)
            {
                throw new ValidateLevelException("get all company error");
            }
            else
            {  
                var responseString = await responseObj.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<CompanyDTO>>(responseString);
            }
        }
    }
}
