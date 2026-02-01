using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyProject.Models;
using MyProject.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MyProject.Pages
{
    [Authorize]
    public class CreateMemorialModel : PageModel
    {
        private readonly IMemorialService _memorialService;

        public CreateMemorialModel(IMemorialService memorialService)
        {
            _memorialService = memorialService;
        }
        [BindProperty]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Death date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Death")]
        public DateTime DeathDate { get; set; }

        [BindProperty]
        [Display(Name = "Biography")]
        public string Biography { get; set; } = "";

        public void OnGet()
        {
            // Initialize default values if needed
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validate that death date is after birth date
            if (DeathDate <= BirthDate)
            {
                ModelState.AddModelError("DeathDate", "Death date must be after birth date");
                return Page();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var memorial = new Memorial
            {
                UserId = userId,
                Name = Name,
                BirthDate = BirthDate,
                DeathDate = DeathDate,
                Biography = Biography ?? ""
            };

            await _memorialService.CreateMemorialAsync(memorial);
            
            TempData["SuccessMessage"] = $"Memorial for {Name} has been created successfully!";
            
            return RedirectToPage("/Dashboard/Index");
        }
    }
}