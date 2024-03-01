using Microsoft.AspNetCore.Identity;

namespace BTL.Models
{
    public class AppUserModel : IdentityUser
    {
        public string Ocupation { get; set; }
    }
}
