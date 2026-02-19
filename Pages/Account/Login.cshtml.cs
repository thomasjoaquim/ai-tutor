using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyProject.Models;
using MyProject.Services;
using System.Security.Claims;

namespace MyProject.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public MyProject.Models.LoginModel Input { get; set; } = new();

        public string ReturnUrl { get; set; } = "";

        public void OnGet(string returnUrl = "")
        {
            ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "/Dashboard" : returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "")
        {
            ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "/Dashboard" : returnUrl;

            if (!ModelState.IsValid)
                return Page();

            var user = await _userService.AuthenticateAsync(Input.Email, Input.Password);
            
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };
            
            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Input.RememberMe,
                ExpiresUtc = Input.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), authProperties);

            if (string.IsNullOrEmpty(ReturnUrl) || ReturnUrl == "/")
            {
                return RedirectToPage("/Dashboard/Index");
            }
            
            return LocalRedirect(ReturnUrl);
        }
    }
}