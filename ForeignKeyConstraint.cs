

#region using statements

using System.Collections.Generic;
using DataJuggler.Core.UltimateHelper;

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
        private List<ForeignKeyColumnPair> columns;
        private string fieldName;
        private string name;
        private string referencedTable;
        private string referencedColumn;
        private string table;
        private string onDelete;
        private string onUpdate;
        private bool isDisabled;
        #endregion

        #region Constructors

            #region Default Constructor()
            /// <summary>
            /// Create a new instance of a ForeignKeyConstraint
            /// </summary>
            public ForeignKeyConstraint()
            {
                // Create a new collection of 'ForeignKeyColumnPair' objects.
                Columns = new List<ForeignKeyColumnPair>();
            }
            #endregion

            #region Parameterized Constructor(string name, string table, string referencedTable)
            /// <summary>
            /// Create a new instance of a ForeignKeyConstraint and set the properties.
            /// Call AddColumn afterward for each column pair (once for a normal FK, 
            /// once per column for a composite FK).
            /// </summary>
            /// <param name="name"></param>
            /// <param name="table"></param>
            /// <param name="referencedTable"></param>
            public ForeignKeyConstraint(string name, string table, string referencedTable)
            {
                // create the collection
                this.columns = new List<ForeignKeyColumnPair>();

                // Store the arguments
                Name = name;
                Table = table;
                ReferencedTable = referencedTable;
            }
            #endregion

        #endregion

        #region Methods

            #region AddColumn(string fieldName, string referencedColumn, int ordinal)
            /// <summary>
            /// This method adds one column pair to this constraint. Called once for
            /// a normal FK, once per column for a composite FK. The first call (ordinal 1)
            /// also fills the legacy FieldName / ReferencedColumn fields.
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="referencedColumn"></param>
            /// <param name="ordinal"></param>
            public void AddColumn(string fieldName, string referencedColumn, int ordinal)
            {
                // if the Columns collection does not exist yet
                if (NullHelper.IsNull(Columns))
                {
                    // create the collection
                    Columns = new List<ForeignKeyColumnPair>();
                }

                // add this column pair
                Columns.Add(new ForeignKeyColumnPair(fieldName, referencedColumn, ordinal));

                // if this is the first column, keep the legacy fields in sync for old readers
                if (ordinal == 1)
                {
                    FieldName = fieldName;
                    ReferencedColumn = referencedColumn;
                }
            }
            #endregion

        #endregion

        #region Properties
            
            #region Columns
            /// <summary>
            /// This property gets or sets the value for 'Columns'.
            /// </summary>
            public List<ForeignKeyColumnPair> Columns
            {
                get { return columns; }
                set { columns = value; }
            }
            #endregion
            
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
            
            #region IsDisabled
            /// <summary>
            /// This property gets or sets the value for 'IsDisabled'.
            /// </summary>
            public bool IsDisabled
            {
                get { return isDisabled; }
                set { isDisabled = value; }
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
            
            #region OnDelete
            /// <summary>
            /// This property gets or sets the value for 'OnDelete'.
            /// </summary>
            public string OnDelete
            {
                get { return onDelete; }
                set { onDelete = value; }
            }
            #endregion
            
            #region OnUpdate
            /// <summary>
            /// This property gets or sets the value for 'OnUpdate'.
            /// </summary>
            public string OnUpdate
            {
                get { return onUpdate; }
                set { onUpdate = value; }
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

