using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            string result = GetUsersWithProducts(context);
            Console.WriteLine(result);
        }
        //Problem 01
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportUserDTO[] userDtos = JsonConvert.DeserializeObject<ImportUserDTO[]>(inputJson);
            ICollection<User> validUsers = new HashSet<User>();

            foreach (var userDto in userDtos)
            {
                User user = mapper.Map<User>(userDto);

                validUsers.Add(user);
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }
        //Problem 02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportProductDTO[] productDtos = JsonConvert.DeserializeObject<ImportProductDTO[]>(inputJson);
            Product[] products = mapper.Map<Product[]>(productDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }
        //Problem 03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportCategoryDTO[] categoriesDtos = JsonConvert.DeserializeObject<ImportCategoryDTO[]>(inputJson);
            ICollection<Category> categories = new HashSet<Category>();

            foreach (var categoryDto in categoriesDtos)
            {
                if (categoryDto.Name == null)
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryDto);
                categories.Add(category);
            }
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }
        //Problem 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportCategoryProductDTO[] categoryProductDtos = JsonConvert.DeserializeObject<ImportCategoryProductDTO[]>(inputJson);
            ICollection<CategoryProduct> categoryProducts = new HashSet<CategoryProduct>();

            foreach (ImportCategoryProductDTO cpDto in categoryProductDtos)
            {

                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(cpDto);
                categoryProducts.Add(categoryProduct);
            }

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count}";
        }
        //Problem 05
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
        //Problem 06
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    })
                    .ToArray()
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }
        //Problem 07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count,
                    averagePrice = $"{c.CategoriesProducts.Average(p => p.Product.Price):f2}",
                    totalRevenue = $"{c.CategoriesProducts.Sum(p => p.Product.Price):f2}",
                })
                .ToArray();

            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }
        //Problem 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(p => p.Buyer != null),
                        products = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                         .ToArray()
                    }

                })
                .OrderByDescending(u => u.soldProducts.count)
                .AsNoTracking()
                .ToArray();

            var usersOutput = new
            {
                usersCount = users.Count(),
                users = users
            };

            return JsonConvert.SerializeObject(usersOutput, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy(false, true) 
                    },
                    NullValueHandling = NullValueHandling.Ignore
                });
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