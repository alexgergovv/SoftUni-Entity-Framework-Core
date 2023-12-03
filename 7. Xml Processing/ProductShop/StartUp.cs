using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Reflection.Metadata;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            Console.WriteLine(GetUsersWithProducts(context));
        }
        //Problem 01
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();
            ImportUserDto[] userDtos = xmlHelper.Deserialize<ImportUserDto[]>(inputXml, "Users");

            ICollection<User> validUsers = new HashSet<User>();
            foreach (var userDto in userDtos)
            {
                if (!userDto.Age.HasValue)
                {
                    continue;
                }

                User user = mapper.Map<User>(userDto);
                validUsers.Add(user);
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }
        //Problem 02
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();
            ImportProductDto[] productDtos = xmlHelper.Deserialize<ImportProductDto[]>(inputXml, "Products");

            ICollection<Product> validProducts = new HashSet<Product>();
            foreach (ImportProductDto productDto in productDtos)
            {
                Product product = mapper.Map<Product>(productDto);
                validProducts.Add(product);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }
        //Problem 03
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper XmlHelper = new XmlHelper();
            ImportCategoryDto[] categoryDtos = XmlHelper.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

            ICollection<Category> validCategories = new HashSet<Category>();
            foreach (ImportCategoryDto categoryDto in categoryDtos)
            {
                if (string.IsNullOrEmpty(categoryDto.Name))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryDto);
                validCategories.Add(category);
            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }
        //Problem 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper XmlHelper = new XmlHelper();
            ImportCategoryProductDto[] categoryProductDtos = XmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            ICollection<CategoryProduct> validCategoryProducts = new HashSet<CategoryProduct>();
            foreach (ImportCategoryProductDto categoryProductDto in categoryProductDtos)
            {
                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(categoryProductDto);
                validCategoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        }
        //Problem 05
        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            XmlHelper XmlHelper = new XmlHelper();

            ExportProductDto[] products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ExportProductDto>(mapper.ConfigurationProvider)
                .Take(10)
                .ToArray();

            return XmlHelper.Serialize<ExportProductDto[]>(products, "Products");
        }
        //Problem 06
        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ExportUserDto[] users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ProjectTo<ExportUserDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportUserDto[]>(users, "Users");
        }
        //Problem 07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ExportCategoryDto[] categories = context.Categories
                .ProjectTo<ExportCategoryDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            return xmlHelper.Serialize<ExportCategoryDto[]>(categories, "Categories");
        }
        //Problem 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            XmlHelper XmlHelper = new XmlHelper();

            List<ExportUsersAndProductsDto> userDto = context.Users
                .Select(u => new ExportUsersAndProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportProductCountDto
                    {
                        Count = u.ProductsSold.Count(),
                        Products = u.ProductsSold.Select(p => new ExportProductInfoDto
                        {
                            Name = p.Name,
                            Price = p.Price,
                        })
                        .OrderByDescending(p => p.Price)
                        .ToList(),
                    }
                })
                .Where(u => u.SoldProducts.Count > 0)
                .OrderByDescending(u => u.SoldProducts.Count)
                .Take(10)
                .ToList();

            ExportUsersCountDto users = new ExportUsersCountDto
            {
                Users = userDto,
                Count = context.Users.Count(u => u.ProductsSold.Count > 0)
            };

            return XmlHelper.Serialize<ExportUsersCountDto>(users, "Users");
        }
        public static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }
    }
}