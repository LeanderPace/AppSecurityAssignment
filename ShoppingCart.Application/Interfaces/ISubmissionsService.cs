using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface ISubmissionsService
    {
        IQueryable<SubmissionViewModel> GetSubmissions();
        SubmissionViewModel GetSubmission(Guid id);
        void AddSubmission(SubmissionViewModel model);
    }
}
