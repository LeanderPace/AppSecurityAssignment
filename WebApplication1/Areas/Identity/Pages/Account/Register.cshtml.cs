using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using WebApplication1.Models;
using WebApplication1.Utility;
using static WebApplication1.Utility.Encryption;

namespace WebApplication1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMembersService _membersService;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMembersService membersService,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _membersService = membersService;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            public string Password { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName};

                Input.Password = RandomString();

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "student");

                    AsymmetricKeys encKeys = new AsymmetricKeys();
                    encKeys = GenerateAsymmetricKeys();

                    MemberViewModel member = new MemberViewModel
                    {
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        LecturerEmail = User.Identity.Name,
                        PublicKey = encKeys.PublicKey,
                        PrivateKey = encKeys.PrivateKey
                    };

                    _membersService.AddMember(member);

                    SendEmail(Input.Email, Input.Password);

                    /*_logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }*/


                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public void SendEmail(string toEmail, string password)
        {
            string to = toEmail;
            string from = "leanderpace62a@outlook.com";

            MailMessage message = new MailMessage(from, to);

            string bodyContent = "Your student account has just been created. Please log in using the following \n\nEmail: " + toEmail + "\nPassword: " + password;

            string mailbody = bodyContent;
            message.Subject = "Student Account";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587); 
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential(from, "mcast278395");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string RandomString()
        {
            int length = 7;

            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string specialChars = "#@&%!*^$";
            const string numbers = "0123456789";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }
            for (int i = 0; i < 2; i++)
            {
                int index = rnd.Next(numbers.Length);
                sb.Append(numbers[index]);
            }
            for (int i = 0; i < 1; i++)
            {
                int index = rnd.Next(specialChars.Length);
                sb.Append(specialChars[index]);
            }

            return sb.ToString();
        }
    }
}
