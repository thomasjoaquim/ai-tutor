using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyProject.Models;
using MyProject.Services;

namespace MyProject.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly IAdminService _adminService;

        public DashboardModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public AdminDashboardStats Stats { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public List<Memorial> Memorials { get; set; } = new();
        public List<TokenUsage> TokenUsages { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Stats = await _adminService.GetDashboardStatsAsync();
            Users = await _adminService.GetAllUsersAsync();
            Memorials = await _adminService.GetAllMemorialsAsync();
            TokenUsages = await _adminService.GetAllTokenUsageAsync();

            return Page();
        }
    }
}