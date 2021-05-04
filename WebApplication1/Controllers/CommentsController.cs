﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

namespace WebApplication1.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentsService _commentService;
        private readonly ISubmissionsService _submissionService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentsService commentService, ISubmissionsService submissionService, ILogger<CommentsController> logger)
        {
            _commentService = commentService;
            _submissionService = submissionService;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index(Guid id)
        {
            var commentList = _commentService.GetComments(id);
            return View(commentList);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(Guid id)
        {
            var comments = _commentService.GetComments(id);
            ViewBag.Comments = comments;
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CommentViewModel data, Guid id)
        {
            var comments = _commentService.GetComments(id);
            ViewBag.Comments = comments;

            DateTime createdDate = System.DateTime.Now;

            string commenterEmail = User.Identity.Name;

            if (ModelState.IsValid)
            {
                data.submission = _submissionService.GetSubmission(id);

                _commentService.AddComment(data, createdDate, commenterEmail);
                
                TempData["Message"] = "Comment posted successfully";
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Error posting comment");
                return View(data);
            }
        }
    }
}
