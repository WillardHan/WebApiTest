using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Infrastructure.Controller;

namespace WebApiTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        public IdentityController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            var result = string.Empty;

            var client = new HttpClient();
            var response = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = DomainParameter.IdentityServerUrl,
                Policy ={
                            RequireHttps = false
                        }
            });
            if (response.IsError) Console.WriteLine(response.Error);

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = response.TokenEndpoint,
                ClientId = "client2",
                ClientSecret = "c8d0cf33-391a-bc27-9609-6eb1d2b0dc8b",
                UserName = "alice",
                Password = "alice",
                Scope = $"{DomainParameter.Audience} openid profile email"
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
