using AutoMapper;
using CarRentalApp.Models.Domain;
using CarRentalApp.Models.Dtos;

namespace CarRentalApp.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Car, CarDto>();
            CreateMap<Rental, RentalRequestDto>();
            CreateMap<RentalRequestDto, Rental>();
        }
    }
}
