using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesTrips.Models;

namespace RazorPagesTrips.Pages.TravelAgencyPages;

public class CreateModel : PageModel
{
    private readonly RazorPagesTripsContext _context;

    public CreateModel(RazorPagesTripsContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public TravelAgency TravelAgency { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.TravelAgency.Add(TravelAgency);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
