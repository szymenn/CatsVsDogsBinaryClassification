using CatsVsDogs.Core.Helpers;
using CatsVsDogs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CatsVsDogs.Infrastructure.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabases(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>() )
                {
                    Migrate(appContext);
                }
               
            }
 
            return host;
        }

        private static void Migrate(DbContext appContext)
        {
            if (appContext.Database.ProviderName != Constants.InMemoryProvider)
            {
                appContext.Database.Migrate();
            }
        }
    }
}