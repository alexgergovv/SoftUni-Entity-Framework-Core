namespace Trucks.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;
    using Trucks.DataProcessor.ExportDto;
    using Utilities.Utilities;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ExportDespatcherDto[] despatchers = context.Despatchers
                .Include(d => d.Trucks)
                .AsNoTracking()
                .Where(d => d.Trucks.Count > 0)
                .Select(d => new ExportDespatcherDto
                {
                   Name = d.Name,
                   TrucksCount = d.Trucks.Count,
                   Trucks = d.Trucks.Select(t => new ExportTrucksForDespatcherDto
                   {
                       RegistrationNumber = t.RegistrationNumber,
                       MakeType = t.MakeType.ToString()
                   })
                   .OrderBy(t => t.RegistrationNumber)
                   .ToArray()
                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.Name)
                .ToArray();

            return xmlHelper.Serialize<ExportDespatcherDto[]>(despatchers, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            ExportClientDto[] clients = context.Clients
                .Include(c => c.ClientsTrucks)
                .ThenInclude(ct => ct.Truck)
               .AsNoTracking()
               .ToArray()
               .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
               .Select(c => new ExportClientDto
               {
                   Name = c.Name,
                   Trucks = c.ClientsTrucks
                   .Where(ct => ct.Truck.TankCapacity >= capacity)
                   .Select(ct => new ExportTruckDto
                   {
                       RegistrationNumber = ct.Truck.RegistrationNumber,
                       VinNumber = ct.Truck.VinNumber,
                       TankCapacity = ct.Truck.TankCapacity,
                       CargoCapacity = ct.Truck.CargoCapacity,
                       CategoryType = ct.Truck.CategoryType.ToString(),
                       MakeType = ct.Truck.MakeType.ToString()
                   })
                   .OrderBy(t => t.MakeType)
                   .ThenByDescending(t => t.CargoCapacity)
                   .ToArray()
               })
               .OrderByDescending(c => c.Trucks.Count())
               .ThenBy(c => c.Name)
               .Take(10)
               .ToArray();

            return JsonConvert.SerializeObject(clients, Formatting.Indented);
        }
    }
}
