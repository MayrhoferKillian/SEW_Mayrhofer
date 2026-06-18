using Microsoft.EntityFrameworkCore;

public class RazorPagesTripsContext(DbContextOptions<RazorPagesTripsContext> options) : DbContext(options)
{
    public DbSet<RazorPagesTrips.Models.TravelAgency> TravelAgency { get; set; } = default!;
}
