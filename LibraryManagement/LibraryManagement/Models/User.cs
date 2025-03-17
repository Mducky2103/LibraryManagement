using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        
    }
}
