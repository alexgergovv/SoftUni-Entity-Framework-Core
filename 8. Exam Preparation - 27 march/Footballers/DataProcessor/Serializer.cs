namespace Footballers.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.Json.Serialization;
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        private static XmlHelper xmlHelper;
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            xmlHelper = new XmlHelper();
            ExportCoachDto[] coaches = context.Coaches
                .Include(c => c.Footballers)
                .AsNoTracking()
                .ToArray()
                .Where(c => c.Footballers.Count > 0)
                .Select(c => new ExportCoachDto
                {
                    Name = c.Name,
                    FootballersCount = c.Footballers.Count,
                    Footballers = c.Footballers
                    .Select(f => new ExportFootballerForCoachDto
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .OrderBy(f => f.Name)
                    .ToArray()
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.Name)
                .ToArray();

            return xmlHelper.Serialize<ExportCoachDto[]>(coaches, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            ExportTeamDto[] teams = context.Teams
                .Include(t => t.TeamsFootballers)
                .ThenInclude(t => t.Footballer)
                .AsNoTracking()
                .ToArray()
                .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
                .Select(t => new ExportTeamDto
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                    .Select(tf => new ExportFootballerDto
                    {
                        Name = tf.Footballer.Name,
                        ContractStartDate = tf.Footballer.ContractStartDate.ToString("MM/dd/yyyy"),
                        ContractEndDate = tf.Footballer.ContractEndDate.ToString("MM/dd/yyyy"),
                        BestSkillType = tf.Footballer.BestSkillType.ToString(),
                        PositionType = tf.Footballer.PositionType.ToString()
                    })
                    .OrderByDescending(f => DateTime.ParseExact(f.ContractEndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                    .ThenBy(f => f.Name)
                    .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
