using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models.DTOModel
{
    public class ReportDTO
    {
        public string? Header { get; set; }
        public string? Position { get; set; }
        public string? Description { get; set; }
    }
}
