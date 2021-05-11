using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [Required]
        public string taskName { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public DateTime issueDate { get; set; }

        [Required]
        public DateTime deadline { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string surname { get; set; }

        [Required]
        public string email { get; set; }
    }
}
