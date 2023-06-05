using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models.DTOModel
{
    public class FeedbackOrdersDTO
    {             
        public string FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string TelNumber { get; set; }
        public string Status { get; set; }      
    }
}
