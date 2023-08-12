

#region using statements

using System;
using System.Collections.Generic;

#endregion

namespace DataJuggler.Net
{
    
    #region class DataTable 
	[Serializable]
	public class DataTable 
	{

		#region Private Variables
        private int tableId;
		private string className;
		private string classfilename;
		private string connectionstring;
		private bool createcollectionclass;
		private bool exclude;
		private List<DataField> fields;
		private string name;
		private Database parentdatabase;
		private List<DataRow> rows;
		private DataManager.Scope scope;
		private bool serializable;
		private SQLGenerator sqlgenerator;
		private string xmlfilename;
        private bool isView;
        private string objectNameSpaceName;
        private List<DataIndex> indexes;
        private List<CheckConstraint> checkConstraints;
        private List<ForeignKeyConstraint> foreignKeys;        
        private string schemaName;
        private string tag;
		#endregion

 		#region Constructor +1 override

			#region DefaultConstructor()
			public DataTable()
			{
				// Create Fields Collection
				this.Fields = new List<DataField>();

				// Create New DataRows Collection
				this.Rows = new List<DataRow>();
				
				// Create sql Generator
				this.sqlgenerator = new SQLGenerator();
			}
			#endregion

			#region DataTable(Database parentDatabase)
			public DataTable(Database parentDatabase)
			{
				// Set parentDatabase
				this.ParentDatabase = parentDatabase;

				// Create New DataFields Collection
				this.Fields = new List<DataField>();

				// Create New DataRows Collection
				this.Rows = new List<DataRow>();
				
				// Create SQLGenerator
				this.sqlgenerator = new	SQLGenerator();
				
			}
			#endregion

		#endregion

		#region Methods

            #region CapitalizeFirstChar(string word)
            public static string CapitalizeFirstChar(string word)
			{
				// If Null String
				if (String.IsNullOrEmpty(word))
				{
					// Return Null String
					return word;
				}

				// If Empty String
				if(word.Length == 1)
				{
					// Return Word In Upper Case
					return word.ToUpper();
				}

				// Create Char Array
				Char[] letters = word.ToCharArray();

				// Capitalize First Character
				letters[0] = Char.ToUpper(letters[0]);
				return new string(letters);

			}
			#endregion

            #region CheckIfNameHasPrefix(string tableName, ref firstCapitalLetterIndex)
            /// <summary>
            /// This method checks if a table name contains a prefix
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            internal static bool CheckIfNameHasPrefix(string tableName, ref int firstCapitalLetterIndex)
            {
                // initial value
                bool hasPrefix = false;
                
                // get the lowercase count
                int lowerCaseCount = 0;
                
                // local
                int tableNameLen = 0;
                
                // if the string exists
                if (!String.IsNullOrEmpty(tableName))
                {
                    // set the len
                    tableNameLen = tableName.Length;
                    
                    for (int x = 0; x < tableName.Length;x++)
                    {
                        // set the character at the index
                        char c = tableName[x];
                        
                        // if this is a lower case character
                        if (char.IsLower(c))
                        {
                            // increment lower case count
                            lowerCaseCount++;
                        }
                        else
                        {
                            // break out of loop
                            firstCapitalLetterIndex = x;
                            
                            // break out of loop
                            break;
                        }
                    }
                }
                
                // set to true
                if ((lowerCaseCount >= 1) && (lowerCaseCount <= 5) && (lowerCaseCount < (tableNameLen - 2)))
                {
                    // this has a prefix
                    hasPrefix = true;
                }
                
                // return value
                return hasPrefix;
            } 
            #endregion
            
            #region CompareTo(object tableObject)
            /// <summary>
            /// Compare this table to the table passed in
            /// by tableName.
            /// </summary>
            /// <param name="dataTable"></param>
            /// <returns></returns>
            public int CompareTo(object tableObject)
            {
                // initial value
                int returnValue = 0;
            
                // verify the dataTable exists
                DataTable dataTable = tableObject as DataTable;
                
                // verify the cast was successful
                if(dataTable != null)
                {
                    // Set the return value
                    returnValue = this.Name.CompareTo(dataTable.Name);
                }
                
                // return value
                return returnValue;
            } 
            #endregion

