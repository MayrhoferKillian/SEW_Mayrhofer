using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesTrips.Models;

namespace RazorPagesTrips.Pages.TravelAgencyPages;

public class DeleteModel : PageModel
{
    private readonly RazorPagesTripsContext _context;

    public DeleteModel(RazorPagesTripsContext context)
    {
        _context = context;
    }

    [BindProperty]
    public TravelAgency TravelAgency { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var travelAgency = await _context.TravelAgency.FirstOrDefaultAsync(m => m.Id == id);
        if (travelAgency is null)
        {
            return NotFound();
        }
        else
        {
            TravelAgency = travelAgency;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var travelAgency = await _context.TravelAgency.FindAsync(id);
        if (travelAgency != null)
        {
            TravelAgency = travelAgency;
            _context.TravelAgency.Remove(TravelAgency);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
