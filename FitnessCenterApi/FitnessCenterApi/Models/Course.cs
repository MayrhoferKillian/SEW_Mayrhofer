namespace FitnessCenterApi.Models
{
    public class Course
    {
        public int id { get; set; }
        public string bezeichnung { get; set; }
        public DateTime veranstaltungszeitpunkt { get; set; }
        public int maxTeilnehmer { get; set; }
        public List<string> buchungsliste { get; set; }
    }
}
