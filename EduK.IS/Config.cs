using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace EduK.IS
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            //var newResource = new IdentityResource(name: "pessoal", displayName: "Dados Pessoais", claimTypes: new[] { "cargo", "funcao", "setor", "telefone", "nascimento", "endereco" });
            //newResource.Description = "Dados pessoais do usuário";

            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("edukapi", "EduK Api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "edukclient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("EduK$Client".Sha256())
                    },
                    AllowedScopes = { "edukapi" }
                },

                new Client
                {
                    ClientId = "edukweb",
                    ClientName = "EduK Web",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = true,

                    ClientSecrets =
                    {
                        new Secret("Eduk@Web".Sha256())
                    },

                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "edukapi"
                    },
                    AllowOfflineAccess = true,
                    LogoUri = "edukweb"
                },

                new Client
                {
                    ClientId = "sample",
                    ClientName = "Sample App",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("Sample@App".Sha256())
                    },

                    RedirectUris = { "http://localhost:5003/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "edukapi"
                    },
                    AllowOfflineAccess = true,
                    LogoUri = "sample"
                }
            };
        }

    }
}
