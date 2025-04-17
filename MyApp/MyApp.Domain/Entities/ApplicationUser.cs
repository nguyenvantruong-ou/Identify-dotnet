
using Microsoft.AspNetCore.Identity;

namespace MyApp.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
