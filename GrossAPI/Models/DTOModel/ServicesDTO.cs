using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models.DTOModel
{
    public class ServicesDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        [Range(1, int.MaxValue)]
        public decimal Price { get; set; }      
        public Guid CategoryId { get; set; }
    }
}
