using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPK.Web.Data;
using TPK.Web.Models;

namespace TPK.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly TPKDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthorizationService _service;

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
        public IActionResult Login(Admin model)
        {
            var admin = _context.Admin.FirstOrDefault(a => a.UserName == model.UserName);
            if (admin != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(admin.Password, model.Password);
                if(result == PasswordVerificationResult.Success)
                {
                    var claims = new ClaimsPrincipal(new ClaimsIdentity[] { new ClaimsIdentity() { Label = model.UserName } });
                    HttpContext.SignInAsync(claims);
                    return RedirectToAction("Index", "Content");
                }
            }

            return View();
        }
    }
}
