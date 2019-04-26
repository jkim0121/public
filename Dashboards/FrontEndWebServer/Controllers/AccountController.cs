using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Runtime.Serialization.Json;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using log4net;

using Deg.FrontEndWebServer.Models;

namespace Deg.FrontEndWebServer.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : Controller
    {
        private ILog _log = LogManager.GetLogger(typeof(AccountController));

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {

            ViewBag.LoginUrl = "http://localhost:54312/account/login";

            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [Route("login")]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                var address = default(IPAddress);
                try
                {
                    address = new IPAddress(Request.UserHostAddress.Split('.').Select(part => Convert.ToByte(part)).ToArray());
                }
                catch
                {
                    address = new IPAddress(new byte[] { 127, 0, 0, 1, });
                }

                var sessionID = WebShared.Instance.RegisterSession(model.Username, model.Password, address, Request.UserAgent, false);
                if (sessionID != Guid.Empty)
                {
                    IdentitySignin(model.Username, model.Password, null, false);
                    return RedirectToAction("Index", "Home", new { sessionID = sessionID, });
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return View();
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //        //
        //        // POST: /Account/ForgotPassword
        //        [HttpPost]
        //        [AllowAnonymous]
        //        [ValidateAntiForgeryToken]
        //        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var user = await UserManager.FindByNameAsync(model.Email);
        //                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //                {
        //                    // Don't reveal that the user does not exist or is not confirmed
        //                    return View("ForgotPasswordConfirmation");
        //                }

        //                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //                // Send an email with this link
        //                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
        //                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //            }

        //            // If we got this far, something failed, redisplay form
        //            return View(model);
        //        }

        //        //
        //        // GET: /Account/ForgotPasswordConfirmation
        //        [AllowAnonymous]
        //        public ActionResult ForgotPasswordConfirmation()
        //        {
        //            return View();
        //        }

        //        //
        //        // GET: /Account/ResetPassword
        //        [AllowAnonymous]
        //        public ActionResult ResetPassword(string code)
        //        {
        //            return code == null ? View("Error") : View();
        //        }

        //        //
        //        // POST: /Account/ResetPassword
        //        [HttpPost]
        //        [AllowAnonymous]
        //        [ValidateAntiForgeryToken]
        //        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return View(model);
        //            }
        //            var user = await UserManager.FindByNameAsync(model.Email);
        //            if (user == null)
        //            {
        //                // Don't reveal that the user does not exist
        //                return RedirectToAction("ResetPasswordConfirmation", "Account");
        //            }
        //            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("ResetPasswordConfirmation", "Account");
        //            }
        //            AddErrors(result);
        //            return View();
        //        }

        //        //
        //        // GET: /Account/ResetPasswordConfirmation
        //        [AllowAnonymous]
        //        public ActionResult ResetPasswordConfirmation()
        //        {
        //            return View();
        //        }

        //        //
        //        // POST: /Account/ExternalLogin
        //        [HttpPost]
        //        [AllowAnonymous]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult ExternalLogin(string provider, string returnUrl)
        //        {
        //            // Request a redirect to the external login provider
        //            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //        }

        //        //
        //        // GET: /Account/SendCode
        //        [AllowAnonymous]
        //        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        //        {
        //            var userId = await SignInManager.GetVerifiedUserIdAsync();
        //            if (userId == null)
        //            {
        //                return View("Error");
        //            }
        //            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //        }

        //        //
        //        // POST: /Account/SendCode
        //        [HttpPost]
        //        [AllowAnonymous]
        //        [ValidateAntiForgeryToken]
        //        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return View();
        //            }

        //            // Generate the token and send it
        //            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        //            {
        //                return View("Error");
        //            }
        //            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        //        }

        //        //
        //        // POST: /Account/LogOff
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public ActionResult LogOff()
        //        {
        //            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //            return RedirectToAction("Index", "Home");
        //        }

        //        protected override void Dispose(bool disposing)
        //        {
        //            if (disposing)
        //            {
        //                if (_userManager != null)
        //                {
        //                    _userManager.Dispose();
        //                    _userManager = null;
        //                }

        //                if (_signInManager != null)
        //                {
        //                    _signInManager.Dispose();
        //                    _signInManager = null;
        //                }
        //            }

        //            base.Dispose(disposing);
        //        }

        #region Helpers
        //        // Used for XSRF protection when adding external logins
        //        private const string XsrfKey = "XsrfId";

        //        private IAuthenticationManager AuthenticationManager
        //        {
        //            get
        //            {
        //                return HttpContext.GetOwinContext().Authentication;
        //            }
        //        }

        //        private void AddErrors(IdentityResult result)
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error);
        //            }
        //        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        //        internal class ChallengeResult : HttpUnauthorizedResult
        //        {
        //            public ChallengeResult(string provider, string redirectUri)
        //                : this(provider, redirectUri, null)
        //            {
        //            }

        //            public ChallengeResult(string provider, string redirectUri, string userId)
        //            {
        //                LoginProvider = provider;
        //                RedirectUri = redirectUri;
        //                UserId = userId;
        //            }

        //            public string LoginProvider { get; set; }
        //            public string RedirectUri { get; set; }
        //            public string UserId { get; set; }

        //            public override void ExecuteResult(ControllerContext context)
        //            {
        //                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        //                if (UserId != null)
        //                {
        //                    properties.Dictionary[XsrfKey] = UserId;
        //                }
        //                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //            }
        //}
        #endregion

        public void IdentitySignin(string userId, string name, string providerKey = null, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            // create *required* claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            claims.Add(new Claim(ClaimTypes.Name, name));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            // add to user here!
            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
            }, identity);
        }

        public void IdentitySignout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                          DefaultAuthenticationTypes.ExternalCookie);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}