using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class SubmissionsService : ISubmissionsService
    {
        private IMapper _autoMapper;
        private ISubmissionsRepository _submissionRepo;

        public SubmissionsService(IMapper autoMapper, ISubmissionsRepository submissionRepo)
        {
            _autoMapper = autoMapper;
            _submissionRepo = submissionRepo;
        }

        public void AddSubmission(SubmissionViewModel model)
        {
            var sub = _autoMapper.Map<Submission>(model);
            sub.TaskId = sub.task.id;
            sub.task = null;

            _submissionRepo.AddSubmission(sub);
        }
    
        public SubmissionViewModel GetSubmission(Guid id)
        {
            var submission = _submissionRepo.GetSubmission(id);

            if (submission == null)
                return null;
            else
            {
                var res = _autoMapper.Map<SubmissionViewModel>(submission);
                return res;
            }
        }

        public IQueryable<SubmissionViewModel> GetSubmissionsForStudent(string email)
        {
            return _submissionRepo.GetSubmissions().Where(e => e.email == email).ProjectTo<SubmissionViewModel>(_autoMapper.ConfigurationProvider);
        }

        public IQueryable<SubmissionViewModel> GetSubmissionsForTeacher(Guid id)
        {
            return _submissionRepo.GetSubmissions().Where(e => e.task.id == id).ProjectTo<SubmissionViewModel>(_autoMapper.ConfigurationProvider);
        }
    }
}
