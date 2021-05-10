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

        public string studentName { get; set; }

        public string studentSurname { get; set; }

        public string email { get; set; }

        public string file { get; set; }

        public string signature { get; set; }

        public virtual Task task { get; set; }

        [ForeignKey("Task")]
        public Guid TaskId { get; set; }
    }
}
