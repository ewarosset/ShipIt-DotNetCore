using System.Configuration;

namespace ShipIt.Repositories
{
    public class ConnectionHelper
    {
        public static string GetConnectionString()
        {
            var dbname = ConfigurationManager.AppSettings["ShipIt"];

            if (dbname == null)
            {
                return System.Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
            };

            var username = ConfigurationManager.AppSettings["postgres"];
            var password = ConfigurationManager.AppSettings["bookish"];
            var hostname = ConfigurationManager.AppSettings["127.0.0.1"];
            var port = ConfigurationManager.AppSettings["5432"];

            return "Server=" + hostname + ";Port=" + port + ";Database=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }
    }
}
