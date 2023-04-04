using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace NoteTakingApp.Models.DTO
{
    public class RegistartionData
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? UserName { get; set; }
        [Required]

        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
