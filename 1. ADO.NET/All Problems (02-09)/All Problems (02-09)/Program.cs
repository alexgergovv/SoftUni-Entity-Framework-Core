using Microsoft.Data.SqlClient;
using System.Text;

namespace All_Problems__02_09_
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await using SqlConnection connection = new SqlConnection(Config.ConnectionString);
            await connection.OpenAsync();
            //Problem 02
            Console.WriteLine(await GetVillainsNameAsync(connection));
            //Problem 03
            Console.WriteLine(await GetMinionsNameAsync(connection, 8));
            //Problem 04
            Console.WriteLine(await AddMinionAsync(connection, "Carry 20 Eindhoven", "Jimmy"));
            //Problem 05
            Console.WriteLine(await ChangeTownsNameToUpperCaseAsync(connection, "Bulgaria"));
            //Problem 06
            Console.WriteLine(await DeleteVillainAndReleaseHisMinionsAsync(connection, 1));
            //Problem 07
            Console.WriteLine(await getAllMinionsNameAsync(connection));
            //Problem 08
            Console.WriteLine(await IncreaseMinionAgeAsync(connection, "1 5 4"));
            //Problem 09
            Console.WriteLine(await IncreaseMinionAgeProcedureAsync(connection, 3));
        }
        //Problem 02
        static async Task<string> GetVillainsNameAsync(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(SQLQueries.GetVillainsNames, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                string name = (string)reader["Name"];
                int count = (int)reader["MinionsCount"];
                sb.AppendLine($"{name} - {count}");
            }

            await reader.CloseAsync();
            return sb.ToString().TrimEnd();
        }
        //Problem 03
        static async Task<string> GetMinionsNameAsync(SqlConnection connection, int villainId)
        {
            SqlCommand getVillainNameCommand = new SqlCommand(SQLQueries.GetVillainNameById, connection);
            getVillainNameCommand.Parameters.AddWithValue("@Id", villainId);

            object? villainNameObj = await getVillainNameCommand.ExecuteScalarAsync();
            if (villainNameObj == null)
            {
                return $"No villain with ID {villainId} exists in the database.";
            }
            StringBuilder sb = new StringBuilder();
            string villainName = (string)villainNameObj;
            sb.AppendLine($"Villain: {villainName}");

            SqlCommand getAllMinions = new SqlCommand(SQLQueries.GetAllMinions, connection);
            getAllMinions.Parameters.AddWithValue("@Id", villainId);
            SqlDataReader reader = await getAllMinions.ExecuteReaderAsync();
            while (reader.Read())
            {
                long rowNumber = (long)reader["RowNum"];
                string minionName = (string)reader["Name"];
                int age = (int)reader["Age"];
                sb.AppendLine($"{rowNumber}. {minionName} {age}");
            }
            if (!reader.HasRows)
            {
                sb.AppendLine($"No villain with ID {villainId} exists in the database.");
            }
            await reader.CloseAsync();
            return sb.ToString().TrimEnd();
        }
        //Problem 04
        private static async Task<string> AddMinionAsync(SqlConnection connection, string minionInfo, string villainName)
        {
            StringBuilder sb = new StringBuilder();
            string[] minionArgs = minionInfo
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            string minionName = minionArgs[0];
            int minionAge = int.Parse(minionArgs[1]);
            string townName = minionArgs[2];

            SqlTransaction sqlTransaction = connection.BeginTransaction();
            try
            {
                int townId = await GetTownIdOrAddByNameAsync(connection, sb, townName, sqlTransaction);
                int villainId = await GetVillainIdOrAddByNameAsync(connection, sb, villainName, sqlTransaction);
                int minionId = await AddNewMinionAndReturnIdAsync(connection, minionName, minionAge, townId, sqlTransaction);

                await SetMinionToBeServantOfVillainAsync(connection, minionId, villainId, sqlTransaction);
                sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

                await sqlTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();
            }

            return sb.ToString().TrimEnd();
        }

        private static async Task<int> GetTownIdOrAddByNameAsync(SqlConnection connection, StringBuilder sb, string townName, SqlTransaction sqlTransaction)
        {
            SqlCommand getTownIdCmd = new SqlCommand(SQLQueries.GetTownById, connection, sqlTransaction);
            getTownIdCmd.Parameters.AddWithValue("@townName", townName);
            object? townIdObj = await getTownIdCmd.ExecuteScalarAsync();
            if (townIdObj == null)
            {
                SqlCommand addNewTownCmd = new SqlCommand(SQLQueries.AddNewTown, connection, sqlTransaction);
                addNewTownCmd.Parameters.AddWithValue("@townName", townName);

                await addNewTownCmd.ExecuteNonQueryAsync();
                townIdObj = await getTownIdCmd.ExecuteScalarAsync();
                sb.AppendLine($"Town {townName} was added to the database.");
            }

            return (int)townIdObj;
        }
        private static async Task<int> GetVillainIdOrAddByNameAsync(SqlConnection connection, StringBuilder sb, string villainName, SqlTransaction sqlTransaction)
        {
            SqlCommand getVillainIdCmd = new SqlCommand(SQLQueries.GetVillainIdByName, connection, sqlTransaction);
            getVillainIdCmd.Parameters.AddWithValue("@Name", villainName);
            int? villainId = (int?)await getVillainIdCmd.ExecuteScalarAsync();
            if (!villainId.HasValue)
            {
                SqlCommand addNewVillainCmd = new SqlCommand(SQLQueries.AddNewVillain, connection, sqlTransaction);
                addNewVillainCmd.Parameters.AddWithValue("@villainName", villainName);
                await addNewVillainCmd.ExecuteNonQueryAsync();
                villainId = (int?)await getVillainIdCmd.ExecuteScalarAsync();
                sb.AppendLine($"Villain {villainName} was added to the database.");
            }

            return villainId.Value;
        }
        private static async Task<int> AddNewMinionAndReturnIdAsync(SqlConnection connection, string minionName, int minionAge, int townId, SqlTransaction sqlTransaction)
        {
            SqlCommand addMinionCmd = new SqlCommand(SQLQueries.AddNewMinion, connection, sqlTransaction);
            addMinionCmd.Parameters.AddWithValue(@"Name", minionName);
            addMinionCmd.Parameters.AddWithValue(@"Age", minionAge);
            addMinionCmd.Parameters.AddWithValue(@"townId", townId);

            await addMinionCmd.ExecuteNonQueryAsync();

            SqlCommand getMinionIdCmd = new SqlCommand(SQLQueries.GetMinionByName, connection, sqlTransaction);
            getMinionIdCmd.Parameters.AddWithValue(@"Name", minionName);

            int minionId = (int)await getMinionIdCmd.ExecuteScalarAsync();
            return minionId;
        }
        private static async Task SetMinionToBeServantOfVillainAsync(SqlConnection connection, int minionId, int villainId, SqlTransaction sqlTransaction)
        {
            SqlCommand addMinionVillainCmd = new SqlCommand(SQLQueries.SetMinionToBeServentOfVillain, connection, sqlTransaction);
            addMinionVillainCmd.Parameters.AddWithValue(@"minionId", minionId);
            addMinionVillainCmd.Parameters.AddWithValue(@"villainId", villainId);

            await addMinionVillainCmd.ExecuteNonQueryAsync();
        }
        //Problem 05
        private static async Task<string> ChangeTownsNameToUpperCaseAsync(SqlConnection connection, string countryName)
        {
            SqlCommand getTownsNameCmd = new SqlCommand(SQLQueries.GetTownsName, connection);
            StringBuilder sb = new StringBuilder();
            getTownsNameCmd.Parameters.AddWithValue("@countryName", countryName);
            SqlDataReader reader = await getTownsNameCmd.ExecuteReaderAsync();
            if (!reader.HasRows)
            {
                return $"No town names were affected.";

            }
            await reader.CloseAsync();

            SqlCommand updateTownsNameCmd = new SqlCommand(SQLQueries.UpdateTownsName, connection);
            updateTownsNameCmd.Parameters.AddWithValue("@countryName", countryName);
            int affectedRows = await updateTownsNameCmd.ExecuteNonQueryAsync();
            sb.AppendLine($"{affectedRows} town names were affected.");

            SqlCommand getTownsNameUpperCaseCmd = new SqlCommand(SQLQueries.GetTownsName, connection);
            getTownsNameUpperCaseCmd.Parameters.AddWithValue("@countryName", countryName);
            SqlDataReader sqlReader = await getTownsNameUpperCaseCmd.ExecuteReaderAsync();
            string townsNames = "";
            while (sqlReader.Read())
            {
                townsNames += (string)sqlReader["Name"] + " ";
            }
            await sqlReader.CloseAsync();
            string[] townsNameArray = townsNames.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
            sb.AppendLine($"[{string.Join(", ", townsNameArray)}]");
            return sb.ToString().TrimEnd();
        }
        //Problem 06
        private static async Task<string> DeleteVillainAndReleaseHisMinionsAsync(SqlConnection connection, int villainId)
        {
            StringBuilder sb = new StringBuilder();
            SqlTransaction sqlTransaction = connection.BeginTransaction();
            try
            {
                SqlCommand getVillainByIdCmd = new SqlCommand(SQLQueries.GetVillainName, connection, sqlTransaction);
                getVillainByIdCmd.Parameters.AddWithValue("@villainId", villainId);
                string? villainName = (string?)await getVillainByIdCmd.ExecuteScalarAsync();
                if (villainName == null)
                {
                    return "No such villain was found.";
                }

                SqlCommand releaseMinionsCmd = new SqlCommand(SQLQueries.ReleaseMinionsFromVillain, connection, sqlTransaction);
                releaseMinionsCmd.Parameters.AddWithValue("@villainId", villainId);
                int deletedMinionsCount = await releaseMinionsCmd.ExecuteNonQueryAsync();

                SqlCommand deleteVillainCmd = new SqlCommand(SQLQueries.DeleteVillain, connection, sqlTransaction);
                deleteVillainCmd.Parameters.AddWithValue("@villainId", villainId);
                await deleteVillainCmd.ExecuteNonQueryAsync();

                sb.AppendLine($"{villainName} was deleted.");
                sb.AppendLine($"{deletedMinionsCount} minions were released.");

                await sqlTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await sqlTransaction.RollbackAsync();
            }
            return sb.ToString().TrimEnd();
        }
        private static async Task<string> getAllMinionsNameAsync(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand getAllMinionsNameCmd = new SqlCommand(SQLQueries.GetAllMinionsName, connection);
            SqlDataReader reader = await getAllMinionsNameCmd.ExecuteReaderAsync();
            string names = "";
            while (reader.Read())
            {
                names += (string)reader["Name"] + " ";
            }
            await reader.CloseAsync();
            string[] minionNamesArray = names
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            for (int i = 0; i < minionNamesArray.Length / 2; i++)
            {
                sb.AppendLine(minionNamesArray[i]);
                sb.AppendLine(minionNamesArray[minionNamesArray.Length - i - 1]);
                if ((minionNamesArray.Length % 2 == 1) && (i == minionNamesArray.Length / 2))
                {
                    sb.AppendLine(minionNamesArray[i + 1]);
                }
            }

            return sb.ToString().TrimEnd();
        }
        //Problem 08
        private static async Task<string> IncreaseMinionAgeAsync(SqlConnection connection, string minionIds)
        {
            StringBuilder sb = new StringBuilder();
            int[] minionIdArray = minionIds
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            for (int i = 0; i < minionIdArray.Length; i++)
            {
                SqlCommand updateMinionAgeCmd = new SqlCommand(SQLQueries.IncreaseMinionAge, connection);
                updateMinionAgeCmd.Parameters.AddWithValue("@Id", minionIdArray[i]);
                await updateMinionAgeCmd.ExecuteNonQueryAsync();
            }

            SqlCommand printMinionsNameAndAgeCmd = new SqlCommand(SQLQueries.PrintAllMinionsNameAndAge, connection);
            SqlDataReader reader = await printMinionsNameAndAgeCmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                sb.AppendLine((string)reader["Name"] + " " + (int)reader["Age"]);
            }
            await reader.CloseAsync();
            return sb.ToString().TrimEnd();
        }
        //Problem 09
        private static async Task<string> IncreaseMinionAgeProcedureAsync(SqlConnection connection, int minionId)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand increaseMinionAgeProcedureCmd = new SqlCommand(SQLQueries.ExecuteProcedure, connection);
            increaseMinionAgeProcedureCmd.Parameters.AddWithValue("@Id", minionId);
            await increaseMinionAgeProcedureCmd.ExecuteNonQueryAsync();

            SqlCommand getMinionNameAndAgeCmd = new SqlCommand(SQLQueries.GetMinionNameAndAge, connection);
            getMinionNameAndAgeCmd.Parameters.AddWithValue("@Id", minionId);
            SqlDataReader reader = await getMinionNameAndAgeCmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                sb.AppendLine($"{(string)reader["Name"]} - {(int)reader["Age"]} years old");
            }

            await reader.CloseAsync();
            return sb.ToString().TrimEnd();
        }
    }
}