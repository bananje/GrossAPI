using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models.ViewModel
{
    public class ServiceVM
    {
        public string Id { get; set; }
        public string Title { get; set; }
        [Range(1, int.MaxValue)]
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
