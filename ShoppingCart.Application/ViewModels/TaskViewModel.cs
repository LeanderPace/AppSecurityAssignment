using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class TaskViewModel
    {
        public Guid id { get; set; }

        [Required(ErrorMessage = "Task name is required")]
        public string taskName { get; set; }

        [Required(ErrorMessage = "Task description is required")]
        public string description { get; set; }

        [Required(ErrorMessage = "Issue date is required")]
        public DateTime issueDate { get; set; }

        [Required(ErrorMessage = "Deadline date is required")]
        public DateTime deadline { get; set; }

        [Required(ErrorMessage = "Teacher name is required")]
        public string name { get; set; }

        [Required(ErrorMessage = "Teacher surname is required")]
        public string surname { get; set; }

        public string email { get; set; }
    }
}
