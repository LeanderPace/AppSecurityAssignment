using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class CommentViewModel
    {
        public Guid id { get; set; }

        public string content { get; set; }

        public DateTime commentDate { get; set; }

        public SubmissionViewModel submission { get; set; }
    }
}
