namespace MyCityProject.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string? Title { get; set; }   // помечаем "?" чтобы разрешить null
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}
