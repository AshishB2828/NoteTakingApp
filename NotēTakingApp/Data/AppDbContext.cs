using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Models;

namespace NoteTakingApp.Data
{
    
        public class AppDbContext : IdentityDbContext<ApplicationUser>
        {

            public AppDbContext(DbContextOptions options) : base(options)
            {

            }

            public DbSet<Note> Notes { get; set; }
            public DbSet<ApplicationUser> Users { get; set; }
        }
    
}
