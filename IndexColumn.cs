

#region using statements

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace DataJuggler.Net
{

    #region class IndexColumn
    /// <summary>
    /// This class represents a column for an Index
    /// </summary>
    public class IndexColumn
    {
        
        #region Private Variables
        private string fieldName;
        private bool isDescendingKey;
        private bool isIncludedColumn;
        private int ordinal;
        #endregion
        
        #region Constructor
        /// <summary>
        /// Create a new instance of a 'IndexColumn' object.
        /// </summary>
        public IndexColumn()
        {
        }
        #endregion

        #region Parameterized Constructor(string fieldName, bool isDescendingKey, bool isIncludedColumn, int ordinal)
        /// <summary>
        /// Create a new instance of a 'IndexColumn' object.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="isDescendingKey"></param>
        /// <param name="isIncludedColumn"></param>
        /// <param name="ordinal"></param>
        public IndexColumn(string fieldName, bool isDescendingKey, bool isIncludedColumn, int ordinal)
        {
            // store the args
            FieldName = fieldName;
            IsDescendingKey = isDescendingKey;
            IsIncludedColumn = isIncludedColumn;
            Ordinal = ordinal;
        }
        #endregion
        
        #region Properties
            
            #region FieldName
            /// <summary>
            /// This property gets or sets the value for 'FieldName'.
            /// </summary>
            public string FieldName
            {
                get { return fieldName; }
                set { fieldName = value; }
            }
            #endregion
            
            #region IsDescendingKey
            /// <summary>
            /// This property gets or sets the value for 'IsDescendingKey'.
            /// </summary>
            public bool IsDescendingKey
            {
                get { return isDescendingKey; }
                set { isDescendingKey = value; }
            }
            #endregion
            
            #region IsIncludedColumn
            /// <summary>
            /// This property gets or sets the value for 'IsIncludedColumn'.
            /// </summary>
            public bool IsIncludedColumn
            {
                get { return isIncludedColumn; }
                set { isIncludedColumn = value; }
            }
            #endregion
            
            #region Ordinal
            /// <summary>
            /// This property gets or sets the value for 'Ordinal'.
            /// </summary>
            public int Ordinal
            {
                get { return ordinal; }
                set { ordinal = value; }
            }
            #endregion
            
        #endregion
        
    }
    #endregion

}
