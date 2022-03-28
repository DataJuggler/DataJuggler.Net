

#region using statements

using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace DataJuggler.Net
{

    #region class Database
	public class Database
	{

		#region Private Variables
		private string classname;
		private string classfilename;
		private string connectionstring;
		private DataManager parentdatamanager;
		private bool exclude;
		private string name;
		private string password;
		private string path;
		private bool serializable;
		private List<StoredProcedure> storedprocedures;
		private List<DataTable> tables;
		private string xmlfilename;
        private List<Function> functions;
		#endregion

		#region Constructors

			#region Default Constructor
			public Database()
			{
				// Create New tables Class
				this.Tables = new List<DataTable>();
				
				// Create StoredProcedures
				this.StoredProcedures = new List<StoredProcedure>();
			    
			    // Initialize Strings
			    this.ConnectionString = "";
			    this.Name = "";

			}
			#endregion

			#region	 Database(DataManager parentDataManager)
			public Database(DataManager parentDataManager)
			{
				// Create ParentDataManager
				this.ParentDataManager = parentDataManager;
				
				// Create New tables Class
				tables = new List<DataTable>();

                // Initialize Strings
                this.ConnectionString = "";
                this.Name = "";

				
			}
			#endregion

		#endregion

		#region Methods
    
		#endregion
		
		#region Properties

            #region ActiveTables
            /// <summary>
            /// This read only property returns the Tables that have Exclude = false,
            /// which is every table unless you exclude it in the DataEditor control. 
            /// </summary>
            public List<DataTable> ActiveTables
			{
				get
				{
                    // initial value
                    List<DataTable> activeTables = new List<DataTable>();

                    // If the Tables object exists
                    if (this.HasTables)
                    {
                        // Iterate the collection of DataTable objects
                        foreach (DataTable dataTable in Tables)
                        {
                            // if the table should not be Excluded
                            if (!dataTable.Exclude)
                            {
                                // Add this Table
                                activeTables.Add(dataTable);
                            }
                        }
                    }

                    // return value
					return activeTables;
				}
			}
			#endregion	

			#region ClassName
			public string ClassName
			{
				get
				{
					return classname;
				}
				set
				{
					classname = value;
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

            #region Functions
            /// <summary>
            /// This property gets or sets the Functions for this database.
            /// </summary>
            public List<Function> Functions
            {
                get { return functions; }
                set { functions = value; }
            }
            #endregion

            #region HasFunctions
            /// <summary>
            /// This read only property returns true if this object contains a Functions collection.
            /// </summary>
            public bool HasFunctions
            {
                get
                {
                    // initial value
                    bool hasFunctions = (this.Functions != null);

                    // return value
                    return hasFunctions;
                }
            }
            #endregion

            #region HasOneOrMoreFunctions
            /// <summary>
            /// This read only property returns true if this object has one or more Functions.
            /// </summary>
            public bool HasOneOrMoreFunctions
            {
                get
                {
                    // initial value
                    bool hasOneOrMoreFunctions = ((this.HasFunctions) && (this.Functions.Count > 0));

                    // return value
                    return hasOneOrMoreFunctions;
                }
            }
            #endregion

            #region HasOneOrMoreTables
            /// <summary>
            /// This read only property returns true if this object has one or more tables.
            /// </summary>
            public bool HasOneOrMoreTables
            {
                get
                {
                    // initial value
                    bool hasOneOrMoreTables = ((this.HasTables) && (this.Tables.Count > 0));

                    // return value
                    return hasOneOrMoreTables;
                }
            }
            #endregion

            #region HasOneOrMoreStoredProcedures
            /// <summary>
            /// This read only property returns true if this object has one or more StoredProcedures.
            /// </summary>
            public bool HasOneOrMoreStoredProcedures
            {
                get
                {
                    // initial value
                    bool hasOneOrMoreStoredProcedures = ((this.HasStoredProcedures) && (this.StoredProcedures.Count > 0));

                    // return value
                    return hasOneOrMoreStoredProcedures;
                }
            }
            #endregion

            #region HasStoredProcedures
            /// <summary>
            /// This read only property returns true if this object contains a StoredProcedures collection.
            /// </summary>
            public bool HasStoredProcedures
            {
                get
                {
                    // initial value
                    bool hasStoredProcedures = (this.StoredProcedures != null);

                    // return value
                    return hasStoredProcedures;
                }
            }
            #endregion

            #region HasTables
            /// <summary>
            /// This read only property returns true if this object contains a Tables collection.
            /// </summary>
            public bool HasTables
            {
                get
                {
                    // initial value
                    bool hasTables = (this.Tables != null);

                    // return value
                    return hasTables;
                }
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
				}
			}
			#endregion

			#region ParentDataManager
			public DataManager ParentDataManager
			{
				get
				{
					return parentdatamanager;
				}
				set
				{
					parentdatamanager = value;

                    // If the parentdatamanager object exists
                    if (parentdatamanager != null)
                    {
					    // Set Properties That Derive From ParentDataManager
					    this.ClassFileName = parentdatamanager.ClassFileName;
					    this.XmlFileName = parentdatamanager.XmlFileName;
                    }
				}
			}
			#endregion

			#region Password
			public string Password
			{
				get
				{
					return password;
				}
				set
				{
					password = value;
				}
			}
			#endregion

			#region Path
			public string Path
			{
				get
				{
					return path;
				}
				set
				{
					path = value;
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
			
			#region StoredProcedures
			public List<StoredProcedure> StoredProcedures
			{
				get
				{
					return storedprocedures;
				}
				set
				{
					storedprocedures = value;
				}
			}
			#endregion

            #region Tables
            public List<DataTable> Tables
			{
				get
				{
					return tables;
				}
				set
				{
					tables = value;
				}
			}
			#endregion	

			#region XmlFileName
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
