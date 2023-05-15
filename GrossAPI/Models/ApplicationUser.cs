using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
    }
}
