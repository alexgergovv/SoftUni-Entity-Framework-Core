using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            this.CreateMap<ImportUserDTO, User>();

            this.CreateMap<ImportProductDTO, Product>();

            this.CreateMap<ImportCategoryDTO, Category>();

            this.CreateMap<ImportCategoryProductDTO, CategoryProduct>();
        }
    }
}
