
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContext = new SoftUniContext();
            //Problem 03
            //Console.WriteLine(GetEmployeesFullInformation(dbContext));

            //Problem 04
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(dbContext));

            //Problem 05
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(dbContext));

            //Problem 06
            //Console.WriteLine(AddNewAddressToEmployee(dbContext));

            //Problem 07
            //Console.WriteLine(GetEmployeesInPeriod(dbContext));

            //Problem 08
            //Console.WriteLine(GetAddressesByTown(dbContext));

            //Problem 09
            //Console.WriteLine(GetEmployee147(dbContext));

            //Problem 10
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(dbContext));

            //Problem 11
            //Console.WriteLine(GetLatestProjects(dbContext));

            //Problem 12
            //Console.WriteLine(IncreaseSalaries(dbContext));

            //Problem 13
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(dbContext));

            //Problem 14
            //Console.WriteLine(DeleteProjectById(dbContext));

            //Problem 15
            Console.WriteLine(RemoveTown(dbContext));
        }
        //Problem 03
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .OrderBy(e=> e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .OrderBy(e => e.FirstName)
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            Address newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            Employee? employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            employee.Address = newAddress;

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => new
                {
                    AddressText = e.Address!.AddressText
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.AddressText}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                    .Where(ep => ep.Project.StartDate.Year >= 2001 &&
                           ep.Project.StartDate.Year <= 2003)
                    .Select(ep => new
                    {
                       ProjectName = ep.Project.Name,
                       StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                       EndDate = ep.Project.EndDate.HasValue
                       ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt")
                       : "not finished"
                    })
                    .ToArray()
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                })
                .ToArray();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 09
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employee = context.Employees.Find(147);

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            var projects = employee.EmployeesProjects.OrderBy(project => project.Project.Name);
            foreach (var p in projects)
            {
                sb.AppendLine(p.Project.Name);
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .ToArray()
                })
                .ToArray();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} - {d.ManagerFirstName} {d.ManagerLastName}");
                foreach (var e in d.Employees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .ToArray();

            var sortedProjects = projects
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt")
                })
                .ToArray();

            foreach (var p in sortedProjects)
            {
                sb.AppendLine($"{p.Name}");
                sb.AppendLine($"{p.Description}");
                sb.AppendLine($"{p.StartDate}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design"
                || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Salary = e.Salary * (decimal)1.12
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .Where(e => e.FirstName.Substring(0, 2) == "Sa")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            IQueryable<EmployeeProject> epToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);
            context.EmployeesProjects.RemoveRange(epToDelete);

            var project = context.Projects.Find(2);
            context.Projects.Remove(project);
            
            context.SaveChanges();

            var projects = context.Projects
                .Take(10)
                .Select(p => new
                {
                    p.Name
                })
                .ToArray();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 15
        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns
                .First(t => t.Name == "Seattle");

            IQueryable<Address> adddresses = context.Addresses
                .Where(a => a.TownId == town.TownId);

            int count = adddresses.Count();

            IQueryable<Employee> employees = context.Employees
                .Where(e => adddresses.Any(a => a.AddressId == e.AddressId));

            foreach (var e in employees)
            {
                e.AddressId = null;
            }

            foreach (var a in adddresses)
            {
                context.Addresses.Remove(a);
            }

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{count} addresses in Seattle were deleted";
        }
    }
}