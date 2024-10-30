namespace CarRentalApp.Models.Dtos
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public decimal DailyRate { get; set; }
        public List<string> Features { get; set; }
    }
}