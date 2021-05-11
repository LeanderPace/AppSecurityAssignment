using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.ActionFilters;
using WebApplication1.Utility;

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

        [HttpGet]
        [Authorize]
        [Comments]
        public IActionResult Create(string id)
        {
            try
            {
                string urlEnc = Encryption.SymmetricDecrypt(id);
                Guid decId = Guid.Parse(urlEnc);
                var comments = _commentService.GetComments(decId);
                ViewBag.Comments = comments;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize]
        [Comments]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommentViewModel data, string id)
        {
            try
            {
                string urlEnc = Encryption.SymmetricDecrypt(id);
                Guid decId = Guid.Parse(urlEnc);

                var comments = _commentService.GetComments(decId);
                ViewBag.Comments = comments;

                DateTime createdDate = DateTime.Now;

                string commenterEmail = User.Identity.Name;

                data.submission = _submissionService.GetSubmission(decId);

                _commentService.AddComment(data, createdDate, commenterEmail);

                TempData["Message"] = "Comment posted successfully";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error");
            }
        }
        public static string GetIpAddress()
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            return addr[1].ToString();
        }
    }
}
