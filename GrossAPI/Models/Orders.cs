﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrossAPI.Models
{
    public class Orders
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public Guid ServiceId { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Services Services { get; set; }

        [ForeignKey("StatusId")]
        public virtual Statuses Statuses { get; set; }

        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
