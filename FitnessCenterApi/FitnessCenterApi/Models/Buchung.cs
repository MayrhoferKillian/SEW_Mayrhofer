namespace FitnessCenterApi.Models
{
    public class Buchung
    {
        public int buchungsid { get; set; }
        public int memberid { get; set; }
        public int courseid { get; set; }
        public DateTime erstellungsDatum { get; set; }

        //EF Navigation
        public Member? Member { get; set; }
        public Course? Course { get; set; }
    }
}
