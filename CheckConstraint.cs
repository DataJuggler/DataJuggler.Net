

#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace DataJuggler.Net
{

    #region class CheckConstraint
    /// <summary>
    /// This class is used to represent a CheckConstraint in a SQL Server Database
    /// </summary>
    [Serializable]
    public class CheckConstraint
    {
        
        #region Private Variables
        private string tableName;
        private string columnName;
        private string checkClause;
        private string constraintName;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of a CheckConstraint object
        /// </summary>
        public CheckConstraint()
        {

        }
        #endregion

        #region Properties

            #region CheckClause
            /// <summary>
            /// This property gets or sets the value for 'CheckClause'.
            /// </summary>
            public string CheckClause
            {
                get { return checkClause; }
                set { checkClause = value; }
            }
            #endregion
            
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
            
            #region ConstraintName
            /// <summary>
            /// This property gets or sets the value for 'ConstraintName'.
            /// </summary>
            public string ConstraintName
            {
                get { return constraintName; }
                set { constraintName = value; }
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
