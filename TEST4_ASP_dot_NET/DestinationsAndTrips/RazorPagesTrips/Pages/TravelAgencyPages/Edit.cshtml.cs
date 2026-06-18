using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesTrips.Models;

namespace RazorPagesTrips.Pages.TravelAgencyPages;

public class EditModel : PageModel
{
    private readonly RazorPagesTripsContext _context;

    public EditModel(RazorPagesTripsContext context)
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
        TravelAgency = travelAgency;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(TravelAgency).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TravelAgencyExists(TravelAgency.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool TravelAgencyExists(int id)
    {
        return _context.TravelAgency.Any(e => e.Id == id);
    }
}
