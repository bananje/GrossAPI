using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models.DTOModel
{
    public class FeedbackOrdersDTO
    {             
        public string FullName { get; set; }
        public string Email { get; set; }
        public string TelNumber { get; set; } 
    }
}
