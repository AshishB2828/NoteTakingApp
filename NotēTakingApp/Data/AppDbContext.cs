using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Models;

namespace NoteTakingApp.Data
{
    
        public class AppDbContext : DbContext
        {

            public AppDbContext(DbContextOptions options) : base(options)
            {

            }

            public DbSet<Note> Notes { get; set; }
        }
    
}
