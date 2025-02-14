using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Infrastructure.Data.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        
        public static async Task<ApplicationBuilder> PrepareDatabase(this ApplicationBuilder app)
        {
            using var serviceScop = app.ApplicationServices.CreateScope();

            var services = serviceScop.ServiceProvider;

            await RoleSeeder(services);
            await SeedAdministrator(services);

            return app;
        }

        private static async Task RoleSeeder(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "Client" };

            IdentityResult roleResult;

            foreach(var roleName in roleNames) 
            {

            }
        }
    }
}
