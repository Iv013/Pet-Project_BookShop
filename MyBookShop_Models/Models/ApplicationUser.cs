using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookShop_Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string Region { get; set; }
        [NotMapped]
        public string StreetAddress { get; set; }
    }
}
