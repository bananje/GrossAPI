using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrossAPI.Models
{
    public class Responses
    {
        [Key]
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ServiceId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Orders Order { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public virtual Services Service { get; set; }

    }
}
