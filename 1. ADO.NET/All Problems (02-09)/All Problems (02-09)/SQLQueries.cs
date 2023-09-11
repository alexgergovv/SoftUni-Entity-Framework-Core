using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace All_Problems__02_09_
{
    internal static class SQLQueries
    {
        //Problem 02
        public const string GetVillainsNames = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                                  FROM Villains AS v
                                                  JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                                                  GROUP BY v.Id, v.Name
                                                  HAVING COUNT(mv.VillainId) > 3 
                                                  ORDER BY COUNT(mv.VillainId)";
        //Problem 03
        public const string GetVillainNameById = @"SELECT Name FROM Villains WHERE Id = @Id";
        public const string GetAllMinions = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                              m.Name, 
                                              m.Age
                                              FROM MinionsVillains AS mv
                                              JOIN Minions As m ON mv.MinionId = m.Id
                                              WHERE mv.VillainId = @Id
                                              ORDER BY m.Name";
        //Problem 04
        public const string GetTownById = @"SELECT Id FROM Towns WHERE Name = @townName";
        public const string AddNewTown = @"INSERT INTO Towns (Name) VALUES (@townName)";
        public const string GetVillainIdByName = @"SELECT Id FROM Villains WHERE Name = @Name";
        public const string AddNewVillain = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
        public const string AddNewMinion = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";
        public const string GetMinionByName = @"SELECT Id FROM Minions WHERE Name = @Name";
        public const string SetMinionToBeServentOfVillain = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";
        //Problem 05
        public const string GetTownsName = @"SELECT t.Name 
                                             FROM Towns as t
                                             JOIN Countries AS c ON c.Id = t.CountryCode
                                             WHERE c.Name = @countryName";
        public const string UpdateTownsName = @"UPDATE Towns
                                                SET Name = UPPER(Name)
                                                WHERE CountryCode = (SELECT c.Id FROM Countries AS c 
                                                WHERE c.Name = @countryName)";
        //Problem 06
        public const string GetVillainName = @"SELECT Name FROM Villains WHERE Id = @villainId";
        public const string ReleaseMinionsFromVillain = @"DELETE FROM MinionsVillains 
                                                          WHERE VillainId = @villainId";
        public const string DeleteVillain = @"DELETE FROM Villains
                                              WHERE Id = @villainId";
        //Problem 07
        public const string GetAllMinionsName = @"SELECT Name FROM Minions";
        //Problem 08
        public const string IncreaseMinionAge = @"UPDATE Minions
                                                  SET Name = LOWER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                  WHERE Id = @Id";
        public const string PrintAllMinionsNameAndAge = @"SELECT Name, Age FROM Minions";
        //Problem 09
        public const string ExecuteProcedure = @"EXEC usp_GetOlder @Id";
        public const string GetMinionNameAndAge = @"SELECT Name, Age FROM Minions WHERE Id = @Id";
    }
}