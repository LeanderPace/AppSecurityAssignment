using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface ISubmissionsService
    {
        IQueryable<SubmissionViewModel> GetSubmissionsForTeacher(Guid id);
        IQueryable<SubmissionViewModel> GetSubmissionsForStudent(string email);
        SubmissionViewModel GetSubmission(Guid id);
        void AddSubmission(SubmissionViewModel model);
    }
}
