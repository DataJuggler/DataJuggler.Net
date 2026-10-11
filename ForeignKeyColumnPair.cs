#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace DataJuggler.Net
{

    #region class ForeignKeyColumnPair
    /// <summary>
    /// This class represents one column mapping inside a ForeignKeyConstraint —
    /// a single pair for a normal FK, one of several for a composite FK.
    /// </summary>
    public class ForeignKeyColumnPair
    {

        #region Private Variables
        private string fieldName;
        private int ordinal;
        private string referencedColumn;
        #endregion

        #region Constructors

            #region Default Constructor()
            /// <summary>
            /// Create a new instance of a ForeignKeyColumnPair
            /// </summary>
            public ForeignKeyColumnPair()
            {
            }
            #endregion

            #region Parameterized Constructor(string fieldName, string referencedColumn, int ordinal)
            /// <summary>
            /// Create a new instance of a ForeignKeyColumnPair and set the properties
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="referencedColumn"></param>
            /// <param name="ordinal"></param>
            public ForeignKeyColumnPair(string fieldName, string referencedColumn, int ordinal)
            {
                // Store the arguments
                FieldName = fieldName;
                ReferencedColumn = referencedColumn;
                Ordinal = ordinal;
            }
            #endregion

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
            
            #region ReferencedColumn
            /// <summary>
            /// This property gets or sets the value for 'ReferencedColumn'.
            /// </summary>
            public string ReferencedColumn
            {
                get { return referencedColumn; }
                set { referencedColumn = value; }
            }
            #endregion
            
        #endregion

    }
    #endregion

}