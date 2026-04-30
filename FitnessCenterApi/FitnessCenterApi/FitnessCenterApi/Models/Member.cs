namespace FitnessCenterApi.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
