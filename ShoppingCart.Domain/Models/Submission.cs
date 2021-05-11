using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [Required]
        public string studentName { get; set; }

        [Required]
        public string studentSurname { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string file { get; set; }

        [Required]
        public string signature { get; set; }

        [Required]
        public virtual Task task { get; set; }

        [ForeignKey("Task")]
        public Guid TaskId { get; set; }
    }
}
