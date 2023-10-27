namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //Problem 02
            //string command = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, command));

            //Problem 03
            //Console.WriteLine(GetGoldenBooks(db));

            //Problem 04
            //Console.WriteLine(GetBooksByPrice(db));

            //Problem 05
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(db, year));

            //Problem 06
            //string categories = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db, categories));

            //Problem 07
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, date));

            //Problem 08
            //string ending = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db, ending));

            //Problem 09
            //string containing = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(db, containing));

            //Problem 10
            //string starting = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(db, starting));

            //Problem 11
            //int lengthCheck = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(db, lengthCheck));

            //Problem 12
            //Console.WriteLine(CountCopiesByAuthor(db));

            //Problem 13
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //Problem 14
            //Console.WriteLine(GetMostRecentBooks(db));

            //Problem 15
            //IncreasePrices(db);

            //Problem 16
            //Console.WriteLine(RemoveBooks(db));
        }
        //Problem 02
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var booksByAgeRestriction = context.Books
                .ToArray()
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, booksByAgeRestriction);
        }
        //Problem 03
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, goldenBooks);
        }
        //Problem 04
        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksByPrice = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Price,
                    b.Title
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in booksByPrice)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 05
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksNotReleasedInYear = context.Books
                .OrderBy(b => b.BookId)
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, booksNotReleasedInYear);
        }
        //Probelm 06
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var listOfCategories = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToList();

            var bookByCategories = context.Books
                .Where(b => b.BookCategories.Any(b => listOfCategories.Contains(b.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookByCategories);
        }
        //Problem 07
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var booksReleasedBeforeDate = context.Books
                .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in booksReleasedBeforeDate)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 08
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(a => a)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }
        //Problem 09
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitles = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }
        //Problem 10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksByAuthor = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorFirstName = b.Author.FirstName,
                    AuthorLastName = b.Author.LastName
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in booksByAuthor)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorFirstName} {book.AuthorLastName})");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 11
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray();

            return books.Count();
        }
        //Problem 12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var countsByAuthor = context.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    TotalCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalCopies)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var author in countsByAuthor)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName} - {author.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 13
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var profitByCategory = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks
                    .Select(cb => new
                    {
                        Profit = cb.Book.Price * cb.Book.Copies
                    })
                    .Sum(c => c.Profit)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var c in profitByCategory)
            {
                sb.AppendLine($"{c.Name} ${c.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var recentBooks = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate)
                    .Take(3)
                    .Select(cb => new
                    {
                        Title = cb.Book.Title,
                        ReleaseDate = cb.Book.ReleaseDate.Value.Year
                    })
                })
                .OrderBy(c => c.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var category in recentBooks)
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate})");
                }
            }

            return sb.ToString().Trim();
        }
        //Problem 15
        public static void IncreasePrices(BookShopContext context)
        {
            var bookReleasedBefore2010 = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var book in bookReleasedBefore2010)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }
        //Problem 16
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToDelete = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            context.RemoveRange(booksToDelete);
            context.SaveChanges();
            return booksToDelete.Count();
        }
    }
}