            #region ContainsBinaryData
            /// <summary>
            /// Does this table contain Binary
            /// data.
            /// </summary>
            public bool ContainsBinaryData
            {
                get
                {
                    // initial value
                    bool containsBinaryData = false;
                    
                    // iterate fields collection
                    foreach(DataField field in this.ActiveFields)
                    {
                        // This is a binary field
                        if(field.DataType == DataManager.DataTypeEnum.Binary)
                        {
                            // this table does contain binary data
                            containsBinaryData = true;
                            
                            // break out of loop 
                            break;
                        }
                    }
                    
                    // return value
                    return containsBinaryData;
                }
            } 
            #endregion

            #region FindIndexByName(string name)
            /// <summary>
            /// This method returns the DataIndex for the name given, if it exists in this.Indexes.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public DataIndex FindIndexByName(string name)
            {
                // initial value
                DataIndex index = null;

                // if the indexes collection exists
                if (this.HasIndexes)
                {
                    // Iterate the items in the collection
                    foreach (DataIndex tempIndex in this.Indexes)
                    {
                        // if the names match
                        if (tempIndex.Name == name)
                        {
                            // set the return value
                            index = tempIndex;

                            // break out of the loop
                            break;
                        }
                    }                    
                }

                // return value
                return index;
            }
            #endregion

            #region FindCheckConstraintByName(string constraintName)
            /// <summary>
            /// This method attempts to find a CheckConstraint by the Constraint Name
            /// </summary>
            /// <param name="constraintName"></param>
            /// <returns></returns>
            public CheckConstraint FindCheckConstraintByName(string constraintName)
            {
                // initial value
                CheckConstraint checkConstraint = null;

                // if the CheckConstraints exist
                if (this.HasCheckConstraints)
                {
                    // iterate the tempCheckConstraint                                                                  
                    foreach (CheckConstraint tempCheckConstraint in this.CheckConstraints)
                    {
                        // If this is the Constraint being sought
                        if (tempCheckConstraint.ConstraintName == constraintName)
                        {
                            // set the return value
                            checkConstraint = tempCheckConstraint;
                        }
                    }
                }

                // return value
                return checkConstraint;
            }
            #endregion

            #region FindCheckConstraintByNameAndNumber(string constraintName, int number)
            /// <summary>
            /// This method attempts to find a CheckConstraint by the Constraint Name
            /// </summary>
            /// <param name="constraintName"></param>
            /// <returns></returns>
            public CheckConstraint FindCheckConstraintByNameAndNumber(string constraintName, int number)
            {
                // initial value
                CheckConstraint checkConstraint = null;

                // local
                int tempNumber = 0;

                // if the CheckConstraints exist
                if (this.HasCheckConstraints)
                {
                    // iterate the tempCheckConstraint                                                                  
                    foreach (CheckConstraint tempCheckConstraint in this.CheckConstraints)
                    {
                        // If this is the Constraint being sought
                        if (tempCheckConstraint.ConstraintName == constraintName)
                        {
                             // Increment the value for tempNumber
                             tempNumber++;

                             // if this is the number being sought
                             if (tempNumber == number)
                             {
                                // set the return value
                                 checkConstraint = tempCheckConstraint;

                                // break out of the loop
                                break;
                             }
                        }
                    }
                }

                // return value
                return checkConstraint;
            }
            #endregion
			
			#region HasMultiplePrimaryKeys()
			/// <summary>
			/// This is used because I started to support composite primary keys
            /// but I haven't got around to it yet.
            /// For now it works best when a table has a single primary key
            /// </summary>
			/// <returns></returns>
			public bool HasMultiplePrimaryKeys()
			{
				// PrimaryKeyCount
				int primaryKeyCount = 0;
				
				// Int Count
				foreach(DataField field in this.ActiveFields)
				{
					// Is This A Primary Key
					if(field.PrimaryKey)
					{
						// Increment PrimaryKeyCount
						primaryKeyCount++;
					}
				}
				
				// Return True If Count Is Greater Then 1
				return (primaryKeyCount > 1);
				
			}
			#endregion

            #region ToString()
            /// <summary>
            /// This method returns the table.Name 
            /// when ToString is called.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                // return the name of the table.
                return this.Name;
            }
        #endregion

        #endregion

        #region Properties

