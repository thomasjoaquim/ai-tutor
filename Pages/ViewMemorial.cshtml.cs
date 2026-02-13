using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MyProject.Services;
using MyProject.Models;

namespace MyProject.Pages
{
    public class ViewMemorialModel : PageModel
    {
        private readonly IMemorialService _memorialService;

        public ViewMemorialModel(IMemorialService memorialService)
        {
            _memorialService = memorialService;
        }

        public Memorial? Memorial { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Memorial = await _memorialService.GetMemorialByUniqueIdAsync(id);

            if (Memorial == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
