

#region using statements

using System.Text;
using Microsoft.Data.SqlClient;

#endregion

namespace DataJuggler.Net
{

    #region class ConnectionStringHelper
    /// <summary>
    /// This class [enter description here].
    /// </summary>
    public class ConnectionStringHelper
    {

        #region BuildConnectionString(string serverName, string databaseName)
        /// <summary>
        /// This method builds a connection string when Windows Authentication is used.
        /// </summary>
        /// <param name="ServerName">The SQL Server Name.</param>
        /// <param name="DatabaseName">The database name.</param>
        /// <param name="encrypt">Starting in 4.0, Microsoft.Data.SqlClient turns on Encryption by default.
        /// If your database is encrypted, pass in true here.</param>
        /// <returns></returns>
        public static string BuildConnectionString(string serverName, string databaseName, bool encrypt = false)
        {
            // Create an instance of the SqlConnectionStringBuilder
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.DataSource = serverName;
            connectionStringBuilder.InitialCatalog = databaseName;
            connectionStringBuilder.IntegratedSecurity = true;
            connectionStringBuilder.Encrypt = encrypt;
            
            // Return Built Connection String
            return connectionStringBuilder.ConnectionString;
        }
        #endregion

        #region BuildConnectionString(string ServerName, string DatabaseName, string userId, string Password)
        /// This method builds a connection string when SQLServer Authentication is used.
        /// A UserId and Password are required for this method.
        /// </summary>
        /// <param name="ServerName">The SQL Server Name.</param>
        /// <param name="DatabaseName">The database name.</param>
        /// <param name="userId">Sql Server user name for this connection.</param>
        /// <param name="password">Sql Server server password for this connection.</param>
        /// <param name="encrypt">Starting in 4.0, Microsoft.Data.SqlClient turns on Encryption by default.
        /// If your database is encrypted, pass in true here.</param>
        /// <returns></returns>
        public static string BuildConnectionString(string serverName, string databaseName, string userId, string password, bool encrypt = false)
        {
            // Create an instance of the SqlConnectionStringBuilder
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            // set the properties
            connectionStringBuilder.DataSource = serverName;
            connectionStringBuilder.InitialCatalog = databaseName;
            connectionStringBuilder.IntegratedSecurity = false;
            connectionStringBuilder.UserID = userId;
            connectionStringBuilder.Password = password;
            connectionStringBuilder.Encrypt = encrypt;

            // Return Built Connection String
            return connectionStringBuilder.ConnectionString;
        }
        #endregion
        
    }
    #endregion

}

