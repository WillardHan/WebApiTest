using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApi.Infrastructure.Controller;

namespace WebApiTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        public IConfiguration configuration { get; set; }
        public IdentityController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            var result = string.Empty;

            var client = new HttpClient();
            var response = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = configuration.GetValue<string>("Identityserver_URL"),
                Policy ={
                            RequireHttps = false
                        }
            });
            if (response.IsError) Console.WriteLine(response.Error);

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = response.TokenEndpoint,
                ClientId = "client2",
                ClientSecret = "secret2",
                UserName = "alice",
                Password = "alice",
                Scope = $"webapitest webapitest2 openid profile email"
            });
            if (tokenResponse.IsError) Console.WriteLine(tokenResponse.Error);

            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            //{
            //    Address = response.TokenEndpoint,
            //    ClientId = "client1",
            //    ClientSecret = "80747563-81e7-f75d-0114-46e04c81ed53",
            //    Scope = DomainParameter.Audience
            //});
            //if (tokenResponse.IsError) Console.WriteLine(tokenResponse.Error);

            //var apiClient = new HttpClient();
            //apiClient.SetBearerToken(tokenResponse.AccessToken);
            //var apiResponse = await apiClient.GetAsync(response.UserInfoEndpoint);
            //if (apiResponse.IsSuccessStatusCode)
            //{
            //    result = await apiResponse.Content.ReadAsStringAsync();
            //}
            //else
            //{
            //    Console.WriteLine(apiResponse.StatusCode);
            //}

            //var apiClient = new HttpClient();
            //apiClient.SetBearerToken(tokenResponse.AccessToken);
            //var apiResponse = await apiClient.GetAsync($"{DomainParameter.WebApiTestUrl}/Company");
            //if (apiResponse.IsSuccessStatusCode)
            //{
            //    result = await apiResponse.Content.ReadAsStringAsync();
            //}
            //else
            //{
            //    Console.WriteLine(apiResponse.StatusCode);
            //}

            return Ok(tokenResponse);
        }
    }
}
