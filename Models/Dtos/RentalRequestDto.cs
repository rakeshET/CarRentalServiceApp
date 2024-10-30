using System.ComponentModel.DataAnnotations;
namespace CarRentalApp.Models.Dtos
{
    public class RentalRequestDto
    {
        public int CarId { get; set; }
        public string CustomerName { get; set; }
        public string DrivingLicense { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}