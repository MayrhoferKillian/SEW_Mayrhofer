namespace FitnessCenterApi.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int CourseId { get; set; }
        public DateTime CreatedAt { get; set; }

        //EF Navigation
        public Member? Member { get; set; }
        public Course? Course { get; set; }
    }
}
