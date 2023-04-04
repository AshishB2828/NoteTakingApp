using Microsoft.AspNetCore.Identity;

namespace NoteTakingApp.Models
{
    public class ApplicationUser: IdentityUser
    {

        public string? Name { get; set; }

    }
}
