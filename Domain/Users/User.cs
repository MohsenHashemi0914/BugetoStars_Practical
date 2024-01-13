using Microsoft.AspNetCore.Identity;

namespace Domain.Users
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}