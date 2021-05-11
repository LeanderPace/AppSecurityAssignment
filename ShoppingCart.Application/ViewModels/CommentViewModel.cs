using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class CommentViewModel
    {
        public Guid id { get; set; }

        [Required(ErrorMessage = "Comment body cannot be left empty")]
        public string content { get; set; }

        public DateTime commentDate { get; set; }

        public string commenterEmail { get; set; }

        public SubmissionViewModel submission { get; set; }
    }
}
