using DocuSign.eSign.Model;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WatchShop.Infrastructure.Data.Domain;

using WatchShopApp.Data;

namespace WatchShop.Infrastructure.Data.Infrastructure
{
    public static class ApplicationBuilderExtension
    {

        
            public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
            {
                using var serviceScope = app.ApplicationServices.CreateScope();
                var services = serviceScope.ServiceProvider;

                await RoleSeeder(services);
                await SeedAdministrator(services);

                var dataCategory = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedCategories(dataCategory);

                var dataBrand = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedBrands(dataBrand);

                return app;
            }


            private static void SeedCategories(ApplicationDbContext dataCategory)
            {
                if (dataCategory.Categories.Any())
                {
                    return;
                }
                dataCategory.Categories.AddRange(new[]
                {
            new Category{CategoryName = "Rolex"},
            new Category{CategoryName = "AP"},
            new Category{CategoryName = "Casio"},
            new Category{CategoryName = "Cartier"},
            new Category{CategoryName = "Casio"},


                }

                );
                dataCategory.SaveChanges();
            }

            private static void SeedBrands(ApplicationDbContext dataManufacturer)
            {
              
            }

            private static async Task RoleSeeder(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roleNames = { "Administrator", "Client" };
                IdentityResult roleResult;

                foreach (var role in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            private static async Task SeedAdministrator(IServiceProvider serviceProvider)
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (await userManager.FindByNameAsync("admin") == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "admin",
                        LastName = "admin",
                        UserName = "admin",
                        Email = "admin@admin.com",
                        Adress = "admin address",
                        PhoneNumber = "0888888888"
                    };

                    var result = await userManager.CreateAsync(user, "Admin123456");

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Administrator").Wait();
                    }
                }
            }

        
    }
}

