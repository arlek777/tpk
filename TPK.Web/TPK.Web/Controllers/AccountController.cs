using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPK.Web.Data;
using TPK.Web.Models;

namespace TPK.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly TPKDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public AccountController(TPKDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Login(Admin model)
        {
            var dbAdmin = _context.Admin.FirstOrDefault(a => a.UserName == model.UserName);
            if (dbAdmin != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(dbAdmin.HashPassword, model.Password);
                if(result == PasswordVerificationResult.Success)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        principal, new AuthenticationProperties { IsPersistent = true });

                    return RedirectToAction("Index", "Content");
                }
            }

            ViewBag.Error = "Not valid.";
            return View();
        }
    }
}
