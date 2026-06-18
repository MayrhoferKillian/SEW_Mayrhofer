using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesTrips.Models;

namespace RazorPagesTrips.Pages.TravelAgencyPages;

public class DetailsModel : PageModel
{
    private readonly RazorPagesTripsContext _context;

    public DetailsModel(RazorPagesTripsContext context)
    {
        _context = context;
    }

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
}
