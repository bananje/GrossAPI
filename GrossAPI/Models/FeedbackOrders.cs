using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrossAPI.Models
{
    public class FeedbackOrders
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Patronymic { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string TelNumber { get; set; }
        public string Status { get; set; }
    }
}
