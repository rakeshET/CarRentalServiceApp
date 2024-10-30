namespace CarRentalApp.Models.Dtos
{
    public class PopularCarDto
    {
        public string Model { get; set; }
        public int RentalCount { get; set; }
        public double Utilization { get; set; }
    }
}