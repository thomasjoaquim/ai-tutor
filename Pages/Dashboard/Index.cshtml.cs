using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyProject.Models;
using MyProject.Services;
using System.Security.Claims;

namespace MyProject.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMemorialService _memorialService;

        public IndexModel(IMemorialService memorialService)
        {
            _memorialService = memorialService;
        }

        public List<Memorial> Memorials { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                Memorials = await _memorialService.GetUserMemorialsAsync(userId);
            }
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                await _memorialService.DeleteMemorialAsync(id, userId);
            }
            return RedirectToPage();
        }
    }
}