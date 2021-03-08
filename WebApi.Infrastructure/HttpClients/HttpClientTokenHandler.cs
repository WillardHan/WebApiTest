using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.HttpClients
{
	public class HttpClientTokenHandler : DelegatingHandler
	{
		private const string ACCESS_TOKEN = "access_token";
		private readonly IHttpContextAccessor httpContextAccesor;
		private readonly ILogger<HttpClientTokenHandler> logger;

		public HttpClientTokenHandler(
			IHttpContextAccessor httpContextAccesor,
			ILogger<HttpClientTokenHandler> logger
			)
		{
			this.httpContextAccesor = httpContextAccesor;
			this.logger = logger;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var authorizationHeader = httpContextAccesor.HttpContext.Request.Headers["Authorization"];
			if (!string.IsNullOrEmpty(authorizationHeader))
			{
				request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
			}

			var token = await httpContextAccesor.HttpContext.GetTokenAsync(ACCESS_TOKEN);
			if (token != null)
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}

			var response = await base.SendAsync(request, cancellationToken);
			if (!response.IsSuccessStatusCode)
			{
				logger.LogWarning($"{request.RequestUri}{(int)response.StatusCode}{response.Headers.Date}");
			}

			return response;
		}
	}
}
