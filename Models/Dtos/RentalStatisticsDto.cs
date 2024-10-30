namespace CarRentalApp.Models.Dtos
{
    public class RentalStatisticsDto
    {
        public string Period { get; set; }
        public int TotalRentals { get; set; }
        public decimal Revenue { get; set; }
        public List<PopularCarDto> PopularCars { get; set; }
    }
}