            #region ActiveFields
            /// <summary>
            /// This read only property returns the Fields that have Exclude = false,
            /// which is every field unless you turn it off.
            /// </summary>
            public List<DataField> ActiveFields
			{
				get
				{
                    // initial value
                    List<DataField> activeFields = new List<DataField>();

                    // If the Fields object exists
                    if (Fields != null)
                    {
                        // Iterate the collection of DataTable objects
                        foreach (DataField field in Fields)
                        {
                            // if the table should not be Excluded
                            if (!field.Exclude)
                            {
                                // Add this Table
                                activeFields.Add(field);
                            }
                        }
                    }

                    // return value
					return activeFields;
				}
			}
			#endregion	

			#region Changes
			public bool Changes
			{
				get
				{
					// If Any field Has Changes Then The Fields Collection Has Changes
					foreach(DataJuggler.Net.DataRow Row in this.Rows)
					{
						if(Row.Changes)
						{
							return true;
						}
					}

					// No Changes
					return false;
				}
				set
				{
					// If Any field Has Changes Then The Fields Collection Has Changes
					foreach(DataJuggler.Net.DataRow Row in this.Rows)
					{
						Row.Changes = value;
					}

				}
			}
			#endregion

            #region CheckConstraints
            /// <summary>
            /// This property gets or sets the CheckConstraints for this table
            /// </summary>
            public List<CheckConstraint> CheckConstraints
            {
                get { return checkConstraints; }
                set {checkConstraints = value; }
            }
            #endregion
				
			#region ClassName
			public string ClassName
			{
				get
				{
					return className;
				}
				set
				{
					// format the class name
                    className = CSharpClassWriter.FormatClassNameEx(value);
				}
			}
			#endregion	

			#region ClassFileName
			public string ClassFileName
			{
				get
				{
					return classfilename;
				}
				set
				{
					classfilename = value;
				}
			}
			#endregion

			#region ConnectionString
			public string ConnectionString
			{
				get
				{
					return connectionstring;
				}
				set
				{
					connectionstring = value;
				}
			}
			#endregion	

			#region CreateCollectionClass
			public bool CreateCollectionClass
			{
				get
				{
					return createcollectionclass;
				}
				set
				{
					createcollectionclass = value;
				}
			}
			#endregion	
            
            #region ForeignKeys
            /// <summary>
            /// This property gets or sets the value for the ForeignKeys for this table
            /// </summary>
            public List<ForeignKeyConstraint> ForeignKeys
            {
                get
                {
                    // return the value
                    return foreignKeys;
                }
                set
                {
                    // set the value
                    foreignKeys = value; 
                }
            }
            #endregion

            #region HasCheckConstraints
            /// <summary>
            /// This property returns true if this object has a 'CheckConstraints'.
            /// </summary>
            public bool HasCheckConstraints
            {
                get
                {
                    // initial value
                    bool hasCheckConstraints = (this.CheckConstraints != null);

                    // return value
                    return hasCheckConstraints;
                }
            }
            #endregion

            #region HasIndexes
            /// <summary>
            /// This property returns true if this object has an 'Indexes'.
            /// </summary>
            public bool HasIndexes
            {
                get
                {
                    // initial value
                    bool hasIndexes = (this.Indexes != null);

                    // return value
                    return hasIndexes;
                }
            }
            #endregion

            #region Indexes
            /// <summary>
            /// This property gets or sets the value for Indexes
            /// </summary>
            public List<DataIndex> Indexes
            {
                get { return indexes; }
                set { indexes = value; }
            }
            #endregion

			#region Exclude
			public bool Exclude
			{
				get
				{
					return exclude;
				}
				set
				{
					exclude = value;
				}
			}
			#endregion	

			#region Fields
			public List<DataField> Fields
			{
				get
				{
					return fields;
				}
				set
				{
					fields = value;
				}
			}
			#endregion

            #region HasParentDatabase()
			/// <summary>
			/// Does this table have a ParentDatabase
			/// </summary>
			public bool HasParentDatabase
			{
				get
				{
                    // initial value
                    bool hasParentDatabase = (this.ParentDatabase != null);
            		
					// return value
					return hasParentDatabase;
				}
			}
			#endregion

            #region HasPrimaryKey()
			/// <summary>
			/// Does this table have a Primary Key
			/// </summary>
			public bool HasPrimaryKey
			{
				get
				{
                    // initial value
                    bool hasPrimary = (this.PrimaryKey != null);
            		
					// return value
					return hasPrimary;
				}
			}
			#endregion

