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
    public class CommentsService : ICommentsService
    {
        private IMapper _autoMapper;
        private ICommentsRepository _commentRepo;

        public CommentsService(IMapper autoMapper, ICommentsRepository commentRepo)
        {
            _autoMapper = autoMapper;
            _commentRepo = commentRepo;
        }
        public void AddComment(CommentViewModel model, DateTime createdDate)
        {
            var comm = _autoMapper.Map<Comment>(model);
            comm.SubmissionId = comm.submission.id;
            comm.submission = null; 
            comm.commentDate = createdDate;

            _commentRepo.AddComment(comm);
        }

        public void DeleteComment(Guid id)
        {
            _commentRepo.DeleteComment(id);
        }

        public CommentViewModel GetComment(Guid id)
        {
            var comment = _commentRepo.GetComment(id);

            if (comment == null)
                return null;
            else
            {
                var res = _autoMapper.Map<CommentViewModel>(comment);
                return res;
            }
        }

        public IQueryable<CommentViewModel> GetComments(Guid id)
        {
            return _commentRepo.GetComments().Where(e => e.submission.id == id).ProjectTo<CommentViewModel>(_autoMapper.ConfigurationProvider);
        }
    }
}
