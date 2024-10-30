namespace CarRentalApp.Models.Domain
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public decimal DailyRate { get; set; }
        public bool IsAvailable { get; set; }
        public List<string> Features { get; set; }
    }
}