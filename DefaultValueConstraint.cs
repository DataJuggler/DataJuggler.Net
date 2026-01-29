

#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace DataJuggler.Net
{

    #region class DefaultValueConstraint
    /// <summary>
    /// This class is used to represent a default value constraint for a field.
    /// </summary>
    public class DefaultValueConstraint
    {
        
        #region Private Variables
        private string constraintName;
        private string tableName;
        private string columnName;
        private double defaultValue;
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
            
            #region DefaultValue
            /// <summary>
            /// This property gets or sets the value for 'DefaultValue'.
            /// </summary>
            public double DefaultValue
            {
                get { return defaultValue; }
                set { defaultValue = value; }
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
