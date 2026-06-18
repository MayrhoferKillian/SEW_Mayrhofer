using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesTrips.Models;

namespace RazorPagesTrips.Pages.TravelAgencyPages;

public class IndexModel : PageModel
{

    private readonly RazorPagesTripsContext _context;

    public IndexModel(RazorPagesTripsContext context)
    {
        _context = context;
    }

    public IList<TravelAgency> TravelAgencies { get; set; } = default!;

    public async Task OnGetAsync()
    {
        TravelAgencies = await _context.TravelAgency.ToListAsync();
    }
}
