

#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace DataJuggler.Net
{

    #region class SQLDatabaseTester
    /// <summary>
    /// This class is used to test SQL Databases
    /// </summary>
    public class SQLDatabaseTester
    {
        
        #region Methods

            #region TestDatabaseConnection(SQLDatabaseConnector sqlDatabaseConnector)
            /// <summary>
            /// This method is used to test a SQL database connection.
            /// </summary>
            public static bool TestDatabaseConnection(SQLDatabaseConnector sqlDatabaseConnector)
            {
                // initial value
                bool connectionAvailable = false;

                try
                {
                    // verify the sqlDatabaseConnector exists and the ConnectionString is set
                    if ((sqlDatabaseConnector != null) && (!String.IsNullOrEmpty(sqlDatabaseConnector.ConnectionString)))
                    {
                        // Open the connection
                        sqlDatabaseConnector.Open();

                        // Test the connection
                        connectionAvailable = sqlDatabaseConnector.Connected;

                        // Close the connection
                        sqlDatabaseConnector.Close();
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();
                }

                // return value
                return connectionAvailable;
            }
            #endregion

        #endregion
        
    }
    #endregion

}
