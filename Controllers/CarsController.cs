using AutoMapper;
using CarRentalApp.Data;
using CarRentalApp.Models.Domain;
using CarRentalApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarRentalController : ControllerBase
    {
        private readonly CarRentalDbContext dbContext;
        private readonly IMapper mapper;

        public CarRentalController(CarRentalDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet("cars")]
        public IActionResult GetAvailableCars([FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate,
            [FromQuery] string carType, [FromQuery] decimal? maxDailyRate)
        {
            var query = dbContext.Cars.AsQueryable();
            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(c => !dbContext.Rentals.Any(r =>
                    c.Id == r.CarId &&
                    ((startDate >= DateOnly.FromDateTime(r.StartDate) &&
                      startDate <= DateOnly.FromDateTime(r.EndDate)) ||
                     (endDate >= DateOnly.FromDateTime(r.StartDate) &&
                      endDate <= DateOnly.FromDateTime(r.EndDate)))));
            }
            if (!string.IsNullOrEmpty(carType))
            {
                query = query.Where(c => c.Type == carType);
            }
            if (maxDailyRate.HasValue)
            {
                query = query.Where(c => c.DailyRate <= maxDailyRate);
            }
            var availableCars = query.ToList();
            var carDtos = mapper.Map<List<CarDto>>(availableCars);
            return Ok(new { availableCars = carDtos });
        }
        [HttpGet("{id}")]
        public IActionResult GetRentalById(int id)
        {
            var rental = dbContext.Rentals.FirstOrDefault(r => r.Id == id);
            if (rental == null)
            {
                return NotFound();
            }
            return Ok(rental);
        }

        [HttpPost("rentals")]
public IActionResult CreateRental([FromBody] RentalRequestDto rentalRequest)
{
    var car = dbContext.Cars.FirstOrDefault(c => c.Id == rentalRequest.CarId);
    if (car == null)
    {
        return NotFound("Car not found");
    }

    var startDate = DateTime.SpecifyKind(rentalRequest.StartDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
    var endDate = DateTime.SpecifyKind(rentalRequest.EndDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

    if ((endDate - startDate).TotalDays < 1)
    {
        return BadRequest("Minimum rental period is 1 day");
    }

    var isCarAvailable = !dbContext.Rentals.Any(r =>
        r.CarId == rentalRequest.CarId &&
        ((startDate >= r.StartDate && startDate <= r.EndDate) ||
         (endDate >= r.StartDate && endDate <= r.EndDate)));
    if (!isCarAvailable)
    {
        return BadRequest("Car is not available for the selected dates");
    }

    var rental = new Rental
    {
        CarId = rentalRequest.CarId,
        CustomerName = rentalRequest.CustomerName,
        DrivingLicense = rentalRequest.DrivingLicense,
        StartDate = startDate,
        EndDate = endDate,
        TotalCost = CalculateRentalCost(car.DailyRate, startDate, endDate)
    };

    dbContext.Rentals.Add(rental);
    dbContext.SaveChanges();

    return CreatedAtAction(nameof(GetRentalById), new { id = rental.Id }, rental);
}

        [HttpPut("rentals/{id}/return")]
        public IActionResult ProcessReturn(int id, [FromBody] CarReturnDto returnDto)
        {
            var rental = dbContext.Rentals
                .Include(r => r.Car)
                .FirstOrDefault(r => r.Id == id);
            if (rental == null)
            {
                return NotFound();
            }
            decimal additionalCharges = 0;
            var rentalEndDate = DateOnly.FromDateTime(rental.EndDate);
            
            if (returnDto.ReturnDate > rentalEndDate)
            {
                var lateDays = returnDto.ReturnDate.DayNumber - rentalEndDate.DayNumber;
                additionalCharges += lateDays * rental.Car.DailyRate * 1.5m;
            }
            rental.ReturnDate = DateTime.SpecifyKind(returnDto.ReturnDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            rental.ReturnFuelLevel = returnDto.CurrentFuelLevel;
            rental.ReturnMileage = returnDto.Mileage;
            rental.TotalCost += additionalCharges;
            dbContext.SaveChanges();
            return Ok(new
            {
                rentalId = rental.Id,
                totalCost = rental.TotalCost,
                additionalCharges = additionalCharges
            });
        }
        [HttpGet("cars/statistics")]
        public IActionResult GetStatistics([FromQuery] string period)
        {
            
            if (!DateOnly.TryParse($"{period}-01", out DateOnly startDate))
            {
                return BadRequest("Invalid period format. Use YYYY-MM format.");
            }
            
            var endDate = startDate.AddMonths(1);
            
            var startDateTime = DateTime.SpecifyKind(startDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            var endDateTime = DateTime.SpecifyKind(endDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            var rentals = dbContext.Rentals
                .Include(r => r.Car)
                .Where(r => r.StartDate >= startDateTime && r.StartDate < endDateTime)
                .ToList();
            var statistics = new RentalStatisticsDto
            {
                Period = period,
                TotalRentals = rentals.Count,
                Revenue = rentals.Sum(r => r.TotalCost),
                PopularCars = rentals.GroupBy(r => r.Car.Model)
                    .Select(g => new PopularCarDto
                    {
                        Model = g.Key,
                        RentalCount = g.Count(),
                        Utilization = CalculateUtilization(g.ToList(), startDateTime, endDateTime)
                    })
                    .OrderByDescending(c => c.RentalCount)
                    .ToList()
            };
            return Ok(statistics);
        }

        private decimal CalculateRentalCost(decimal dailyRate, DateTime startDate, DateTime endDate)
        {
            
            var start = DateOnly.FromDateTime(startDate);
            var end = DateOnly.FromDateTime(endDate);
            var days = end.DayNumber - start.DayNumber + 1;
            return dailyRate * days;
        }
        private double CalculateUtilization(List<Rental> rentals, DateTime startDate, DateTime endDate)
        {
            var start = DateOnly.FromDateTime(startDate);
            var end = DateOnly.FromDateTime(endDate);
            var totalDays = end.DayNumber - start.DayNumber;
            var rentedDays = rentals.Sum(r =>
            {
                var rentalStart = DateOnly.FromDateTime(r.StartDate);
                var rentalEnd = DateOnly.FromDateTime(r.EndDate);
                return rentalEnd.DayNumber - rentalStart.DayNumber + 1;
            });
            return totalDays > 0 ? (rentedDays / (double)totalDays) * 100 : 0;
        }
    }
}