

#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace DataJuggler.Net
{

    #region class IdentityInfo
    /// <summary>
    /// This class contains the Primary Key Identity Column Name and Parent Table Name.
    /// </summary>
    public class IdentityInfo
    {
        
        #region Private Variables
        private string tableName;
        private string columnName;
        #endregion

        #region Constructor(string columnName, string tableName)
        /// <summary>
        /// This class is used to hold a list of Identity column names with the table name
        /// </summary>
        public IdentityInfo(string columnName, string tableName)
        {
            // store the arguments
            ColumnName = columnName;
            TableName = tableName;
        }
        #endregion

        #region Methods

        #endregion

        #region Properties

            #region ColumnName
            /// <summary>
            /// This property gets or sets the value for 'ColumnName'.
            /// </summary>
            public string ColumnName
            {
                get { return columnName; }
                set { columnName = value; }
            }
            #endregion
            
            #region TableName
            /// <summary>
            /// This property gets or sets the value for 'TableName'.
            /// </summary>
            public string TableName
            {
                get { return tableName; }
                set { tableName = value; }
            }
            #endregion
            
        #endregion
        
    }
    #endregion

}