            #region HasSchemaName
            /// <summary>
            /// This read only property returns True if this table has a SchemaName
            /// </summary>
            public bool HasSchemaName
            {
                get    
                {
                    // initial value
                    bool hasSchemaName = (!String.IsNullOrEmpty(this.SchemaName));

                    // return value
                    return hasSchemaName;
                }
            }
            #endregion
			
            #region IsView
            /// <summary>
            /// Is this a view or a table.
            /// </summary>
            public bool IsView
            {
                get { return isView; }
                set { isView = value; }
            } 
            #endregion
			
			#region Name
			public string Name
			{
				get
				{
					return name;
				}
				set
				{
					name = value;
					this.ClassName = name;					
				}
			}
			#endregion

            #region ObjectNameSpaceName
            /// <summary>
            /// This must be set yourself in the client during a
            /// primary build. It is needed to resolve name conflicts
            /// with the DataTier.Net.Client.ClientUtil.ConflictHelper.
            /// </summary>
            public string ObjectNameSpaceName
            {
                get { return objectNameSpaceName; }
                set { objectNameSpaceName = value; }
            } 
            #endregion

			#region ParentDatabase
			/// <summary>
			/// The name of the database this table is in.
			/// </summary>
			public Database ParentDatabase
			{
				get
				{
					return parentdatabase;
				}
				set
				{
					parentdatabase = value;

					// Set Properties That Derive From parentDatabase
					this.ClassFileName = this.ParentDatabase.ClassFileName;
					this.XmlFileName = this.ParentDatabase.XmlFileName;
				}
			}
			#endregion

			#region PrimaryKey
			/// <summary>
			/// This property returns the PrimaryKey for this tagle.
			/// </summary>
			public DataField PrimaryKey
			{
				get
				{
				    // initial value
				    DataField primaryKey = null;
				
				    // if the fields collection exists
					if(this.ActiveFields != null)
					{
					    // Check Each field
					    foreach(DataJuggler.Net.DataField field in this.ActiveFields)
					    {
					        // if this is the Primary Key field
						    if(field.PrimaryKey)
						    {
							    // set the primaryKey field
							    primaryKey = field;
							    
							    // break out of for loop
							    break;
						    }
					    }
	                }
					
					// Fixed Bug: PrimaryKey was not working 10/31/2007
					//            Switched to camelCase and made an bluner.
					
					// Return value
					return primaryKey;
				}
			}
			#endregion

			#region Rows
			public List<DataRow> Rows
			{
				get
				{
					return rows;
				}
				set
				{
					rows = value;
				}
			}
			#endregion

			#region Scope
			public DataManager.Scope Scope
			{
				get
				{
					return scope;
				}
				set
				{
					scope = value;
				}
			}
			#endregion

            #region SchemaName
            /// <summary>
            /// This property gets or sets the SchemaName
            /// </summary>
            public string SchemaName
            {
                get
                {
                    return schemaName;
                }
                set
                {
                    schemaName = value;
                }
            }
            #endregion

			#region Serializable
			public bool Serializable
			{
				get
				{
					return serializable;
				}
				set
				{
					serializable = value;
				}
			}
			#endregion

            #region SQLGenerator
            public SQLGenerator SQLGenerator
            {
                get
                {
                    return sqlgenerator;
                }
            }
            #endregion

            #region TableId
            /// <summary>
            /// This field does not actually mape to a database table.
            /// This is used by DataTier.Net in order to set the TableId
            /// when a DataTable is converted, so that when it is saved
            /// an update will take place and not an insert. 
            /// </summary>
			public int TableId
			{
				get
				{
					return tableId;
				}
				set
				{
					tableId = value;
				}
			}
			#endregion

            #region Tag
            /// <summary>
            /// This property gets or sets the value for 'Tag'.
            /// </summary>
            public string Tag
            {
                get { return tag; }
                set { tag = value; }
            }
            #endregion

			#region xmlFileName
			public string XmlFileName
			{
				get
				{
					return xmlfilename;
				}
				set
				{
					xmlfilename = value;
				}
			}
			#endregion

		#endregion

    }
	#endregion
    
}
