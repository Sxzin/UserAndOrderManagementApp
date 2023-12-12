using Microsoft.AspNetCore.Identity;

namespace UserAndOrderManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
