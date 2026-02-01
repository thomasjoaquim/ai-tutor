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
            ReturnUrl = returnUrl ?? "/Dashboard";
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "")
        {
            ReturnUrl = returnUrl ?? "/Dashboard";

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

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Input.RememberMe,
                ExpiresUtc = Input.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return LocalRedirect(ReturnUrl);
        }
    }
}