

#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace DataJuggler.Net
{

    #region class ForeignKeyConstraint
    /// <summary>
    /// This class represents a Foreign Key for a DataTable.
    /// </summary>
    public class ForeignKeyConstraint
    {
        
        #region Private Variables
        private string foreignKey;
        private string name;
        private string referencedTable;
        private string referencedColumn;
        private string table;
        #endregion

        #region Constructors

            #region Default Constructor()
            /// <summary>
            /// Create a new instance of a ForeignKeyConstraint
            /// </summary>
            public ForeignKeyConstraint()
            {
            }
            #endregion

            #region Parameterized Constructor(string name, string table, string foreignKey, string referencedTable, string referencedColumn)
            /// <summary>
            /// Create a new instance of a ForeignKeyConstraint and set the properties
            /// </summary>
            /// <param name="name"></param>
            /// <param name="table"></param>
            /// <param name="foreignKey"></param>
            /// <param name="referencedTable"></param>
            /// <param name="referencedField"></param>
            public ForeignKeyConstraint(string name, string table, string foreignKey, string referencedTable, string referencedColumn)
            {
                // Store the arguments
                Name = name;
                Table = table;
                ForeignKey = foreignKey;
                ReferencedTable = referencedTable;
                ReferencedColumn = referencedColumn;
            }
            #endregion
        
        #endregion

        #region Properties
            
            #region ForeignKey
            /// <summary>
            /// This property gets or sets the value for 'ForeignKey'.
            /// </summary>
            public string ForeignKey
            {
                get { return foreignKey; }
                set { foreignKey = value; }
            }
            #endregion
            
            #region Name
            /// <summary>
            /// This property gets or sets the value for 'Name'.
            /// </summary>
            public string Name
            {
                get { return name; }
                set { name = value; }
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
            
            #region ReferencedTable
            /// <summary>
            /// This property gets or sets the value for 'ReferencedTable'.
            /// </summary>
            public string ReferencedTable
            {
                get { return referencedTable; }
                set { referencedTable = value; }
            }
            #endregion
            
            #region Table
            /// <summary>
            /// This property gets or sets the value for 'Table'.
            /// </summary>
            public string Table
            {
                get { return table; }
                set { table = value; }
            }
            #endregion
            
        #endregion
        
    }
    #endregion

}
