using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyProject.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MyProject.Pages
{
    [Authorize]
    public class EditMemorialModel : PageModel
    {
        private readonly IMemorialService _memorialService;

        public EditMemorialModel(IMemorialService memorialService)
        {
            _memorialService = memorialService;
        }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        [Required]
        public string Name { get; set; } = "";

        [BindProperty]
        [Required]
        public DateTime BirthDate { get; set; }

        [BindProperty]
        [Required]
        public DateTime DeathDate { get; set; }

        [BindProperty]
        public string Biography { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var memorial = await _memorialService.GetMemorialByIdAsync(id);
            
            if (memorial == null || memorial.UserId != userId)
            {
                return RedirectToPage("/Dashboard/Index");
            }

            Id = memorial.Id;
            Name = memorial.Name;
            BirthDate = memorial.BirthDate;
            DeathDate = memorial.DeathDate;
            Biography = memorial.Biography ?? "";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var memorial = await _memorialService.GetMemorialByIdAsync(Id);
            
            if (memorial == null || memorial.UserId != userId)
            {
                return RedirectToPage("/Dashboard/Index");
            }

            memorial.Name = Name;
            memorial.BirthDate = BirthDate;
            memorial.DeathDate = DeathDate;
            memorial.Biography = Biography;

            await _memorialService.UpdateMemorialAsync(memorial);

            return RedirectToPage("/Dashboard/Index");
        }
    }
}