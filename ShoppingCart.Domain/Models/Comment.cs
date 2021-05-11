using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [Required]
        public string content { get; set; }
        public DateTime commentDate { get; set; }

        public string commenterEmail { get; set; }

        [Required]
        public virtual Submission submission { get; set; }

        [ForeignKey("Submission")]
        public Guid SubmissionId { get; set; }
    }
}
