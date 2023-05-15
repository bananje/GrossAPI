using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace GrossAPI.Models
{
    public class Images
    {
        [Key]
        public Guid Id { get; set; }
        public string IndexImg { get; set; }
        public string Extension { get; set; }
        public Guid? PostId { get; set; }
        public Guid? ReportId { get; set; }

        [ForeignKey("PostId")]
        public virtual Posts Posts { get; set; }

        [ForeignKey("ReportId")]
        public virtual Reports Reports { get; set; }
    }
}
