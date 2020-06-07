// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;
using System.Collections.Generic;

namespace BlazorContracts.Auth
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource(name: "blazorcontracts-api")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientId = "blazorcontracts-web",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "blazorcontracts-api" }
                }
            };
    }
}