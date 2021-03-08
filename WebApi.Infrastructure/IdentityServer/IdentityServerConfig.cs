using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Infrastructure.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> Apis
            => new List<ApiResource>
        {
            new ApiResource("api1","My API")
        };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId="client",
                    AllowedGrantTypes =GrantTypes.ClientCredentials,
                    ClientSecrets={
                    new Secret("aju".Sha256())
                    },
                    AllowedScopes={ "api1"}
                }
            };
    }
}
