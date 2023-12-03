using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Supplier
            this.CreateMap<ImportSupplierDto, Supplier>();
            this.CreateMap<Supplier, ExportSupplierDto>()
                .ForMember(d => d.PartsCount, opt => opt.MapFrom(s => s.Parts.Count));

            //Part
            this.CreateMap<ImportPartDto, Part>()
                .ForMember(d => d.SupplierId, opt => opt.MapFrom(s => s.SupplierId.Value));
            this.CreateMap<Part, ExportPartDto>();

            //Car
            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());
            this.CreateMap<Car, ExportCarsDto>();
            this.CreateMap<Car, ExportBmwCarDto>();
            this.CreateMap<Car, ExportCarWithPartDto>()
                .ForMember(d => d.Parts, opt => opt.MapFrom(s => s.PartsCars.Select(p => p.Part).OrderByDescending(p => p.Price)));
            this.CreateMap<Car, ExportCarForSaleDto>();

            //Customer
            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate)));
            this.CreateMap<Customer, ExportCustomerDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BoughtCars, opt => opt.MapFrom(src => src.Sales.Count))
                .ForMember(dest => dest.SpentMoney,
                opt => opt.MapFrom(src => src.Sales.Sum(s => s.Car.PartsCars.Sum(pc => pc.Part.Price))));


            //Sale
            this.CreateMap<ImportSaleDto, Sale>();
            CreateMap<Sale, ExportSalesDto>()
               .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Car.PartsCars.Sum(pc => pc.Part.Price)))
               .ForMember(dest => dest.PriceWithDiscount, opt => opt.MapFrom(src =>
                   src.Car.PartsCars.Sum(pc => pc.Part.Price) - (src.Car.PartsCars.Sum(pc => pc.Part.Price) * src.Discount / 100)));
        }
    }
}
