using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlogApp.Models;
using System.Linq;

namespace BlogApp.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public UserController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = AuthenticateUser(model.Email, model.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1), 
                        AllowRefresh = true, 
                        IssuedUtc = DateTimeOffset.UtcNow,
                       
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("GetBlogPosts", "BlogPosts");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
            }

            return View(model);
        }

        private UserModel AuthenticateUser(string email, string password)
        {
            return _databaseContext.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        }
    }
}
