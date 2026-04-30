namespace FitnessCenterApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set;}
        public int MaxParticipants { get; set; }
        public List<Booking> Bookings{ get; set; } = new List<Booking>();
    }
}
