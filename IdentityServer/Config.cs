// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Address(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("webapitest", "Web Api Test"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "client1",
                    ClientName = "Client Credentials 1",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("80747563-81e7-f75d-0114-46e04c81ed53".Sha256()) },

                    AllowedScopes = { "webapitest" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "client2",
                    ClientName = "Client Credentials 2",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("c8d0cf33-391a-bc27-9609-6eb1d2b0dc8b".Sha256()) },

                    AllowedScopes = { "webapitest", IdentityServerConstants.StandardScopes.OpenId, "profile", "email" }
                },
            };
    }
}