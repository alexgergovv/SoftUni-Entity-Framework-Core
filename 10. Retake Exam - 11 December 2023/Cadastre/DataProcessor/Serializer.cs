using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Cadastre.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
                 var properties = dbContext.Properties
                .AsNoTracking()
                .Where(p => p.DateOfAcquisition >= new DateTime(2000, 1, 1))
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new ExportPropertyDto()
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy"),
                    Owners = p.PropertiesCitizens
                    .Select(pc => pc.Citizen)
                    .OrderBy(c => c.LastName)
                    .Select(c => new ExportCitizenDto()
                    {
                        LastName = c.LastName,
                        MaritalStatus = c.MaritalStatus.ToString(),
                    })
                    .ToArray()
                })
                .ToList();

            return JsonConvert.SerializeObject(properties, Formatting.Indented);
        }
        private static XmlHelper xmlHelper;
        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            xmlHelper = new XmlHelper();
            ExportPropertyForAreaDto[] exportProperties = dbContext.Properties
                .AsNoTracking()
                .Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new ExportPropertyForAreaDto()
                {
                    PostalCode = p.District.PostalCode,
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy")
                })
                .ToArray();

            return xmlHelper.Serialize<ExportPropertyForAreaDto[]>(exportProperties, "Properties");
        }
    }
}
