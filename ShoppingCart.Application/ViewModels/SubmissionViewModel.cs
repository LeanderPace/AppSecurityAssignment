using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class SubmissionViewModel
    {
        public Guid id { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        public string studentName { get; set; }

        [Required(ErrorMessage = "Student surname is required")]
        public string studentSurname { get; set; }

        public string email { get; set; }

        public string file { get; set; }

        public string signature { get; set; }

        public TaskViewModel task { get; set; }
    }
}
