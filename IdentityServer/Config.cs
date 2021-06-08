// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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

        public static IEnumerable<ApiResource> Apis =>
                    new ApiResource[]
                    {
                         new ApiResource("webapitest", "Web Api Test")
                         {
                             Scopes = {
                                 "webapitest"
                             }
                         },
                         new ApiResource("mvctest", "Mvc Test")
                         {
                             Scopes = {
                                 "mvctest"
                             }
                         }
                    };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("mvctest", "Mvc Test"),
                new ApiScope("webapitest", "Web Api Test"),
                new ApiScope("webapitest2", "Web Api Test 2")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "client1",
                    ClientName = "Client Credentials 1",
                    ClientSecrets = { new Secret("secret1".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = { "webapitest", "webapitest2" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "client2",
                    ClientName = "Client Credentials 2",
                    ClientSecrets = { new Secret("secret2".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowOfflineAccess = true,
                    AllowedScopes = { "webapitest", IdentityServerConstants.StandardScopes.OpenId, "profile", "email", OidcConstants.StandardScopes.OfflineAccess }
                },

                new Client
                {
                    ClientId = "client3",
                    ClientName = "Client Credentials 3",
                    ClientSecrets = { new Secret("secret3".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    
                    RedirectUris = { "http://10.0.75.1:55008/signin-oidc" },
                    FrontChannelLogoutUri = "http://10.0.75.1:55008/signout-oidc",
                    PostLogoutRedirectUris = { "http://10.0.75.1:55008/signout-callback-oidc" },

                    AllowAccessTokensViaBrowser = true,

                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 360, // 360 seconds

                    AllowedScopes = { "mvctest", "openid", "profile", "address" }
                },

                new Client
                {
                    ClientId = "client4",
                    ClientName = "Client Credentials 4",
                    ClientSecrets = { new Secret("secret4".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "http://10.0.75.1:55008/signin-oidc" },
                    FrontChannelLogoutUri = "http://10.0.75.1:55008/signout-oidc",
                    PostLogoutRedirectUris = { "http://10.0.75.1:55008/signout-callback-oidc" },
                    //RequireConsent = true,

                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 10, // 360 seconds

                    AllowedScopes = { "mvctest", "openid", "profile", "address", OidcConstants.StandardScopes.OfflineAccess }
                },

                new Client
                {
                    ClientId = "client5",
                    ClientName = "Client Credentials 5",
                    ClientSecrets = { new Secret("secret5".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    RedirectUris = { "http://10.0.75.1:55008/signin-oidc" },
                    FrontChannelLogoutUri = "http://10.0.75.1:55008/signout-oidc",
                    PostLogoutRedirectUris = { "http://10.0.75.1:55008/signout-callback-oidc" },

                    

                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 360, // 360 seconds

                    RequirePkce = false,
                    //AllowAccessTokensViaBrowser = true,

                    AllowedScopes = { "mvctest", "openid", "profile", "address", OidcConstants.StandardScopes.OfflineAccess }
                },

                new Client
                {
                    ClientId = "client6",
                    ClientName = "Client Credentials 6",
                    ClientSecrets = { new Secret("secret6".Sha256()) },
                    AllowedGrantTypes = GrantTypes.DeviceFlow,

                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedScopes = { "mvctest", "webapitest", "webapitest2", "openid", "profile", "address" }
                },
            };
    }
}