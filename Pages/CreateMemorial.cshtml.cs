using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Pages
{
    public class CreateMemorialModel : PageModel
    {
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

        public IActionResult OnPost()
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

            // Here you would typically save to database
            // For now, we'll just redirect to a success page
            TempData["SuccessMessage"] = $"Memorial for {Name} has been created successfully!";
            
            return RedirectToPage("/Index");
        }
    }
}