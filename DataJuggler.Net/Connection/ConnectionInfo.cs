

#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace DataJuggler.Net.Connection
{

    #region class ConfigurationInfo
    /// <summary>
    /// This class represents an application configuration
    /// </summary>
    public class ConnectionInfo
    {
        
        #region Private Variables
        private string connectionString;
        private string databaseName;
        private string databasePassword;
        private string databaseServer;
        private string databaseUserName;
        private bool integratedSecurity;
        #endregion

        #region Consructor
        /// <summary>
        /// Create a new instance of a ConnectionInfo.
        /// </summary>
        public ConnectionInfo()
        {

        } 
        #endregion

        #region Methods

            #region Validate()
            /// <summary>
            /// This method returns true if the values for DatabaseServer,
            /// DatabaseUserName & Databasepassword are present.
            /// </summary>
            /// <returns></returns>
            public bool Validate()
            {
                // initial value
                bool valid = false;

                // if the ConnectionString exists
                if (this.HasConnectionString)
                {
                    // we are considered valid with a Connectionstring
                    valid = HasConnectionString;
                }
                else
                {
                    // validate each proerty
                    bool hasServerName = (!String.IsNullOrEmpty(this.DatabaseServer));
                    bool hasUserName = (!String.IsNullOrEmpty(this.DatabaseUserName));
                    bool hasDatabaseName = (!String.IsNullOrEmpty(this.DatabaseName));
                    bool hasPassword = (!String.IsNullOrEmpty(this.DatabasePassword));

                    // if we should use IntegratedSecurity (Windows Authentication)
                    if (this.IntegratedSecurity)
                    {
                        // all you need is a Server & DatabaseName
                        valid = ((hasServerName) && (hasDatabaseName));
                    }
                    else
                    {
                        // this is valid if all of these are true
                        valid = ((hasServerName) && (hasUserName) && (hasDatabaseName) && (hasPassword));
                    }
                }
            
                // return value
                return valid;
            }
            #endregion

        #endregion

        #region Properties

            #region ConnectionString
            /// <summary>
            /// This property gets or sets the value for 'ConnectionString'.
            /// </summary>
            public string ConnectionString
            {
                get { return connectionString; }
                set { connectionString = value; }
            }
            #endregion

            #region DatabaseName
            /// <summary>
            /// The name of the database to connect to.
            /// </summary>
            public string DatabaseName
            {
                get { return databaseName; }
                set { databaseName = value; }
            }
            #endregion

            #region DatabasePassword
            /// <summary>
            /// The password for the database.
            /// </summary>
            public string DatabasePassword
            {
                get { return databasePassword; }
                set { databasePassword = value; }
            }
            #endregion

            #region DatabaseServer
            /// <summary>
            /// The server that this database resides on.
            /// </summary>
            public string DatabaseServer
            {
                get { return databaseServer; }
                set { databaseServer = value; }
            }
            #endregion

            #region DatabaseUserName
            /// <summary>
            /// The user name for this SQL login.
            /// </summary>
            public string DatabaseUserName
            {
                get { return databaseUserName; }
                set { databaseUserName = value; }
            }
            #endregion

            #region HasConnectionString
            /// <summary>
            /// This read only property returns true if this object has a ConnectionString set.
            /// </summary>
            public bool HasConnectionString
            {
                get
                {
                    // initial value
                    bool hasConnectionString = (!String.IsNullOrEmpty(this.ConnectionString));

                    // return value
                    return hasConnectionString;
                }
            } 
            #endregion

            #region IntegratedSecurity
            /// <summary>
            /// This property gets or sets the value for 'IntegratedSecurity'.
            /// </summary>
            public bool IntegratedSecurity
            {
                get { return integratedSecurity; }
                set { integratedSecurity = value; }
            }
            #endregion

            #region IsValid
            /// <summary>
            /// This property calls the 'Validation()' 
            /// method to test if all required fields 
            /// are filled in.
            /// </summary>
            public bool IsValid
            {
                get
                {
                    // initial value
                    bool isValid = this.Validate();

                    // return value
                    return isValid;
                }
            }
            #endregion
            
        #endregion

    } 
    #endregion

}
