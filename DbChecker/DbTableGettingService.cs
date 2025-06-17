using System.Data;
using Microsoft.Data.SqlClient;

namespace DbChecker;

public static class DbTableGettingService
{
    private const string CountSelectSql = "SELECT COUNT(*) FROM \"{0}\"";

    public static List<Tuple<string, int>> GetTableStat(string connectionString)
    {
        using var sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();

        if (sqlConnection.State != ConnectionState.Open) return new List<Tuple<string, int>>();

        var tables = sqlConnection
            .GetSchema("Tables")
            .AsEnumerable()
            .Select(s => s[2].ToString() ?? "")
            .Select(x =>
            {
                using var countCommand =
                    new SqlCommand(CountSelectSql.Replace("{0}", x), sqlConnection);
                var count = (int)countCommand.ExecuteScalar();

                return new Tuple<string, int>(x, count);
            })
            .ToList();

        return tables;

    }
}