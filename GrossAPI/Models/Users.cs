using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models
{
    public class Users
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string PasswordHash { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public string TelNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
    }
}
