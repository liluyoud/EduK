using EduK.IS.Data;
using EduK.IS.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduK.IS
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                {
                    var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    context.Database.Migrate();
                    EnsureSeedData(context);
                }

                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var lilo = userMgr.FindByNameAsync("175.504.618-99").Result;
                    if (lilo == null)
                    {
                        lilo = new ApplicationUser
                        {
                            UserName = "175.504.618-99",
                            Email = "liluyoud@gmail.com",
                            PhoneNumber = "(69) 98114-1732"
                        };
                        var result = userMgr.CreateAsync(lilo, "Lilo$123").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(lilo, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Liluyoud Cury de Lacerda"),
                            new Claim(JwtClaimTypes.Email, "liluyoud@gmail.com")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Console.WriteLine("Liluyoud criado");
                    }
                    else
                    {
                        Console.WriteLine("Liluyoud já cadastrado");
                    }
                }
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            Console.WriteLine("Clients being populated");
            foreach (var client in Config.GetClients().ToList())
            {
                var registro = context.Clients.SingleOrDefault(c => c.ClientId == client.ClientId);
                if (registro == null)
                {
                    var entity = client.ToEntity();
                    context.Clients.Add(entity);
                }
            }
            context.SaveChanges();

            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in Config.GetApiResources().ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }
        }
    }
}
