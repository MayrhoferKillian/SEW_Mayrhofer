using System.ComponentModel.DataAnnotations;

namespace RazorPagesTrips.Models
{
    public class TravelAgency
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Country { get; set; }

        [Range(1.0, 5.0)]
        public double Rating { get; set; }
    }
}
