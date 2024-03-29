﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.ActionFilters;
using WebApplication1.Models;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class SubmissionsController : Controller
    {
        private readonly ISubmissionsService _submissionService;
        private readonly ITasksService _tasksService;
        private readonly IMembersService _membersService;
        private readonly IWebHostEnvironment _host;
        private readonly ILogger<SubmissionsController> _logger;

        public SubmissionsController(ISubmissionsService submissionService, ITasksService tasksService,
            IWebHostEnvironment host, ILogger<SubmissionsController> logger, IMembersService membersService)
        {
            _submissionService = submissionService;
            _tasksService = tasksService;
            _membersService = membersService;
            _host = host;
            _logger = logger;
        }

        [Authorize]
        public IActionResult TeacherSubmissions(string id)
        {
            try
            {
                string urlEnc = Encryption.SymmetricDecrypt(id);
                Guid decId = Guid.Parse(urlEnc);

                var submissionList = _submissionService.GetSubmissionsForTeacher(decId);
                return View(submissionList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error", "Home");
            }
        }

        [ViewOwnWork]
        public IActionResult StudentSubmission()
        {
            try
            {
                string email = User.Identity.Name;

                var submissionList = _submissionService.GetSubmissionsForStudent(email);
                return View(submissionList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult Create(IFormFile file, SubmissionViewModel data, string id)
        {
            try
            {
                string urlEnc = Encryption.SymmetricDecrypt(id);
                Guid decId = Guid.Parse(urlEnc);

                var memId = _membersService.GetMember(User.Identity.Name);

                if (file != null)
                {
                    data.task = _tasksService.GetTask(decId);

                    if (data.task.deadline > DateTime.Now)
                    {
                        string uniqueFilename;

                        if (System.IO.Path.GetExtension(file.FileName) == ".pdf" && file.Length < 1048576)
                        {
                            byte[] whiteList = new byte[] { 37, 80, 68, 70 };
                            if (file != null)
                            {
                                MemoryStream ms = new MemoryStream();

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
                                            ModelState.AddModelError("file", "Invalid file");
                                            _logger.LogError("Invalid file |" + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                                            return View();
                                        }
                                    }

                                    f.Position = 0;

                                    uniqueFilename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                                    data.file = uniqueFilename;

                                    string absolutePath = @"ValuableFiles\" + uniqueFilename;

                                    try
                                    {
                                        file.CopyTo(ms);
                                        var encFile = Encryption.HybridEncrypt(ms, memId.PublicKey);
                                        data.signature = Encryption.SignData(ms, memId.PrivateKey);
                                        System.IO.File.WriteAllBytes(absolutePath, encFile.ToArray());
                                        f.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Error happend while saving file |" + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                                        return View("Error", new ErrorViewModel() { Message = "Error while saving the file. Try again later" });
                                    }
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("file", "File is not valid or size is greater than 10Mb");
                            _logger.LogError("File is not valid or size is greater than 10Mb |" + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                            return View();
                        }

                        data.email = HttpContext.User.Identity.Name;

                        _submissionService.AddSubmission(data);

                        TempData["message"] = "Document submitted successfully";
                        _logger.LogInformation("File uploaded successfully | ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                        return View();
                    }
                    else
                    {
                        TempData["error"] = "Deadline date overdue";
                        _logger.LogError("Deadline date overdue |" + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("file", "Check your input. Operation failed");
                    return View(data);
                }
            }   
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error");
            }
        }

        [Authorize]
        public IActionResult DownloadFile(string id)
        {
            try
            {
                string urlEnc = Encryption.SymmetricDecrypt(id);
                Guid decId = Guid.Parse(urlEnc);

                var subId = _submissionService.GetSubmission(decId);
                string absolutePath = @"ValuableFiles\" + subId.file;

                FileStream fs = new FileStream(absolutePath, FileMode.Open, FileAccess.Read);
                MemoryStream ms = new MemoryStream();

                fs.CopyTo(ms);

                var member = _membersService.GetMember(subId.email);
                MemoryStream downloadedFile = Encryption.HybridDecrypt(ms, member.PrivateKey);

                return File(downloadedFile, "application/ocet-stream", Guid.NewGuid() + ".pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                return RedirectToAction("Error");
            }
        } 

        [Authorize]
        public IActionResult Details(string id)
        {
            try
            {
                string urlEnc = Encryption.SymmetricDecrypt(id);
                Guid decId = Guid.Parse(urlEnc);

                _logger.LogInformation("Accessing Information of Submission |" + " ip: " + GetIpAddress() + " | Timestamp: " + DateTime.Now + " | Email: " + User.Identity.Name);
                var mySubmission = _submissionService.GetSubmission(decId);
                return View(mySubmission);
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
