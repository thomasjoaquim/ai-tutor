using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyProject.Models;
using MyProject.Services;
using System.Security.Claims;

namespace MyProject.Pages.Admin
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;

        public DashboardModel(IAdminService adminService, IUserService userService)
        {
            _adminService = adminService;
            _userService = userService;
        }

        public AdminDashboardStats Stats { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public List<Memorial> Memorials { get; set; } = new();
        public List<TokenUsage> TokenUsages { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var currentUser = await _userService.GetUserByIdAsync(userId);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                return RedirectToPage("/Dashboard/Index");
            }

            Stats = await _adminService.GetDashboardStatsAsync();
            Users = await _adminService.GetAllUsersAsync();
            Memorials = await _adminService.GetAllMemorialsAsync();
            TokenUsages = await _adminService.GetAllTokenUsageAsync();

            return Page();
        }
    }
}