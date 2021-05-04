using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }
        public string content { get; set; }
        public DateTime commentDate { get; set; }

        public string commenterEmail { get; set; }

        public virtual Submission submission { get; set; }

        [ForeignKey("Submission")]
        public Guid SubmissionId { get; set; }
    }
}
