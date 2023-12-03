using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDto, User>();

            this.CreateMap<ImportProductDto, Product>()
                .ForMember(d => d.BuyerId, opt => opt.MapFrom(s => s.BuyerId.Value));

            this.CreateMap<ImportCategoryDto, Category>();

            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();

            this.CreateMap<Product, ExportProductDto>()
                .ForMember(d => d.Buyer, opt => opt.MapFrom(s => (s.Buyer.FirstName + " " + s.Buyer.LastName) ?? null));

            this.CreateMap<Product, ExportSoldProductsDto>();

            this.CreateMap<User, ExportUserDto>()
                .ForMember(d => d.ProductsSold, opt => opt.MapFrom(s => s.ProductsSold));

            this.CreateMap<Category, ExportCategoryDto>()
                .ForMember(d => d.Count, opt => opt.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(d => d.AveragePrice, opt => opt.MapFrom(s => s.CategoryProducts.Average(cp => cp.Product.Price)))
                .ForMember(d => d.TotalRevenue, opt => opt.MapFrom(s => s.CategoryProducts.Sum(cp => cp.Product.Price)));


            //this.CreateMap<User, ExportProductCountDto>()
            //    .ForMember(d => d.Count, opt => opt.MapFrom(s => s.ProductsSold.Count))
            //    .ForMember(d => d.Products, opt => opt.MapFrom(s => s.ProductsSold));

            //this.CreateMap<Product, ExportSoldProductsDto>();
        }
    }
}
