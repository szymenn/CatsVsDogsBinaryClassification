using CatsVsDogs.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatsVsDogs.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
            
        }
        
        public DbSet<PredictionHistory> PredictionHistory { get; set; }
    }
}