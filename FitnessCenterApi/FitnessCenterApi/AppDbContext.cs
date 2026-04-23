using FitnessCenterApi.Models;
using Microsoft.EntityFrameworkCore;


namespace FitnessCenterApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Course> Courses{ get; set; }
        public DbSet<Buchung> Buchungen{ get; set; }
        public DbSet<Member> Members{ get; set; }
    }
}
