namespace CarRentalApp.Models.Domain
{
    public class Rental
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CustomerName { get; set; }
        public string DrivingLicense { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public decimal? ReturnFuelLevel { get; set; }
        public int? ReturnMileage { get; set; }
        public virtual Car Car { get; set; }
    }
}