using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SubmissionsController : Controller
    {
        private readonly ISubmissionsService _submissionService;
        private readonly ITasksService _tasksService;
        private readonly IWebHostEnvironment _host;
        private readonly ILogger<SubmissionsController> _logger;

        public SubmissionsController(ISubmissionsService submissionService, ITasksService tasksService, 
            IWebHostEnvironment host, ILogger<SubmissionsController> logger)
        {
            _submissionService = submissionService;
            _tasksService = tasksService;
            _host = host;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var submissionList = _submissionService.GetSubmissions();
            return View(submissionList);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult Create(IFormFile file, SubmissionViewModel data, Guid id)
        {

            if (ModelState.IsValid)
            {
                data.task = _tasksService.GetTask(id);

                if(data.task.deadline > DateTime.Now)
                {
                    data.task = _tasksService.GetTask(id);

                    string uniqueFilename;

                    if (System.IO.Path.GetExtension(file.FileName) == ".pdf" && file.Length < 1048576)
                    {
                        byte[] whiteList = new byte[] { 37, 80, 68, 70 };
                        if (file != null)
                        {
                            using (var f = file.OpenReadStream())
                            {
                                byte[] buffer = new byte[4];
                                f.Read(buffer, 0, 4);

                                for (int i = 0; i < whiteList.Length; i++)
                                {
                                    if (whiteList[i] == buffer[i])
                                    { }
                                    else
                                    {
                                        ModelState.AddModelError("file", "file is not valid and accapteable");
                                        return View();
                                    }
                                }

                                f.Position = 0;

                                uniqueFilename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                                data.file = uniqueFilename;

                                string absolutePath = _host.WebRootPath + @"\files" + uniqueFilename;

                                try
                                {
                                    using (FileStream fsOut = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                                    {
                                        f.CopyTo(fsOut);
                                    }
                                    f.Close();
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Error happend while saving file");

                                    return View("Error", new ErrorViewModel() { Message = "Error while saving the file. Try again later" });
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("file", "File is not valid and acceptable or size is greater than 10Mb");
                        return View();
                    }

                    data.email = HttpContext.User.Identity.Name;

                    _submissionService.AddSubmission(data);

                    TempData["message"] = "Document submitted successfully";
                    return View();
                }
                else
                {
                    TempData["error"] = "Deadline date overdue";
                    return View();
                }
    
            }
            else
            {
                ModelState.AddModelError("", "Check your input. Operation failed");
                return View(data);
            }
        }

        [Authorize]
        public IActionResult Details(Guid id)
        {
            try
            {
                _logger.LogInformation("Accessing Information of Submission " + id);
                var mySubmission = _submissionService.GetSubmission(id);
                return View(mySubmission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error");
            }

        }
    }
}
