using AutoMapper;
using CarRentalApp.Models.Domain;
using CarRentalApp.Models.Dtos;

namespace CarRentalApp.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<Car, CarReturnDto>().ReverseMap();
            CreateMap<Rental, RentalRequestDto>().ReverseMap();
        }

    }
}
