namespace CarRentalApp.Models.Dtos
{
    public class CarReturnDto
    {
        public DateOnly ReturnDate { get; set; }
        public decimal CurrentFuelLevel { get; set; }
        public int Mileage { get; set; }
    }
}