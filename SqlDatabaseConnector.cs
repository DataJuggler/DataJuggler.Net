

#region using statements

using DataJuggler.Core.UltimateHelper;
using DataJuggler.Net.Enumerations;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using DataJuggler.Net.Xml.Writers;
using DataJuggler.Net.Delegates;
using DataJuggler.Net.Cryptography;

#endregion

namespace DataJuggler.Net
{

    #region class SQLDatabaseConnector
    /// <summary>
    /// Wrapper class for SqlDatabase Connection.
    /// </summary>
    public class SQLDatabaseConnector
	{

		#region Private Variables
		private SqlConnection databaseconnection;
		private SqlCommand command;
		private string failedreason;
		private bool loadviews;
		private string lastsql;
		private int lastidentity;
		private bool captureidentity;
		private bool isinsert;
        private string connectionString;
        private bool ignoreDataSync;
        private bool ignoreAzureFirewallRules;
        private const string MultipleResultSets = "MultipleActiveResultSets=True";
		#endregion

		#region Constructors

			#region Default Constructor
			/// <summary>
			/// Create a new instance of a sql Database Connector
			/// </summary>
			public SQLDatabaseConnector()
			{	
				// Create DatabaseConnection
				this.DatabaseConnection = new SqlConnection();
			}
			#endregion

		#endregion

		#region Methods

            #region BuildAddDefaultSQL(string tableName, DefaultValueConstraint constraint)
            /// <summary>
            /// Builds the ADD CONSTRAINT ... DEFAULT ... FOR sql for the constraint given. Uses the Definition text
            /// when available; falls back to the numeric DefaultValue for older schemas without a Definition.
            /// Skipped if the column already has a default (whatever its name).
            /// </summary>
            private string BuildAddDefaultSQL(string tableName, DefaultValueConstraint constraint)
            {
                // initial value
                string sql = "";

                // local
                string definition = "";

                // if the constraint exists and has a column
                if ((NullHelper.Exists(constraint)) && (TextHelper.Exists(constraint.ColumnName)))
                {
                    // prefer the raw definition, e.g. ((1)), (getdate()), ('Pending')
                    if (TextHelper.Exists(constraint.Definition))
                    {
                        definition = constraint.Definition;
                    }
                    else
                    {
                        // older schema: numeric value only
                        definition = "(" + constraint.DefaultValue + ")";
                    }

                    // guard: only if the column has no default yet (checked by column, so a different name still counts)
                    sql = "IF NOT EXISTS (SELECT 1 FROM sys.default_constraints dc INNER JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id" +
                          " WHERE dc.parent_object_id = OBJECT_ID(N'" + EscapeLiteral("[" + tableName + "]") + "') AND c.name = N'" + EscapeLiteral(constraint.ColumnName) + "')" + Environment.NewLine;

                    // build the sql
                    sql += "ALTER TABLE [" + tableName + "] ADD CONSTRAINT [" + constraint.ConstraintName + "] DEFAULT " + definition + " FOR [" + constraint.ColumnName + "]" + Environment.NewLine + "Go" + Environment.NewLine;
                }

                // return value
                return sql;
            }
            #endregion

            #region BuildConnectionString(string serverName, string databaseName)
            /// <summary>
            /// This method builds a connection string when Windows Authentication is used.
            /// </summary>
            /// <param name="ServerName"></param>
            /// <param name="DatabaseName"></param>
            /// <returns></returns>
            public string BuildConnectionString(string serverName, string databaseName)
            {
                // Return the connection string
                return ConnectionStringHelper.BuildConnectionString(serverName, databaseName);
            }
            #endregion

            #region BuildConnectionString(string serverName, string databaseName, string userId, string password)
            /// <summary>
			/// This method builds a connection string when SQLServer Authentication is used.
            /// A UserId and Password are required for this method.
			/// </summary>
			/// <param name="ServerName"></param>
			/// <param name="DatabaseName"></param>
			/// <param name="UserId"></param>
			/// <param name="Password"></param>
			/// <returns></returns>
			public string BuildConnectionString(string serverName, string databaseName, string userId, string password)
			{
                // Return the connection string
                return ConnectionStringHelper.BuildConnectionString(serverName, databaseName, userId, password);
			}
			#endregion
			
			#region BuildTableSQL(DataJuggler.Net.DataTable dataTable)
			public string BuildTableSQL(DataJuggler.Net.DataTable dataTable)
			{
				// Create sql Statement To Select All From This table
                return dataTable.SQLGenerator.CreateSelectAllSQL(dataTable);
			}
			#endregion

            #region CloneField(DataField sourceField)
            /// <summary>
			/// This class is used to Clone a DataField.
            /// This method had to be written when the
            /// DataList<Fields> was replaced with
            /// a List<DataField> to work with 
            /// XmlMirror to add remote functionality.
			/// </summary>
			/// <param name="IngoreFieldValue"></param>
			/// <returns></returns>
			public static DataField CloneField(DataField sourceField)
			{
				// Create field To Return
				DataField Field = new DataField();

				// Clone Each Property
				Field.AccessMode = sourceField.AccessMode;
				Field.DataType = sourceField.DataType;
				Field.Exclude = sourceField.Exclude;
				Field.FieldName = sourceField.FieldName;
				Field.Index = sourceField.Index;
				Field.IsNullable = sourceField.IsNullable;
				Field.PrimaryKey = sourceField.PrimaryKey;
				Field.Required = sourceField.Required;
				Field.Scope = sourceField.Scope;
				Field.Size = sourceField.Size;
                
				// Return This field
				return Field;
						
			}
			#endregion

            #region CloneFields(List<DataField> sourceFields)
            /// <summary>
			/// This class is used to Clone a DataField.
            /// This method had to be written when the
            /// DataList<Fields> was replaced with
            /// a List<DataField> to work with 
            /// XmlMirror to add remote functionality.
			/// </summary>
			/// <param name="IngoreFieldValue"></param>
			/// <returns></returns>
			public static List<DataField> CloneFields(List<DataField> sourceFields)
			{
                // initial value
                List<DataField> fields = null;

                // If the sourceFields object exists
                if (sourceFields != null)
                {
                    // Create a new collection of 'DataField' objects.
                    fields = new List<DataField>();

                    // If the sourceFields collection exists and has one or more items
                    if (ListHelper.HasOneOrMoreItems(sourceFields))
                    {
                        // Iterate the collection of DataField objects
                        foreach (DataField sourceField in sourceFields)
                        {
                            // Clone this field
                            DataField field = CloneField(sourceField);

                            // Add this field to the return value
                            fields.Add(field);
                        }
                    }
                }
                
				// return value
				return fields;
			}
			#endregion

            #region Close()
            /// <summary>
            /// This method closes the database connection
            /// </summary>
            public void Close()
            {
                // if the database connection exists
                if (this.HasDatabaseConnection)
                {
                    // close the connection
                    this.DatabaseConnection.Close();
                }
            } 
            #endregion

			#region ConvertStringToBoolean(string booleanString)
			public bool ConvertStringToBoolean(string booleanString)
			{

				try
				{
					if(booleanString != null)
					{
						if(booleanString.ToLower() == "true")
						{
							return true;
						}
						else
						{
							return false;
						}
					}
					else
					{
						// defaults to false
						return false;
					}
				}
				catch(Exception e)
				{
					// string to hold the error
					string error = e.ToString();
					// False 
					return false;
				}
					
			}
			#endregion
            
            #region EscapeLiteral(string value)
            /// <summary>
            /// returns the value with single quotes doubled, for use inside N'...'
            /// </summary>
            private static string EscapeLiteral(string value)
            {
                // initial value
                string escapedValue = "";

                // If the value string exists
                if (TextHelper.Exists(value))
                {
                    // double any single quotes
                    escapedValue = value.Replace("'", "''");
                }

                // return value
                return escapedValue;
            }
            #endregion

			#region ExecuteNonQuery(string sql)
			public bool ExecuteNonQuery(string sql)
			{
				try
				{	
					// Set Last sql
                    this.LastSQL = sql;
					
					// Create command Object
                    this.Command = new SqlCommand(sql, this.DatabaseConnection);
			
					// ExecuteNonQuery
					this.Command.ExecuteNonQuery();

					// return True 
					return true;
				}
				catch(Exception e)
				{
					// Set Failed Reason To Error Message
					this.FailedReason = e.ToString();

					// Return false
					return false;
				}
				
			}
			#endregion

            #region ExecuteSql(string sql)
            /// <summary>
            /// This method executes the sql
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public DataSet ExecuteSql(string sql)
            {
                // initial value
                DataSet dataSet = null;

                // if this object is currently connected
                if (this.Connected)
                {
                    // Set Last sql
                    this.LastSQL = "Sql Text: " + sql;

                    // Create command Object
                    this.Command = new SqlCommand(sql, this.DatabaseConnection);

                    // Set CommandType
                    this.Command.CommandType = System.Data.CommandType.Text;

                    // Create DataAdapter
                    SqlDataAdapter Adapter = new SqlDataAdapter(this.Command);

                    // Create And Open Data
                    dataSet = new DataSet();

                    // Fill DataAdapter
                    Adapter.Fill(dataSet);
                }

                // return value
                return dataSet;
            } 
            #endregion
			
			#region ExecuteStoredProcedure(StoredProcedure procedure)
			/// <summary>
			/// Execute A StoredProcedure That Does Not Return A DataSet or Value
			/// Execute An Insert, Update or Delete StoredProcedure
			/// </summary>
			/// <param name="procedure"></param>
			/// <returns></returns>
			public bool ExecuteStoredProcedure(StoredProcedure procedure)
			{
				try
				{	
					// Set Last sql
					this.LastSQL = "Stored procedure: " + procedure.ProcedureName;
					
					// Create command Object
					this.Command = new SqlCommand(procedure.ProcedureName,this.DatabaseConnection);
					
					// Set CommandType
					this.Command.CommandType = System.Data.CommandType.StoredProcedure;
					
					// Add Any Parameters
					foreach(StoredProcedureParameter Param in procedure.Parameters)
					{
						// Add Parameter Value
						this.Command.Parameters.Add(Param.ParameterValue);
					}
			
					// ExecuteNonQuery
					this.Command.ExecuteNonQuery();

					// return True 
					return true;
					
				}
				catch(Exception e)
				{
					// Set Failed Reason To Error Message
					this.FailedReason = e.ToString();

					// Return false
					return false;
				}
				
			}
			#endregion
			
			#region ExecuteStoredProcedure(StoredProcedure procedure, string returnDataSetTableName)
			/// <summary>
			/// Execute A StoredProcedure That Returns A DataSet 
			/// Execute A Select Stored procedure
			/// </summary>
			/// <param name="procedure"></param>
			/// <returns></returns>
			public System.Data.DataSet ExecuteStoredProcedure(StoredProcedure procedure, string returnDataSetTableName)
			{
				try
				{	
					// Set Last sql
					this.LastSQL = "Stored procedure: " + procedure.ProcedureName;
					
					// Create command Object
					this.Command = new SqlCommand(procedure.ProcedureName,this.DatabaseConnection);
					
					// Set CommandType
					this.Command.CommandType = System.Data.CommandType.StoredProcedure;
					
					// Add Any Parameters
					foreach(StoredProcedureParameter Param in procedure.Parameters)
					{
						// Add Parameter Value
						this.Command.Parameters.Add(Param.ParameterValue);
					}
					
					// Create DataAdapter
					SqlDataAdapter Adapter = new SqlDataAdapter(this.Command);

					// Create And Open Data
					DataSet DS = new DataSet();
						
					// Fill DataAdapter
					Adapter.Fill(DS,returnDataSetTableName);

					// return True 
					return DS;
					
				}
				catch(Exception e)
				{
					// Set Failed Reason To Error Message
					this.FailedReason = e.ToString();

					// Return false
					return new System.Data.DataSet();
				}
				
			}
			#endregion

            #region FindDepenciesForStoredProcedure(string procedureName, List<DataTable> allTables)
            /// <summary>
            /// This method finds all dependent tables and views for a stored procedure.
            /// Note: The allTables collection must exist and this works only for single database querries.
            /// </summary>
            /// <param name="procedureName">The name of the procedure to find dependencies for</param>
            /// <param name="allTables">All the tables for a database.</param>
            /// <returns></returns>
            public List<DataTable> FindDepenciesForStoredProcedure(string procedureName, List<DataTable> allTables)
            {
                // initial value
                List<DataTable> depencies = new List<DataTable>();

                try
                {
                    // if the procedureName is set and the allTables collection contains at least one item
                    if ((TextHelper.Exists(procedureName)) && (ListHelper.HasOneOrMoreItems(allTables)))
                    {
                        // query to load all table names for this procedure
                        string sql = "SELECT objects.name As DepencyName FROM sys.procedures INNER  JOIN sys.all_sql_modules ON all_sql_modules.object_id = procedures.object_id LEFT  JOIN sys.objects ON objects.name <> procedures.name AND all_sql_modules.definition LIKE '%' + objects.name + '%' WHERE  procedures.name = '" + procedureName + "'";

                         // Open The command Object
                        SqlCommand command = new SqlCommand(sql, DatabaseConnection);

                        // Create a Data adapter
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        // Create And Open Data
                        DataSet DS = new DataSet();

                        // Fill DataAdapter
                        adapter.Fill(DS, "tables");

                        // Load the CheckConstraints
                        foreach (System.Data.DataRow dataRow in DS.Tables["tables"].Rows)
                        {
                            // set the tableName
                            string tableName = (string) dataRow["DepencyName"];

                            // If the tableName string exists
                            if (TextHelper.Exists(tableName))
                            {
                                // attempt to find this table in allTables
                                DataTable tempDepency = allTables.FirstOrDefault(x => x.Name == tableName);

                                // If the tempDepency object exists
                                if (NullHelper.Exists(tempDepency))
                                {
                                    // Add this table
                                    depencies.Add(tempDepency);
                                }
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();

                    // Write the error to the debugger
                    DebugHelper.WriteDebugError("FindDepenciesForStoredProcedure", "SQLDatabaseConnector", error);
                }

                // return value
                return depencies;
            }
            #endregion

            #region FindInsertIndex(List<DataField> dataFields, DataField field) 
            /// <summary>
            /// This method is used to find the field index of where to insert a field
            /// </summary>
            /// <param name="dataFields"></param>
            /// <param name="field"></param>
            /// <returns></returns>
            public int FindInsertIndex(List<DataField> dataFields, DataField field) 
            {
                // initial value
                int index = -1;

                // local
                int tempIndex = -1;

                // iif the dataFields exist and the field exists
                if ((ListHelper.HasOneOrMoreItems(dataFields)) && (NullHelper.Exists(field)))
                {
                    // Iterate the collection of DataField objects
                    foreach (DataField tempField in dataFields)
                    {
                        // Increment the value for tempIndex
                        tempIndex++;

                        // set the value for compare
                        int compare = String.Compare(tempField.FieldName, field.FieldName);

                        // if we have reached the insert index
                        if (compare > 0)
                        {
                            // set the return value
                            index = tempIndex;

                            // break out of the loop
                            break;
                        }
                    }

                    // if the index was not found
                    if (index < 0)
                    {
                        // set the return value to the end (not found goes at the end)
                        index = dataFields.Count;
                    }
                }
                else
                {
                    // set to 0 for the first one
                    index = 0;
                }

                // return value
                return index;
            }
            #endregion
			
            #region FindPrimaryKeysCount(List<DataTable> depencies)
            /// <summary>
            /// This method is used to find the count of primary keys in the tables collection listed
            /// </summary>
            /// <param name="tables"></param>
            /// <returns></returns>
            public int FindPrimaryKeysCount(List<DataTable> depencies)
            {
                // initial value
                int primaryKeysCount = 0;

                // If the dependencies collection exists and has one or more items
                if (ListHelper.HasOneOrMoreItems(depencies))
                {
                    // Iterate the collection of DataTable objects
                    foreach (DataTable table in depencies)
                    {
                        // if there are one or more fields                       
                        if (ListHelper.HasOneOrMoreItems(table.ActiveFields))
                        {
                            // iterate the collection of fields
                            foreach (DataField field in table.ActiveFields)
                            {
                                // if this field is a Primary Key
                                if (field.PrimaryKey)
                                {
                                    // Increment the value for primaryKeysCount
                                    primaryKeysCount++;
                                }
                            }
                        }
                    }
                }

                // return value
                return primaryKeysCount;
            }
            #endregion

            #region FindField(List<DataField> fields, string fieldName)
            /// <summary>
            /// This method finds the Index of a field in the fields collection.
            /// This was needed after the DataList<Fields> was removed
            /// due to using XmlMirror to build writers work better with
            /// List<T> objects than Collections that inherit from CollectionBase
            /// </summary>
            /// <param name="fields"></param>
            /// <returns>If found the DataField being sought</returns>
			public static DataField FindField(List<DataField> fields, string fieldName)
			{
				// Initial Value (Not Found)
                DataField field = null;

                // If the fields collection exists and has one or more items
                if (ListHelper.HasOneOrMoreItems(fields))
                {
				    // Check Each Column
                    foreach (DataField tempField in fields)
				    {
					    // If this Is the Correct field 
					    if(tempField.FieldName.ToLower() == fieldName.ToLower())
					    {
						    // set the return value
						    field = tempField;

                            // break out of the loop
                            break;
					    }
				    }
                }
				
				// return value
				return field;
			}
			#endregion

            #region FindFieldIndex(List<DataField> fields, string fieldName)
            /// <summary>
            /// This method finds the Index of a field in the fields collection.
            /// This was needed after the DataList<Fields> was removed
            /// due to using XmlMirror to build writers work better with
            /// List<T> objects than Collections that inherit from CollectionBase
            /// </summary>
            /// <param name="fields"></param>
             /// <returns>If found the index of the DataField being sought</returns>
			public static int FindFieldIndex(List<DataField> fields, string fieldName)
			{
				// Initial Value (Not Found)
                int index = -1;

                // local
                int tempIndex = -1;

                // If the fields collection exists and has one or more items
                if (ListHelper.HasOneOrMoreItems(fields))
                {
				    // Check Each Column
                    foreach (DataField field in fields)
				    {
                        // Increment the value for tempIndex
					    tempIndex++;
					
					    // If this Is the Correct field 
					    if(field.FieldName.ToLower() == fieldName.ToLower())
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
			
			#region FindFieldOrdinalInDataRow(System.Data.DataSet ds, string tableName, string fieldName, string fieldCaption)
			public int FindFieldOrdinalInDataRow(System.Data.DataSet ds, string tableName, string fieldName, string fieldCaption)
			{
				// Initial Value (Not Found)
				int fieldOrdinal = -1; 

                // local
                int index = -1;
				
				// Check Each Column
                foreach (System.Data.DataColumn col in ds.Tables[tableName].Columns)
				{
					// Increment fieldOrdinal
					index++;
					
					// If this Is the Correct field 
					if(col.ColumnName.ToLower() == fieldName.ToLower())
					{
						// Return fieldOrdinal
						fieldOrdinal = index;

                        // break out of the loop
                        break;
					}
					
					// If this Is the Correct field 
					if(col.ColumnName.ToLower() == fieldCaption.ToLower())					
					{					
						// Return fieldOrdinal
						fieldOrdinal = index;

                        // break out of the loop
                        break;
					}
				}
				
				// Return fieldOrdinal
				return fieldOrdinal;
				
			}
			#endregion

            #region FindSchemaName(string name)
            /// <summary>
            /// This method finds the SchemaName for the table given
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            private string FindSchemaName(string name)
            {
                // initial value
                string schemaName = "";

                try
                {
                    // sql Statement To Select All tables
                    string sql = "SELECT '[' + SCHEMA_NAME(schema_id)+']' AS SchemaName FROM sys.tables Where Name = '" + name + "'";

                    // Open The command Object
                    SqlCommand command = new SqlCommand(sql, DatabaseConnection);
                    SqlDataAdapter Adapter = new SqlDataAdapter(command);

                    // Create And Open Data
                    DataSet dataSet = new DataSet();

                    // Fill DataAdapter
                    Adapter.Fill(dataSet, "SchemaName");

                    // itereate the Rows
                    foreach (System.Data.DataRow dr in dataSet.Tables["SchemaName"].Rows)
                    {
                        // set the schemaName
                        schemaName = dr[0].ToString();

                        // break out of the loop
                        break;
                    }

                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();
                }

                // return value
                return schemaName;
            }
            #endregion

            #region FindTable(List<DataTable> tables, string tableName)
            /// <summary>
            /// This method finds a table in the tables collection.
            /// This was needed after the DataList<Tables> was removed
            /// due to using XmlMirror to build writers work better with
            /// List<T> objects than Collections that inherit from CollectionBase
            /// </summary>
            /// <param name="tables"></param>
            /// <returns>If found the DataTable being sought</returns>
			public static DataTable FindTable(List<DataTable> tables, string tableName)
			{
				// Initial Value (Not Found)
                DataTable table = null;

                // If the tables collection exists and has one or more items
                if (ListHelper.HasOneOrMoreItems(tables))
                {
                    // Iterate the collection of DataTable objects
                    foreach (DataTable tempTable in tables)
				    {
					    // If this Is the Correct table name 
					    if(tempTable.Name.ToLower() == tableName.ToLower())
					    {
						    // set the return value
						    table = tempTable;

                            // break out of the loop
                            break;
					    }
				    }
                }
				
				// return value
				return table;
			}
			#endregion

            #region FindTableIndex(List<DataTable> tables, string tableName)
            /// <summary>
            /// This method finds the Index of a table in the tables collection.
            /// </summary>
            /// <param name="tables">table collection to search</param>
            /// <param name="tableName">The tableName to find</param>
            /// <returns>If found the index of the DataTable being sought</returns>
			public static int FindTableIndex(List<DataTable> tables, string tableName)
			{
				// Initial Value (Not Found)
                int index = -1;

                // local
                int tempIndex = -1;

                // If the tables collection exists and has one or more items
                if (ListHelper.HasOneOrMoreItems(tables))
                {
				    // Check Each Column
                    foreach (DataTable table in tables)
				    {
                        // Increment the value for tempIndex
					    tempIndex++;
					
					    // If this Is the Correct field 
					    if(table.Name.ToLower() == tableName.ToLower())
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

            #region GetDatabaseNamesForServer(string serverName, string connectionString)
            /// <summary>
            /// This method attempts to return the database names for the serverName given.
            /// </summary>
            /// <param name="serverName"></param>
            /// <returns></returns>
            public static List<string> GetDatabaseNamesForServer(string serverName, string connectionString)
            {
                // initial value
                List<string> databases = new List<string>();

                try
                {
                    // We must first attempt to connect to this server
                    SqlConnection connection = new SqlConnection(connectionString);
                 
                    // open the connection
                    connection.Open();

                    // set the sql statement
                    string sql = "SELECT Name FROM sys.databases where owner_sid != 1 Order By Name";

                    // Open The command Object
                    SqlCommand command = new SqlCommand(sql, connection);

                    // Create a Data adapter
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    // Create And Open Data
                    DataSet DS = new DataSet();

                    // Fill DataAdapter
                    adapter.Fill(DS, "Databases");

                    // Load the CheckConstraints
                    foreach (System.Data.DataRow dataRow in DS.Tables["Databases"].Rows)
                    {
                        // set the tableName
                        string databaseName = (string) dataRow["Name"];

                        // If the databaseName string exists
                        if (TextHelper.Exists(databaseName))
                        {
                            // add the database name
                            databases.Add(databaseName);
                        }
                    }

                    // close the connection
                    connection.Close();
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();

                    // Write the debug to the output
                    DebugHelper.WriteDebugError("GetDatabaseNamesForServer", "SQLDatabaseConnector", error);
                }

                // return value
                return databases;
            }
            #endregion

            #region GetDatabaseSchemaHash(Database database, bool ignoreDataSync = true, bool ignoreAzureFirewallRules = true)
			/// <summary>
			/// Loads The Database Schema For The Database and returns a hashed value representing the current schema
			/// </summary>
			/// <param name="database"></param>
			/// <param name="ConnectionString"></param>
			/// <returns></return>
			public string GetDatabaseSchemaHash(Database database, bool ignoreDataSync = true, bool ignoreAzureFirewallRules = true)
			{  
                // initial value
                string schemaHash = "";

                // local
                int schemaValue = 0;

			    // call the method to load the schema
				database = LoadDatabaseSchema(database, ignoreDataSync, ignoreAzureFirewallRules);

                // If the database object exists
                if (NullHelper.Exists(database))
                {
                    // if the Tables collection exists and has one or more items
                    if (ListHelper.HasOneOrMoreItems(database.Tables))
                    {
                        // iterate the tables
                        foreach (DataTable table in database.Tables)
                        {
                            // Add the value of this table Name
                            schemaValue += GetTextValue(table.Name);

                            // if the table has fields
                            if (ListHelper.HasOneOrMoreItems(table.Fields))
                            {
                                // iterate the fields
                                foreach (DataField field in table.Fields)
                                {
                                    // Add the value of this field Name
                                    schemaValue += GetTextValue(field.FieldName);

                                    // Add the value of this field DBDataType
                                    schemaValue += GetTextValue(field.DBDataType);
                                }
                            }
                        }
                    }

                    // if the schemaValue has been calculated
                    if (schemaValue > 0)
                    {
                        // convert to a number
                        string temp = schemaValue.ToString();

                        // encrypt the string 
                        schemaHash = CryptographyHelper.EncryptString(temp);
                    }
                }

				// Return Database
				return schemaHash;
			}
			#endregion

            #region GetNextField(ref string sql)
            /// <summary>
            /// This method finds the fieldNames in a sql statement.
            /// The first fieldName found is returned, then the fieldName
            /// is removed from the sql string.
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            private string GetNextField(ref string sql)
            {
                // initial value
                string fieldName = "";
                
                // verify we have a string
                if(!String.IsNullOrEmpty(sql))
                {
                    // Create a copy of sql
                    string Sql = sql;
                    
                    // Remove the select so it is just a list of fields
                    Sql = Sql.Replace("Select ", "");
                    
                    // Remove everything after the word from
                    Sql = Sql.Substring(0, Sql.IndexOf("From") -1);
                    
                    // locals
                    int startIndex = Sql.IndexOf("[");
                    int endIndex = Sql.IndexOf("]");
                    int len = endIndex - startIndex + 1;
                    
                    // verify we have a string
                    if(!String.IsNullOrEmpty(Sql))
                    {
                        // verify both items were fond
                        if ((startIndex >= 0) && (endIndex >= 0))
                        {
                            // Get this fieldName
                            fieldName = Sql.Substring(startIndex, len);
                        }
                        
                        // Remove the comma if any
                        int commaIndex = Sql.IndexOf(",");
                        
                        // local
                        string replaceField = fieldName;
                        
                        // if there is a comma then this is not the last field
                        if(commaIndex > 0)
                        {
                            // remove up to and including the comma
                            replaceField = fieldName + ",";
                        }

                        // remove the fieldName from the original sql statement.
                        sql = sql.Replace(replaceField, "");
                    }
                }
                
                // return value
                return fieldName;
            } 
            #endregion

            #region GetProcedureText(string procedureName)
            /// <summary>
            /// This method returns the Procedure Text for the procedure name and connection string
            /// </summary>
            private string GetProcedureText(string procedureName)
            {
                // initial value
                string procedureText = "";

                try
                {
                    // verify both strings exist
                    if (TextHelper.Exists(procedureName, connectionString))
                    {
                        // if connected
                        if (Connected)
                        {
                            // string 
                            string sql = "sp_helptext " + procedureName;

                            // create the dataSet
                            System.Data.DataSet dataSet = ExecuteSql(sql);

                            // if the dataSet exists
                            if ((dataSet != null) && (dataSet.Tables != null) && (dataSet.Tables.Count > 0))
                            {
                                // create a string builder
                                StringBuilder sb = new StringBuilder();

                                // get a reference to the resultTable
                                System.Data.DataTable resultTable = DataHelper.ReturnFirstTable(dataSet);
                               
                                // if the table exists
                                if (resultTable != null)
                                {
                                    // iterate the rows
                                    foreach (System.Data.DataRow row in resultTable.Rows)
                                    {
                                        // get the text for this row
                                        string rowText = row.ItemArray[0].ToString();

                                        // append the row text
                                        sb.Append(rowText);
                                    }
                                }

                                // get the procedureText, and replace out the word [dbo]. as this
                                // does not always show up, same for braces [ ]
                                procedureText = sb.ToString().Replace("[dbo].", "").Replace("[", "").Replace("]", "").Trim();
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();
                }

                // return value
                return procedureText;
            }
            #endregion

            #region GetSchemaFields(DataTable table)
            /// <summary>
            /// This method returns a dataTable containing the schema information for a table
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public List<DataField> GetSchemaFields(DataTable table)
            {
                // intial value
                List<DataField> fields = new List<DataField>();

                try
                {
                    // If the table object exists
                    if (NullHelper.Exists(table))
                    {
                        // sql Statement To Select All tables
                        string sql = "Select * From [" + table.Name + "]";

                        // if this table has a SchemaName and the SchemaName is not Dbo
                        if ((table.HasSchemaName) && (table.SchemaName.ToLower() != "[dbo]"))
                        {
                            // Include the SchemaName 
                            sql = "Select * From " + table.SchemaName + "." + "[" + table.Name + "]";
                        }

                        // Open The command Object
                        this.Command = new SqlCommand(sql, this.DatabaseConnection);

                        // Open Reader
                        SqlDataReader reader = Command.ExecuteReader(CommandBehavior.KeyInfo);

                        // Creatre DataTable
                        System.Data.DataTable schemaTable = reader.GetSchemaTable();

                         // Loop Through table
                        foreach (System.Data.DataRow databaseField in schemaTable.Rows)
                        {
                            // Create New DataField
                            DataJuggler.Net.DataField field = new DataField();

                            // Set fieldName
                            field.FieldName = DataTable.CapitalizeFirstChar(databaseField["ColumnName"].ToString());

                            // Set the DBFieldName
                            field.DBFieldName = databaseField["ColumnName"].ToString();

                            // Update 3.26.2016: Table DatabaseLog in AdventureWorks has Event has a field name
                            if (field.FieldName == "Event")
                            {
                                // change the FieldName
                                field.FieldName = "EventName";
                            }

                            // Update 3.26.2016: Table DatabaseLog in AdventureWorks has Event has a field name
                            if (field.FieldName == "Object")
                            {
                                // change the FieldName
                                field.FieldName = "ObjectName";
                            }

                            // check if this field is hidden, if yes do not include it
                            bool hidden = (bool)databaseField["IsHidden"];

                            // if the field is not hidden 
                            // solves problem of aliased fields appearing in the select statements
                            if (!hidden)
                            {
                                // Set Properties

                                // Set fieldOrdinal
                                field.FieldOrdinal = (int)databaseField["ColumnOrdinal"];

                                // Set Key
                                field.PrimaryKey = (bool)databaseField["IsKey"];

                                // Set dataType
                                string dataType = databaseField["dataType"].ToString();

                                // Set the DBDatatype
                                field.DBDataType = dataType;

                                 // Set IsNullable
                                field.IsNullable = (bool)databaseField["AllowDbNull"];

                                // if this is a decimal
                                if (dataType == "System.Decimal")
                                {
                                    // Attempt to get the Precision & Scale
                                    try
                                    {
                                        // attemp to get the money string
                                        string sqlDataType = databaseField[23].ToString();

                                        // if the money string exists
                                        if (sqlDataType == "System.Data.SqlTypes.SqlMoney")
                                        {
                                            // set the datatype
                                            field.DataType = DataManager.DataTypeEnum.Currency;
                                        }
                                        else if (sqlDataType == "System.Data.SqlTypes.SqlDecimal")
                                        {
                                            // set the dataType
                                            field.DataType = DataManager.DataTypeEnum.Decimal;

                                            // set precision and scale
                                            string precision = databaseField[3].ToString();
                                            string scale = databaseField[4].ToString();

                                            // if the precision and scale exist
                                            if ((!String.IsNullOrEmpty(precision)) && (!String.IsNullOrEmpty(scale)))
                                            {
                                                // set the precision and scale
                                                field.Precision = Int32.Parse(precision);
                                                field.Scale = Int32.Parse(scale);
                                            }
                                        }
                                    }
                                    catch (Exception error2)
                                    {
                                        // for debugging only
                                        string err2 = error2.ToString();
                                    }
                                }
                                else
                                {
                                    // see if this is auto increment
                                    field.IsAutoIncrement = (bool)databaseField["isAutoIncrement"];

                                    // Parse dataType
                                    field.DataType = ParseDataType(dataType, field);
                                }

                                // Set ColumnSize
                                field.Size = (int)databaseField["ColumnSize"];

                                // Set IsNullable
                                field.IsReadOnly = (bool)databaseField["IsReadOnly"];

                                // If this is a view
                                if (table.IsView)
                                {
                                    // For a view all fields are read write
                                    field.AccessMode = DataManager.AccessMode.ReadWrite;
                                }
                                else
                                {
                                    // Set Access Mode
                                    if (field.IsReadOnly)
                                    {
                                        // Set AccessMode
                                        field.AccessMode = DataManager.AccessMode.ReadOnly;
                                    }
                                    else
                                    {
                                        field.AccessMode = DataManager.AccessMode.ReadWrite;
                                    }
                                }

                                // Add to Fields Collection
                                fields.Add(field);
                            }
                        }

                        // Close Reader
                        reader.Close();

                    }
                }
                catch (Exception error)
                {
                    // This happens for Azure SQL databases
                    if (!error.Message.Contains("database_firewall_rules"))
                    {
                        // write the error to the console window
                        DebugHelper.WriteDebugError("GetSchemaTable", "SQLDatabaseConnector", error);
                    }
                }

                // return value
                return fields;
            }
            #endregion

            #region GetSqlObject(string objectName)
            /// <summary>
            /// This method returns a SQL Object containing the Defintion, UseAnsiNulls, UseQuotedIdentifier, IsSchemaBound
            /// This is used by DB Compare to know if a Stored Procedure or View should have Uses Ansi Nulls and Use Quoted Identifier
            /// on before the objects is created
            /// </summary>
            /// <param name="viewName"></param>
            /// <returns></returns>
            public SqlObject GetSqlObject(string objectName)
            {
                // initial value
                SqlObject sqlObject = new SqlObject();

                try
                {
                    string sourceSQL = "Select definition, uses_ansi_nulls, uses_quoted_identifier, is_schema_bound FROM";
                    sourceSQL += " sys.sql_modules Where object_id = object_id('" + objectName + "');";

                    // Open The command Object
                    SqlCommand command = new SqlCommand(sourceSQL, DatabaseConnection);

                    // Create a Data adapter
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    // Create And Open Data
                    DataSet DS = new DataSet();

                    // Fill DataAdapter
                    adapter.Fill(DS, "Views");

                    sqlObject.Defintion = DS.Tables[0].Rows[0].ItemArray[0].ToString();
                    sqlObject.UsesAnsiNulls = BooleanHelper.ParseBoolean(DS.Tables[0].Rows[0].ItemArray[1].ToString(), false, false);
                    sqlObject.UsesQuotedIdentifier = BooleanHelper.ParseBoolean(DS.Tables[0].Rows[0].ItemArray[2].ToString(), false, false);
                    sqlObject.IsSchemaBound = BooleanHelper.ParseBoolean(DS.Tables[0].Rows[0].ItemArray[3].ToString(), false, false);
                }
                catch (Exception error)
                {
                    // for debugging only for now
                    DebugHelper.WriteDebugError("GetViewText", "SQLDatabaseConnector", error);
                }

                // return value
                return sqlObject;
            }
            #endregion

            #region GetStoredProcParameters(string procedureName)
            /// <summary>
            /// This method loads the Parameters for the execution of a Stored procedure.
            /// </summary>
            /// <returns></returns>
            public List<StoredProcedureParameter> GetStoredProcParameters(string procedureName)
            {
                // initial value
               List<StoredProcedureParameter> parameters = new List<StoredProcedureParameter>();

                try
                {
                    // if this is connected
                    if (this.Connected)
                    {
                        // set the sql
                        string sql = "Select s.id , s.name, t.name as [type], t.length from  syscolumns s inner join systypes t on s.xtype = t.xtype where id = (select id from sysobjects where name = '[PROCEDURENAME]')";

                        // replace out the procedureName
                        sql = sql.Replace("[PROCEDURENAME]", procedureName);

                        // load the dataSet
                        DataSet dataSet = this.ExecuteSql(sql);

                        // get the first dataTable
                        System.Data.DataTable dataTable = dataSet.Tables[0];

                        // if the dataTable has Data
                        if (dataTable.Rows.Count > 0)
                        {
                            // iterate each row in the dataTable
                            foreach (System.Data.DataRow row in dataTable.Rows)
                            {
                                // this is a parameter
                                StoredProcedureParameter parameter = new StoredProcedureParameter();

                                // set the name
                                parameter.ParameterName = row["Name"].ToString();

                                // set the Data type
                                string dataTypeString = row["type"].ToString();

                                // faking this out
                                DataField field = new DataField();
                                field.FieldName = parameter.ParameterName;

                                // parse the DataType
                                parameter.DataType = ParseDataType(dataTypeString, field);

                                // convert to an integer
                                parameter.Length = Convert.ToInt32(row["Length"]);

                                // add this parameter to the return value
                                parameters.Add(parameter);
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();

                    // set to null so we know an error occurred
                    parameters = null;
                }

                // return value
                return parameters;
            } 
            #endregion

            #region GetTablesForProcedure(List<DataTable> allTables, StoredProcedure procedure)
            /// <summary>
            /// This method returns the tables that a store procedure is dependant on.
            /// This is used to help identity the primary key and other information 
            /// about the Data fields.
            /// </summary>
            /// <param name="allTables"></param>
            /// <param name="procedure"></param>
            /// <returns></returns>
            public List<DataTable> GetTablesForProcedure(List<DataTable> allTables, StoredProcedure procedure)
            {
                // initial value
                List<DataTable> tables = null;

                // locals
                string procedureName = "";

                try
                {
                    // if there are one or more tables in the allTables collection and if the procedure exists
                    if ((ListHelper.HasOneOrMoreItems(allTables)) && (NullHelper.Exists(procedure)))
                    {
                        // Set the value for procedureName
                        procedureName = procedure.ProcedureName;
                    }
                }
                catch (Exception error)
                {
                    // for debugging only for now
                    string err = "An error occurred loading the tables for the procedure named '" + procedureName + "'" + error.ToString();

                    // Write the line to the output Window
                    DebugHelper.WriteDebugError("GetTablesForProcedure", "SQLDatabaseConnector", error);
                }

                // return value
                return tables;
            }
            #endregion

            #region GetTextValue(string text)
            /// <summary>
            /// This method returns an integer value for the word given
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public int GetTextValue(string text)
            {
                // initial value
                int textValue = 0;

                // If the text string exists
                if (TextHelper.Exists(text))
                {
                    // iterate the characters
                    foreach (char c in text)
                    {
                        // add the value of this character
                        textValue += (int) c;
                    }
                }

                // return value
                return textValue;
            }
            #endregion

            #region GetViewText(string viewName)
            /// <summary>
            /// This method returns the text for a view
            /// </summary>
            /// <param name="viewName"></param>
            /// <returns></returns>
            public string GetViewText(string viewName)
            {
                // initial value
                string sql = "";

                try
                {
                    string sourceSQL = "Select definition, uses_ansi_nulls, uses_quoted_identifier, is_schema_bound FROM";
                    sourceSQL += " sys.sql_modules Where object_id = object_id('" + viewName + "');";

                    // Open The command Object
                    SqlCommand command = new SqlCommand(sourceSQL, DatabaseConnection);

                    // Create a Data adapter
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    // Create And Open Data
                    DataSet DS = new DataSet();

                    // Fill DataAdapter
                    adapter.Fill(DS, "Views");

                    sql = DS.Tables[0].Rows[0].ItemArray[0].ToString();
                }
                catch (Exception error)
                {
                    // for debugging only for now
                    DebugHelper.WriteDebugError("GetViewText", "SQLDatabaseConnector", error);
                }

                // return value
                return sql;
            }
            #endregion

            #region IsFieldAPrimaryKey(string fieldName, List<DataTable> depencies)
            /// <summary>
            /// This method return true if the field name given is a primary key
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="depencies"></param>
            /// <returns></returns>
            public bool IsFieldAPrimaryKey(string fieldName, List<DataTable> depencies)
            {
                // initial value
                bool isFieldAPrimaryKey = false;

                // if the fieldName exists
                if ((TextHelper.Exists(fieldName)) && (ListHelper.HasOneOrMoreItems(depencies)))
                {
                    // Iterate the collection of DataTable objects
                    foreach (DataTable table in depencies)
                    {
                        // iterate the collection of fields
                        if (ListHelper.HasOneOrMoreItems(table.ActiveFields))
                        {
                            // iterate the collection of fields
                            foreach (DataField field in table.ActiveFields)
                            {  
                                // if this is the field being sought 
                                if (TextHelper.IsEqual(fieldName, field.FieldName))
                                {
                                    // if this field is a primary key
                                    if (field.PrimaryKey)
                                    {
                                        // Set to true
                                        isFieldAPrimaryKey = true;
                                    }

                                    // break out of the loop
                                    break;
                                }
                            }
                        }
                    }
                }

                // return value
                return isFieldAPrimaryKey;
            }
            #endregion

			#region IsPrimaryKeyUpdateable(DataJuggler.Net.DataTable table)
			/// <summary>
			/// Determines if the table passed in has a primary key And If This 
			/// SqlDatabaseConnector Is In The State To Have Just Inserted A 
			/// Record. Also Captureidentity must be set to true.
			/// </summary>
			/// <param name="table"></param>
			/// <returns>True if all of the above is true; defaults to false</returns>
			public bool IsPrimaryKeyUpdateable(DataJuggler.Net.DataTable table)
			{
				// If This table Has A Primary Key & Captureidentity Is True & DBConnector Last Executed An Insert
                if ((table.HasPrimaryKey) && (this.Captureidentity) && (this.IsInsert) && (this.Lastidentity > 0))
				{
					// Return true;
					return true;
				}
				
				// If Any Of The Above Is Not True This Method Defaults To False
				return false;
				
			}
			#endregion

            #region LoadCheckConstraints(DataTable dataTable)
            /// <summary>
            /// This method loads the CheckConstraints
            /// </summary>
            /// <param name="dataTable"></param>
            /// <returns></returns>
            private List<CheckConstraint> LoadCheckConstraints(DataTable dataTable)
            {
                // initial value
                List<CheckConstraint> checkConstraints = new List<CheckConstraint>();

                // local
                CheckConstraint checkConstraint = null;

                try
                {
                    // if the dataTable exists
                    if (dataTable != null)
                    {
                        // create the sql to get all the CheckConstraints for this table
                        string sql = "SELECT TABLE_NAME, COLUMN_NAME, CHECK_CLAUSE, cc.CONSTRAINT_NAME FROM INFORMATION_SCHEMA.CHECK_CONSTRAINTS cc INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE c ON cc.CONSTRAINT_NAME = c.CONSTRAINT_NAME Where TABLE_NAME = '[TableName]' ORDER BY TABLE_NAME, COLUMN_NAME".Replace("[TableName]", dataTable.Name);

                        // Open The command Object
                        SqlCommand command = new SqlCommand(sql, DatabaseConnection);

                        // Create a Data adapter
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        // Create And Open Data
                        DataSet DS = new DataSet();

                        // Fill DataAdapter
                        adapter.Fill(DS, "tables");
                        
                        // if there is at least one row
                       if (DS.Tables["tables"].Rows.Count > 0)
                       {
                            // Load the CheckConstraints
                            foreach (System.Data.DataRow dataRow in DS.Tables["tables"].Rows)
                            {
                                // Create a new CheckConstraint object
                                checkConstraint = new CheckConstraint();

                                // map the properites in the dataRow to the new checkConstraint object
                                checkConstraint.TableName = (string) dataRow["Table_Name"];
                                checkConstraint.ColumnName = (string) dataRow["Column_Name"];
                                checkConstraint.CheckClause = (string) dataRow["Check_Clause"];
                                checkConstraint.ConstraintName = (string) dataRow["Constraint_Name"];

                                // add the new DataIndex to the indexes collection
                                checkConstraints.Add(checkConstraint);
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();

                    // Show the error to the output window if open.
                    DebugHelper.WriteDebugError("LoadCheckConstraints", "SQLDatabaseConnector", error);
                }

                // return value
                return checkConstraints;
            }
            #endregion
			
			#region LoadDatabaseData(Database database, List<string> ignoreTables = null)
			/// <summary>
			/// Connects To And Loads The Data For A SQL Server Database.
            /// Make sure the Database has a connectionstring property set when this method is called.
			/// </summary>
			public Database LoadDatabaseData(Database database, List<string> ignoreTables = null)
			{
				try
				{
					// Create New Connection Object
					DatabaseConnection = new SqlConnection(database.ConnectionString);
		
					// Open Connection To Database
					DatabaseConnection.Open();

					// LoadDataTablesData
					database.Tables = LoadDataTablesData(database.Tables, ignoreTables);
				}
				catch(System.Exception e)
				{
					// Set Error String
					string error = e.ToString();
				}
				finally
				{
					// Close the database connection if it is open
					if(DatabaseConnection.State != 0)
					{
						DatabaseConnection.Close();
					}
				}

				// Return Database
				return database;
			}
			#endregion
			
			#region LoadDatabaseSchema(Database database, bool ignoreDataSync = true, bool ignoreAzureFirewallRules = true)
			/// <summary>
			/// Loads The Database Schema For The Database
			/// </summary>
			/// <param name="database"></param>
			/// <param name="ConnectionString"></param>
			/// <returns></return>
			public Database LoadDatabaseSchema(Database database, bool ignoreDataSync = true, bool ignoreAzureFirewallRules = true)
			{
                // Update for version 5.5
                IgnoreDataSync = ignoreDataSync;
                IgnoreAzureFirewallRules = ignoreAzureFirewallRules;
				
				try
				{
					// If Database Is Not Open
					if (DatabaseConnection.State != System.Data.ConnectionState.Open)
					{
						// Set Failed Reason
						this.FailedReason = "The database connection is not open";
						
						// Return Database
						return database;
					}

                    // Update for version 2; the database name was not being loaded
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(DatabaseConnection.ConnectionString);
                    
                    // if the schema exists
                    if (NullHelper.Exists(builder))
                    {
                        // set the databaseName
                        database.Name = builder.InitialCatalog;
                    }

					// Load Database tables
                    List<DataTable> tables = LoadDataTablesSchema(database);
					
                    // Update for version 2: I found a faster way to load the fieldsSchema
                    // including the default value. This reduces the number of calls
                    // to load the database schema down to 1 from the number of tables
                    // or views in the database
                    if (ListHelper.HasOneOrMoreItems(tables))
                    {
                        // Load the fields schema for all the tables
                        LoadDataFieldsSchema(ref tables);
                    }

                    // Now set the tables
                    database.Tables = tables;
					
					// Load Stored Procedures
					database.StoredProcedures = LoadStoredProcedures(tables);

                    // Update for version 2.0: Now retrieving the ForeignKeys for a table
                    LoadForeignKeys(ref database);
                }
				catch(System.Exception e)
				{
					// Set Error String
					string error = e.ToString();
				}
				finally
				{
					// Close the database connection if it is open
					if(DatabaseConnection.State != 0)
					{
						DatabaseConnection.Close();
					}
				}

				// Return Database
				return database;
			}
			#endregion

            #region LoadDataFieldsSchema(ref List<DataTable> tables)
            /// <summary>
            /// Load the Fields for a dataTable passed in.
            /// </summary>
            /// <param name="dataTable">The table to create the fields for.</param>
            /// <returns></returns>
            private void LoadDataFieldsSchema(ref List<DataTable> tables)
            {
                // local;
                DataTable table = null;

                try
                {
                    // If the tables collection exists and has one or more items
                    if (ListHelper.HasOneOrMoreItems(tables))
                    {
                        // First we must load the IdentiyInformation so we can set IdentiyColumns correctly
                        List<IdentityInfo> identityColumns = LoadIdentityColumns();

                        // This value holds the name of the IdentityColumn for a table (if present)
                        IdentityInfo tableIdentityInfo = null;

                        // This is used to hold SchemaInformation about the current table and current field
                        List<DataField> schemaFields = null;
                        DataField schemaField = null;

                        // sql Statement To Select All tables
                        string sql = "SELECT TABLE_SCHEMA, Table_Name, COLUMN_NAME, ORDINAL_POSITION, COLUMN_DEFAULT, Is_Nullable, Data_Type, CHARACTER_MAXIMUM_LENGTH, Numeric_Precision, Numeric_Scale, DATETIME_PRECISION FROM INFORMATION_SCHEMA.COLUMNS";

                        // Open The command Object
                        SqlCommand command = new SqlCommand(sql, DatabaseConnection);

                        // Create a Data adapter
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        // Create And Open Data
                        DataSet DS = new DataSet();

                        // Fill DataAdapter
                        adapter.Fill(DS, "fields");

                        // Get the sourceTable to iterate
                        System.Data.DataTable sourceTable = DataHelper.ReturnFirstTable(DS);

                        // Loop Through table
                        foreach (System.Data.DataRow databaseField in sourceTable.Rows)
                        {
                            // Get the tableName
                            string tableName = databaseField["Table_Name"].ToString();

                            // if we do not have a table yet or if the table_Name is not the same
                            if ((NullHelper.IsNull(table)) || (table.Name != tableName))
                            {  
                                // set to null first just in case a table is not found
                                table = null;

                                // Attempt to find the table
                                table = tables.FirstOrDefault(x => x.Name == tableName);

                                // set to null
                                tableIdentityInfo = null;
                                schemaFields = null;
                                schemaField = null;

                                // if the table was found and there are one or more identityColumns
                                if ((NullHelper.Exists(table)) && (ListHelper.HasOneOrMoreItems(identityColumns)))
                                {
                                    // look up the IdentityInfo object for this table
                                    tableIdentityInfo = identityColumns.FirstOrDefault(x => x.TableName == table.Name);

                                    // Attempt to get the SchemaFields
                                    schemaFields = GetSchemaFields(table);
                                }
                            }

                            // If the table object exists
                            if (NullHelper.Exists(table))
                            {
                                // Set SchemaField to null for the new field
                                schemaField = null;

                                // Create New DataField
                                DataJuggler.Net.DataField field = new DataField();

                                // Set fieldName
                                field.FieldName = DataTable.CapitalizeFirstChar(databaseField["Column_Name"].ToString());

                                // Set the DBFieldName
                                field.DBFieldName = databaseField["Column_Name"].ToString();

                                // if there are one or more schemaFields
                                if (ListHelper.HasOneOrMoreItems(schemaFields))                            
                                {
                                    // set the schemaField
                                    schemaField = schemaFields.FirstOrDefault(x => x.FieldName == field.FieldName);
                                }

                                // If the schemaField object exists
                                if (NullHelper.Exists(schemaField))
                                {
                                    // set the value from the schemaField
                                    field.IsNullable = schemaField.IsNullable;
                                }

                                // Set Properties

                                // Set fieldOrdinal
                                field.FieldOrdinal = (int) databaseField["Ordinal_Position"];

                                // Set dataType
                                string dataType = databaseField["data_type"].ToString();

                                // Set the DBDatatype
                                field.DBDataType = dataType;

                                // if the ColumnDefault exists
                                string defaultValue = databaseField["Column_Default"].ToString();

                                // if the defaultValue exists
                                if (TextHelper.Exists(defaultValue))
                                {
                                    // set the defaultValue
                                    field.DefaultValue = defaultValue;
                                }

                                // If the tableIdentityInfo object exists
                                if (NullHelper.Exists(tableIdentityInfo))
                                {
                                    // see if this is auto increment
                                    field.IsAutoIncrement = TextHelper.IsEqual(field.DBFieldName, tableIdentityInfo.ColumnName);
                                }

                                // INFORMATION_SCHEMA returns the data type in lowercase, so compare case-insensitively
                                if ((TextHelper.IsEqual(dataType, "money")) || (TextHelper.IsEqual(dataType, "smallmoney")))
                                {
                                    // money has a fixed precision and scale, so none are stored
                                    field.DataType = DataManager.DataTypeEnum.Currency;
                                }
                                else if ((TextHelper.IsEqual(dataType, "decimal")) || (TextHelper.IsEqual(dataType, "numeric")))
                                {
                                    // set the dataType
                                    field.DataType = DataManager.DataTypeEnum.Decimal;

                                    // set the precision and scale (a missing value defaults to 0)
                                    field.Precision = NumericHelper.ParseInteger(databaseField["Numeric_Precision"].ToString(), 0, 0);
                                    field.Scale = NumericHelper.ParseInteger(databaseField["Numeric_Scale"].ToString(), 0, 0);
                                }
                                else if (TextHelper.IsEqual(dataType, "datetime2"))
                                {
                                    // set the dataType
                                    field.DataType = DataManager.DataTypeEnum.DateTime;

                                    // store the number of fractional second digits (the 0 in datetime2(0))
                                    field.Precision = NumericHelper.ParseInteger(databaseField["DATETIME_PRECISION"].ToString(), 0, 0);
                                }
                                else
                                {
                                    // Parse dataType
                                    field.DataType = ParseDataType(dataType, field);
                                }

                                // If the schemaField object exists
                                if (NullHelper.Exists(schemaField))
                                {
                                    // Set ColumnSize
                                    field.Size = schemaField.Size;

                                    

                                    // Set IsNullable
                                    field.IsReadOnly = schemaField.IsReadOnly;

                                    // Set the value for Primarykey
                                    field.PrimaryKey = schemaField.PrimaryKey;
                                }

                                // If this is a view
                                if (table.IsView)
                                {
                                    // For a view all fields are read write
                                    field.AccessMode = DataManager.AccessMode.ReadWrite;
                                }
                                else
                                {
                                    // Set Access Mode
                                    if (field.IsReadOnly)
                                    {
                                        // Set AccessMode
                                        field.AccessMode = DataManager.AccessMode.ReadOnly;
                                    }
                                    else
                                    {
                                        field.AccessMode = DataManager.AccessMode.ReadWrite;
                                    }
                                }

                                // Find the InsertIndex so the fields are inserted in alphabetical order
                                int insertIndex = FindInsertIndex(table.Fields, field);

                                // Add to Fields Collection
                                table.Fields.Insert(insertIndex, field);
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();

                    // Write the error to the debugger
                    DebugHelper.WriteDebugError("LoadDataFieldsSchema", "DataJuggler.Net.SQLDatabaseConnector", error);
                }
            }
            #endregion

            #region LoadDataIndexes(DataTable dataTable)
            /// <summary>
            /// This method loads the Indexes for the Table given. A multi-column index is
            /// loaded as ONE DataIndex with multiple entries in its Columns list, not as
            /// separate objects per column.
            /// </summary>
            /// <param name="dataTable"></param>
            /// <returns></returns>
            private List<DataIndex> LoadDataIndexes(DataTable dataTable)
            {
                // initial value
                List<DataIndex> indexes = new List<DataIndex>();

                // locals
                DataIndex index = null;
                DataIndex prevIndex = null;
                int indexId = 0;

                try
                {
                    // if the dataTable exists
                    if (dataTable != null)
                    {
                        // create the sql to get all the indexes and their columns for this table,
                        // ordered by index_id then key_ordinal, so every column belonging to the
                        // same index arrives together, and in the correct order
                        string sql = ("Select idx.object_id, idx.name, idx.index_id, idx.type, idx.type_desc, idx.is_unique, idx.data_space_id, idx.ignore_dup_key, idx.is_primary_key, idx.is_unique_constraint, idx.is_padded, idx.is_disabled, idx.is_hypothetical, idx.allow_row_locks, idx.allow_page_locks, idx.has_filter, idx.filter_definition, idx.fill_factor, " +
                                     "col.name AS FieldName, ic.is_descending_key, ic.is_included_column, ic.key_ordinal " +
                                     "From sys.indexes idx " +
                                     "Inner Join sys.index_columns ic On ic.object_id = idx.object_id And ic.index_id = idx.index_id " +
                                     "Inner Join sys.columns col On col.object_id = ic.object_id And col.column_id = ic.column_id " +
                                     "Where idx.object_id = (select object_id from sys.objects where name = '[TableName]') " +
                                     "Order By idx.index_id, ic.key_ordinal").Replace("[TableName]", dataTable.Name);

                        // Open The command Object
                        SqlCommand command = new SqlCommand(sql, DatabaseConnection);

                        // Create a Data adapter
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        // Create And Open Data
                        DataSet DS = new DataSet();

                        // Fill DataAdapter
                        adapter.Fill(DS, "tables");

                        // Load the Indexes
                        foreach (System.Data.DataRow dataRow in DS.Tables["tables"].Rows)
                        {
                            // the index_id this row belongs to
                            indexId = (int) dataRow["index_id"];

                            // if this row belongs to the same index as the row before it, add to it instead of creating a new one
                            if ((NullHelper.Exists(prevIndex)) && (prevIndex.IndexId == indexId))
                            {
                                // reuse the index already in progress
                                index = prevIndex;
                            }
                            else
                            {
                                // Create a new DataIndex object
                                index = new DataIndex();

                                // map the properites in the dataRow to the new DataIndex object
                                index.ObjectId = (int) dataRow["object_id"];
                                index.Name = (string) dataRow["Name"];
                                index.IndexId = indexId;
                                index.IndexType = ParseIndexType(dataRow["type"]);
                                index.TypeDescription = (string) dataRow["type_desc"];
                                index.IsUnique = (bool) dataRow["is_unique"];
                                index.DataSpaceId = (int) dataRow["data_space_id"];
                                index.IgnoreDuplicateKey = (bool) dataRow["ignore_dup_key"];
                                index.IsPrimary = (bool) dataRow["is_primary_key"];
                                index.IsUniqueConstraint = (bool) dataRow["is_unique_constraint"];
                                index.IsPadded = (bool) dataRow["is_padded"];
                                index.IsDisabled = (bool) dataRow["is_disabled"];
                                index.IsHypothetical = (bool) dataRow["is_hypothetical"];
                                index.AllowRowLocks = (bool) dataRow["allow_row_locks"];
                                index.AllowPageLocks = (bool) dataRow["allow_page_locks"];
                                index.HasFilter = (bool) dataRow["has_filter"];
                                index.FilterDefinition = dataRow["filter_definition"].ToString();

                                // FillFactor null causes an error
                                if (dataRow["fill_factor"] != null)
                                {
                                    // Set the FillFactor
                                    string temp = dataRow["fill_factor"].ToString();

                                    // parse out the string
                                    index.FillFactor = Int32.Parse(temp);
                                }
                                else
                                {
                                    // Set the FillFactor
                                    index.FillFactor = 0;
                                }

                                // add the new DataIndex to the indexes collection
                                indexes.Add(index);
                            }

                            // add this column to the index
                            // key_ordinal is tinyint (byte), so Convert instead of casting
                            index.AddColumn((string) dataRow["FieldName"], (bool) dataRow["is_descending_key"], (bool) dataRow["is_included_column"], Convert.ToInt32(dataRow["key_ordinal"]));

                            // remember this as the index currently in progress, for the next row
                            prevIndex = index;
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();

                    // Write the error to the output window if open
                    DebugHelper.WriteDebugError("LoadDataIndexes", "SQLDatabaseConnector", error);
                }

                // return value
                return indexes;
            }
            #endregion
			
			#region LoadDataRows() +1 overrides

				#region LoadDataRows(DataJuggler.Net.DataTable table)
				/// <summary>
				/// Loads All Rows For This table
				/// </summary>
				/// <param name="table"></param>
				/// <returns></returns>
				public List<DataRow> LoadDataRows(DataJuggler.Net.DataTable dataTable)
				{
					// String to hold sql Statement
                    string sql = BuildTableSQL(dataTable);

					// Call LoadDataRows
                    dataTable.Rows = LoadDataRows(dataTable, sql);
								
					// Return tables
                    return dataTable.Rows;
				}
				#endregion

				#region LoadDataRows(DataJuggler.Net.DataTable table, string sql)
				/// <summary>
				///  sql statement To Load DataRow
				/// </summary>
				/// <param name="table"></param>
				/// <param name="sql"></param>
				/// <returns></returns>
				public List<DataRow> LoadDataRows(DataJuggler.Net.DataTable dataTable, string sql)
				{	
					// Recreate Rows Collection
                    dataTable.Rows = new List<DataRow>();
		
					// Set Last sql
                    this.LastSQL = sql;

					// Open The command Object
					this.Command = new SqlCommand();

                    // Set the CommandText
                    this.Command.CommandType = CommandType.Text;

                    // Set the sql
                    this.Command.CommandText = sql;
                    
                    // Set the connection
                    this.Command.Connection = databaseconnection;
                    
					// If this table contains binary Data
					if(dataTable.ContainsBinaryData)
					{
					    // Load DataRows with Binary Data
                        dataTable.Rows = LoadDataRowsWithBinaryData(dataTable, sql);
			        }
			        else
			        {
			            // Load the Rows without binary Data
			            dataTable.Rows = LoadDataRowsWithoutBinaryData(dataTable);
                    }
                    			
					// Return tables
					return dataTable.Rows;
				}
				#endregion

                #region LoadDataRowsWithBinaryData(DataTable dataTable, string sql)
                /// <summary>
                /// This method loads the Data rows that contain binary Data.
                /// A SqlDataReader is used so that SequentialAccess can be set.
                /// </summary>
                /// <param name="dataTable"></param>
                private List<DataRow> LoadDataRowsWithBinaryData(DataTable dataTable, string sql)
                {   
                    // Recreate Rows
                    dataTable.Rows = new List<DataRow>();
                
                    // Create reader
                    SqlDataReader reader = null;
                    
                    // Create DataRow
                    DataRow dataRow = null;
                    
                    // local for a copy of the sql
                    string copySql = sql;
                    
                    try
                    {
                        // Create Reader
                        reader = this.Command.ExecuteReader(CommandBehavior.SequentialAccess);

                        // Read each record
                        while (reader.Read())
                        {   
                            // Create DataRow
                            dataRow = new DataRow();
                            
                            // Clone Fields
                            dataRow.Fields = SQLDatabaseConnector.CloneFields(dataTable.Fields);
                            
                            // reset sql
                            sql = copySql;
                            
                            // local for fieldName
                            int fieldIndex = 0;
                            
                            // local
                            int fieldOrdinal = -1;
                            
                            // Now set the value of each field
                            while (fieldIndex >= 0)
                            {
                                // reset
                                fieldIndex = -1;
                                
                                // increment fieldOrdinal
                                fieldOrdinal++;
                            
                                // Get the next field in sql
                                string fieldName = GetNextField(ref sql);
                                
                                // if the field index was found
                                if (!String.IsNullOrEmpty(fieldName))
                                {
                                    // get the fieldIndex
                                    fieldIndex = FindFieldIndex(dataRow.Fields, fieldName);
                                }
                                
                                // if the fieldIndex exists
                                if ((fieldIndex >= 0) && (dataRow.Fields != null) && (fieldIndex < dataRow.Fields.Count))
                                {
                                    // Remove the brackets
                                    fieldName = fieldName.Replace("[", "");
                                    fieldName = fieldName.Replace("]", "");
                                
                                    try
                                    {
                                        // Set fieldValue
                                        object fieldValue = null;
                                        
                                        // A Binary field must be read differently
                                        if(dataTable.Fields[fieldIndex].DataType == DataManager.DataTypeEnum.Binary)
                                        {
                                            //// Read all bytes
                                            System.Data.SqlTypes.SqlBinary fieldBinaryValue = reader.GetSqlBinary(fieldOrdinal);
                                            
                                            // set fieldValue
                                            fieldValue = fieldBinaryValue.Value;
                                        }
                                        else
                                        {
                                            // Set the value of each field
                                            fieldValue = reader[fieldOrdinal];   
                                        }
                                        
                                        // If this is a string and there is Data trim it up
                                        if ((dataTable.Fields[fieldIndex].DataType == DataManager.DataTypeEnum.String) && (fieldValue != null))
                                        {
                                            // Trim as a string
                                            dataRow.Fields[fieldIndex].FieldValue = fieldValue.ToString().Trim();
                                        }
                                        else
                                        {
                                            // set the field value as is
                                            dataRow.Fields[fieldIndex].FieldValue = fieldValue;
                                        }
                                    }
                                    catch (Exception error)
                                    {
                                        // raise the error up.
                                        string exception = error.ToString();
                                        Console.WriteLine(exception);
                                    }
                                }
                            }
                            
                            // Add this field
                            dataTable.Rows.Add(dataRow);
                        }
                    }
                    finally
                    {
                        // close the reader
                        reader.Close();
                    }
                    
                    // return value
                    return dataTable.Rows;
                }
                #endregion

                #region LoadDataRowsWithoutBinaryData(DataTable dataTable, string sql = "")
                /// <summary>
                /// This method loads the DataRows without Binary Data.
                /// </summary>
                /// <param name="dataTable"></param>
                /// <returns></returns>
                public List<DataRow> LoadDataRowsWithoutBinaryData(DataTable dataTable, string sql = "")
                {
                    // locals
                    int index = -1;

                    try
                    {
                        // Create DataSet
                        DataSet dataSet = new DataSet();

                        // Create dataRow
                        DataRow dataRow = new DataRow(dataTable);

                        // Create the adapter to read the fields
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(this.Command);

                        // if custom sql was passed in
                        if (TextHelper.Exists(sql))
                        {
                            // Create the adapter to read the fields
                            dataAdapter = new SqlDataAdapter(sql, this.DatabaseConnection);

                            // Fill DataAdapter
                            dataAdapter.Fill(dataSet, sql);
                        }
                        else
                        {
                            // Fill DataAdapter
                            dataAdapter.Fill(dataSet, dataTable.Name);
                        }
                    
                        // Loop through each row in the dataSet
                        foreach (System.Data.DataRow tempDataRow in dataSet.Tables[dataTable.Name].Rows)
                        {
                            // reset
                            index = -1;

                            // Create A New row
                            dataRow = new DataJuggler.Net.DataRow(dataTable);

                            // Clone Fields Collection For This row
                            dataRow.Fields = CloneFields(dataTable.ActiveFields);

                            // Add Each field
                            foreach (DataJuggler.Net.DataField field in dataTable.Fields)
                            {
                                // Increment the value for index
                                index++;

                                // Finding field Index
                                int fieldOrdinal = FindFieldOrdinalInDataRow(dataSet, dataTable.Name, field.FieldName, field.Caption);

                                // Verify fieldOrdinal Was Found
                                if (fieldOrdinal >= 0)
                                {
                                    // Set This field 
                                    dataRow.Fields[index].FieldValue = tempDataRow[fieldOrdinal];
                                }
                            }

                            // Add This row
                            dataTable.Rows.Add(dataRow);
                        }
                    }
                    catch (Exception error)
                    {
                        // for debugging only
                        string tableName = dataTable.Name;
                        string temp = sql;
                        
                        // Set the error
                        DebugHelper.WriteDebugError("LoadDataRowsWithoutBinaryData", "SQLDatabaseConnector", error);
                    }
                    
                    // return value
                    return dataTable.Rows;
                } 
                #endregion

            #endregion

            #region LoadDataTablesData(List<DataTable> tables, List<string> ignoreTables = null, ProgressStatusCallback callback = null)
            /// <summary>
            /// This methods loads the data rows for the list of tables, unless the table is in the ignoreTables list.
            /// </summary>
            /// <param name="tables">The list of DataTables to load data rows for.</param>
            /// <param name="ignoreTables">A list of table names to exclude from loading.</param>
            /// <param name="callback">A delegate to callback and report the current status.</param>
            /// <returns></returns>
            public List<DataTable> LoadDataTablesData(List<DataTable> tables, List<string> ignoreTables = null, ProgressStatusCallback callback = null)
			{
                // locals
                bool skipTable = false;
                int progress = 0;

                // If the tables collection exists and has one or more items
                if ((ListHelper.HasOneOrMoreItems(tables)) && (NullHelper.Exists(callback)))
                {
                     // Setup the graph again
                     callback(tables.Count * 2, 0, "Loading data rows", tables.Count, 0, "Begin iterating table collection");
                }

				// Iterate Through tables Collection
				for(int x = 0;x < tables.Count;x++)
				{
                    // reset
                    skipTable = false;

                    // If the ignoreTables collection exists and has one or more items
                    if (ListHelper.HasOneOrMoreItems(ignoreTables))
                    {
                        // Iterate the collection of string objects
                        foreach (string tableName in ignoreTables)
                        {
                            // if this table is found
                            if (TextHelper.IsEqual(tableName, tables[x].Name))
                            {
                                // abort
                                skipTable = true;

                                // break out of the loop
                                break;
                            }
                        }
                    }

                    // if the value for skipTable is false
                    if (!skipTable)
                    {
                        // If the callback object exists
                        if (NullHelper.Exists(callback))
                        {
                            // notify the caller the current status
                            callback(tables.Count * 2, progress, "Loading data rows for table " + tables[x].Name, tables.Count, progress, "Preparing to load");
                        }

					    // Load Rows For This table
					    tables[x].Rows = LoadDataRows(tables[x]);

                        // If the callback object exists
                        if (NullHelper.Exists(callback))
                        {
                            // notify the caller the current status
                            callback(tables.Count * 2, progress + 1, "Loading data rows complete for table " + tables[x].Name, tables.Count, progress + 1, "Load complete");
                        }
                    }
                    else
                    {
                        // notify the caller the current status
                        callback(tables.Count * 2, progress, "Skipping table " + tables[x].Name, tables.Count, progress, "Table was in the ignore list");
                    }

                    // Increment the value for progress
                    progress++;
				}
						
				// Return tables
				return tables;
			}
			#endregion

            #region LoadDataTablesSchema(Database parentDatabase)
            public List<DataTable> LoadDataTablesSchema(Database parentDatabase)
			{
				
				// Create tables Object
				List<DataTable> tables = new List<DataTable>();
				
				// Local for trial version
				int tableCount = 0;
				
				try
				{				
					// sql Statement To Select All tables
					string sql = "Select Name, xType from SysObjects where (xType = 'U' Or xType = 'V') and Name <> 'dtProperties' Order By Name";

					// Open The command Object
                    this.Command = new SqlCommand(sql, DatabaseConnection);
					SqlDataAdapter Adapter = new SqlDataAdapter(this.Command);

					// Create And Open Data
					DataSet DS = new DataSet();
					
					// Fill DataAdapter
					Adapter.Fill(DS,"tables");

					// itereate the Rows
					foreach(System.Data.DataRow dr in DS.Tables["tables"].Rows)
					{	
						// Create A New table
						DataJuggler.Net.DataTable dataTable = new DataJuggler.Net.DataTable();
                        
                        // set the name
                        dataTable.Name = dr[0].ToString();

                        // set xType
                        string xType = dr["xType"].ToString().Trim();

                        // Is this not a system diagram
                        bool addTable = (dataTable.Name != "sysdiagrams");

                        // if this is a View
                        if (xType == "V")
                        {
                            // Set IsView to true
                            dataTable.IsView = true;

                            // if IgnoreAzureFirewallRules is true
                            if (IgnoreAzureFirewallRules)
                            {
                                // if this is the view for Azure Firewall Rules
                                if (dataTable.Name == "database_firewall_rules")
                                {
                                    // do not add this table
                                    addTable = false;
                                }
                            }
                        }
                        else
                        {
                            // Default to table
                            dataTable.IsView = false;
                        }

                        // if IgnoreDataSync is true
                        if (IgnoreDataSync)
                        {
                            // if this is a DataSync table
                            if (dataTable.SchemaName == "DataSync")
                            {
                                // do not add this table
                                addTable = false;
                            }
                            else if (dataTable.Name.ToLower().Contains("_dss"))
                            {
                                // do not add this table
                                addTable = false;
                            }
                        }
                        
						// If the table should be added (Test Again)
						if (addTable)
						{
                            // Attempt to get the sqlObject
                            SqlObject sqlObject = GetSqlObject(dataTable.Name);

                            // If the sqlObject object exists
                            if (NullHelper.Exists(sqlObject))
                            {
                                // This is used by DB Compare to create the correct Views or Stored Procedures
                                // In the GenerateScriptsButton_Click event.
                                dataTable.UsesAnsiNulls = sqlObject.UsesAnsiNulls;
                                dataTable.UsesQuotedIdentifier = sqlObject.UsesQuotedIdentifier;

                                // if this is a view
                                if (dataTable.IsView)
                                {
                                    // Set the ViewText so we have it for generating scripts
                                    dataTable.ViewText = sqlObject.Defintion;
                                }
                            }

							// Increment TableCount
							tableCount++;
						
                            // Set parentDatabase
						    dataTable.ParentDatabase = parentDatabase;

                            // Update 3.26.2016: The SchemaName must be loaded to work with AdventureWorks
                            dataTable.SchemaName = FindSchemaName(dataTable.Name);
						 
                            // Update for version 5.0: Only load indexes and check constraints for Tables, Views do not have them
                            if (!dataTable.IsView)
                            {
                                // load the indexes
                                dataTable.Indexes = LoadDataIndexes(dataTable);
                                
                                // load the Check Constraints
                                dataTable.CheckConstraints = LoadCheckConstraints(dataTable);

                                // Update 8.4.2022 load the default value Constraints
                                dataTable.DefaultValueConstraints = LoadDefaultValueConstraints(dataTable);
                            }

						    // Add This table
						    tables.Add(dataTable);
			            }
					}
                }
				catch(Exception Error)
				{
					// Set FailedReason
					this.FailedReason = Error.ToString();
				}

				// Return tables
				return tables;
			}
            #endregion

            #region LoadDataTableSchema(Database parentDatabase, string tableName)
            public DataJuggler.Net.DataTable LoadDataTableSchema(Database parentDatabase, string tableName)
            {
                // initial value
                DataTable dataTable = null;

                try
                {
                    // sql Statement To Select All tables
                    string sql = "Select Name, xType from SysObjects where (xType = 'U' Or xType = 'V') and Name = '[TABLENAME]' Order By Name";

                    // replace out the tableName
                    sql = sql.Replace("[TABLENAME]", tableName);

                    // Open The command Object
                    this.Command = new SqlCommand(sql, DatabaseConnection);
                    SqlDataAdapter Adapter = new SqlDataAdapter(this.Command);

                    // Create And Open Data
                    DataSet DS = new DataSet();

                    // Fill DataAdapter
                    Adapter.Fill(DS, "tables");

                    // itereate the Rows
                    foreach (System.Data.DataRow dr in DS.Tables["tables"].Rows)
                    {
                        // Create A New table
                        DataJuggler.Net.DataTable dt = new DataJuggler.Net.DataTable();

                        // set the table name
                        string temp = dr[0].ToString();

                        // set the name
                        dt.Name = dr[0].ToString();

                        // set xType
                        string xType = dr["xType"].ToString().Trim();

                        // Is this a 'table'
                        bool addTable = (dt.Name != "sysdiagrams");

                        // if this is a View
                        if (xType == "V")
                        {
                            // Set IsView to true
                            dt.IsView = true;

                            // if IgnoreAzureFirewallRules is true
                            if (IgnoreAzureFirewallRules)
                            {
                                // if this is the view for Azure Firewall Rules
                                if (dt.Name == "database_firewall_rules")
                                {
                                    // do not add this table
                                    addTable = false;
                                }
                            }
                        }
                        else
                        {
                            // Default to table
                            dt.IsView = false;
                        }

                        // If the table should be added (Test Again)
                        if (addTable)
                        {
                            // Set parentDatabase
                            dt.ParentDatabase = parentDatabase;
                            
                            // set the return value
                            dataTable = dt;
                        }
                    }
                }
                catch (Exception Error)
                {
                    // Set FailedReason
                    this.FailedReason = Error.ToString();
                }

                // Return tables
                return dataTable;
            }
            #endregion

            #region LoadAllForeignKeys()
            /// <summary>
            /// This method is used to load all foreign key constraints for the database open by the DataConnector.
            /// A composite (multi-column) foreign key is loaded as ONE ForeignKeyConstraint with multiple
            /// entries in its Columns list, not as separate objects that happen to share the same Name.
            /// </summary>
            /// <returns></returns>
            public List<ForeignKeyConstraint> LoadAllForeignKeys()
            {
                // initial value
                List<ForeignKeyConstraint> allForeignKeys = new List<ForeignKeyConstraint>();

                // locals
                string constraintName = "";
                string tableName = "";
                string columnName = "";
                string referencedTableName = "";
                string referencedColumnName = "";
                int ordinal = 0;
                ForeignKeyConstraint foreignKeyConstraint = null;
                ForeignKeyConstraint prevForeignKeyConstraint = null;

                string sql = "SELECT obj.name AS FK_NAME, sch.name AS [schema_name], tab1.name AS [Table], col1.name AS [Column], tab2.name AS [Referenced_Table], col2.name AS [Referenced_Column], fkc.constraint_column_id AS [Ordinal], " +
                        "fk.delete_referential_action_desc AS [OnDelete], fk.update_referential_action_desc AS [OnUpdate], fk.is_disabled AS [IsDisabled] " +
                        "FROM sys.foreign_key_columns fkc " +
                        "INNER JOIN sys.foreign_keys fk ON fk.object_id = fkc.constraint_object_id " +
                        "INNER JOIN sys.objects obj ON obj.object_id = fkc.constraint_object_id " +
                        "INNER JOIN sys.tables tab1 ON tab1.object_id = fkc.parent_object_id " +
                        "INNER JOIN sys.schemas sch ON tab1.schema_id = sch.schema_id " +
                        "INNER JOIN sys.columns col1 ON col1.column_id = fkc.parent_column_id AND col1.object_id = tab1.object_id " +
                        "INNER JOIN sys.tables tab2 ON tab2.object_id = fkc.referenced_object_id " +
                        "INNER JOIN sys.columns col2 ON col2.column_id = fkc.referenced_column_id AND col2.object_id = tab2.object_id " +
                        "Order By obj.name, fkc.constraint_column_id";

                // Create a SqlCommand
                SqlCommand command = new SqlCommand(sql, DatabaseConnection);

                // Fill DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                // Create a DataSet to hold the results
                DataSet ds = new DataSet();

                // Fill the Adapter
                adapter.Fill(ds, "ForeignKeys");

                // Load the identityColumns
                System.Data.DataTable foreignKeysTable = DataHelper.ReturnFirstTable(ds);

                // iterate the rows
                foreach (System.Data.DataRow row in foreignKeysTable.Rows)
                {
                    // set the value for constraintName
                    constraintName = (string) row["FK_NAME"];

                    // set the value for TableName
                    tableName = (string) row["Table"];

                    // Set the value for columnName
                    columnName = (string) row["Column"];

                    // set the value for referencedTableName
                    referencedTableName = (string) row["Referenced_Table"];

                    // set the value for referencedColumnName
                    referencedColumnName = (string) row["Referenced_Column"];

                    // set the value for ordinal
                    ordinal = (int) row["Ordinal"];

                    if (TextHelper.Exists(constraintName, tableName, columnName, referencedTableName, referencedColumnName))
                    {
                        // if this row belongs to the same constraint as the row before it, add to it instead of creating a new one
                        if ((NullHelper.Exists(prevForeignKeyConstraint)) && (TextHelper.IsEqual(constraintName, prevForeignKeyConstraint.Name)))
                        {
                            // reuse the constraint already in progress
                            foreignKeyConstraint = prevForeignKeyConstraint;
                        }
                        else
                        {
                            // Attempt to create a ForeignKeyConstraint
                            foreignKeyConstraint = new ForeignKeyConstraint(constraintName, tableName, referencedTableName);

                            // cascade rules and enabled state (constraint-level, so only set once)
                            foreignKeyConstraint.OnDelete = row["OnDelete"].ToString();
                            foreignKeyConstraint.OnUpdate = row["OnUpdate"].ToString();
                            foreignKeyConstraint.IsDisabled = (bool) row["IsDisabled"];

                            // add this foreignKeyConstraint to the collection of all foreign key constraints
                            allForeignKeys.Add(foreignKeyConstraint);
                        }

                        // add this column pair to the constraint (fills FieldName / ReferencedColumn too, when this is column 1)
                        foreignKeyConstraint.AddColumn(columnName, referencedColumnName, ordinal);

                        // remember this as the constraint currently in progress, for the next row
                        prevForeignKeyConstraint = foreignKeyConstraint;
                    }
                }

                // return value
                return allForeignKeys;
            }
            #endregion

            #region LoadDefaultValueConstraints(DataTable dataTable)
            /// <summary>
            /// returns a list of Default Value Constraints. Every default is loaded with its raw Definition
            /// (numeric, date, guid and text defaults); DefaultValue is also set when the definition is a number.
            /// </summary>
            public List<DefaultValueConstraint> LoadDefaultValueConstraints(DataTable dataTable)
            {
                // initial value
                List<DefaultValueConstraint> defaultValueConstraints = new List<DefaultValueConstraint>();

                // local
                DefaultValueConstraint defaultValueConstraint = null;
                string definition = "";
                string numericValue = "";

                try
                {
                    // if the dataTable exists
                    if (dataTable != null)
                    {
                        // create the sql to get all the DefaultValueConstraints for this table
                        string sql = "Select con.[name] as Constraint_Name, Schema_Name(t.schema_id) + '.' + t.[name]  as [Table], col.[name] as Column_Name, con.[definition] from sys.default_constraints con left outer join sys.objects t on con.parent_object_id = t.object_id left outer join sys.all_columns col on con.parent_column_id = col.column_id and con.parent_object_id = col.object_id Where t.name = '[TableName]' order by con.name".Replace("[TableName]", dataTable.Name);

                        // Open The command Object
                        SqlCommand command = new SqlCommand(sql, DatabaseConnection);

                        // Create a Data adapter
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        // Create And Open Data
                        DataSet DS = new DataSet();

                        // Fill DataAdapter
                        adapter.Fill(DS, "tables");

                        // if there is at least one row
                        if (DS.Tables["tables"].Rows.Count > 0)
                        {
                            // Load the DefaultValueConstraints
                            foreach (System.Data.DataRow dataRow in DS.Tables["tables"].Rows)
                            {
                                // Create a new DefaultValueConstraint object
                                defaultValueConstraint = new DefaultValueConstraint();

                                // map the properites in the dataRow to the new defaultValueConstraint object
                                defaultValueConstraint.TableName = (string) dataRow["Table"];
                                defaultValueConstraint.ColumnName = (string) dataRow["Column_Name"];
                                defaultValueConstraint.ConstraintName = (string) dataRow["Constraint_Name"];

                                // get the raw definition, e.g. ((1)), (getdate()), (newid()), (N'Pending')
                                definition = (string) dataRow["definition"];

                                // store the raw definition, used to compare and to script the default
                                defaultValueConstraint.Definition = definition;

                                // if the string exists
                                if (TextHelper.Exists(definition))
                                {
                                    // remove any parens
                                    numericValue = definition.Replace("(", "").Replace(")", "");

                                    // Parse the DefaultValue (only numeric defaults parse, everything else uses the Definition)
                                    defaultValueConstraint.DefaultValue = NumericHelper.ParseDouble(numericValue, -1090, -1091);
                                }

                                // add every default, numeric or not
                                defaultValueConstraints.Add(defaultValueConstraint);
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();

                    // Show the error to the output window if open.
                    DebugHelper.WriteDebugError("LoadDefaultValueConstraints", "SQLDatabaseConnector", error);
                }

                // return value
                return defaultValueConstraints;
            }
            #endregion
            
            #region LoadForeignKeys(ref Database database)
            /// <summary>
            /// This method is used to load the foreign keys for each table in the database given.
            /// </summary>
            /// <param name="database"></param>
            public void LoadForeignKeys(ref Database database)
            {
                try
                {
                    // If the database object exists
                    if (NullHelper.Exists(database))
                    {      
                        // create a temporary list of all foreign keys to hold just the table name and the foreign key name
                        List<ForeignKeyConstraint> allForeignKeys = LoadAllForeignKeys();

                        // if there are one or more foreignKeys and one or more tables
                        if ((ListHelper.HasOneOrMoreItems(allForeignKeys)) && (ListHelper.HasOneOrMoreItems(database.Tables)))
                        {
                            // iterate the tables in the database
                            foreach (DataTable table in database.Tables)
                            {
                                // Find all the foreign keys for this table
                                table.ForeignKeys = ForeignKeyConstraintHelper.FindForeignKeysForTable(table, allForeignKeys);
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();
                }
            }
            #endregion

            #region LoadIdentityColumns()
            /// <summary>
            /// This method is used to load a list of IdentityInfo objects
            /// </summary>
            /// <returns></returns>
            public List<IdentityInfo> LoadIdentityColumns()
            {
                // initial value
                List<IdentityInfo> identityColumns = new List<IdentityInfo>();

                // locals
                string tableName = "";
                string columnName = "";
                IdentityInfo identityInfo = null;
                
                try
                {
                    // Set the sql to be used
                    string sql = "select o.name As TableName, c.name As ColumnName from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 Order by TableName"; 

                     // Create And Open Data
					DataSet ds = new DataSet();

                    // Create a new command
                    SqlCommand command = new SqlCommand(sql, DatabaseConnection);
						
					// Fill DataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    // Fill the Adapter
                    adapter.Fill(ds,"IdentityColumns");

                    // Load the identityColumns
                    System.Data.DataTable identityDataTable = DataHelper.ReturnFirstTable(ds);

                    // iterate the rows
                    foreach (System.Data.DataRow row in identityDataTable.Rows)
                    {
                        // set the value for TableName
                        tableName = (string) row[0];

                        // Set the value for tableName
                        columnName= (string) row[1];

                        // If the strings tableName and columnName both exist
                        if (TextHelper.Exists(tableName, columnName))
                        {
                            // Create a new instance of an 'IdentityInfo' object.
                            identityInfo = new IdentityInfo(columnName, tableName);

                            // Add this item
                            identityColumns.Add(identityInfo);
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();

                    // write the problem to the console
                    DebugHelper.WriteDebugError("LoadIdentityColumns", "SQLDatabaseConnector", error);
                }

                // return value
                return identityColumns;
            }
            #endregion
			
 			#region LoadStoredProcedures(List<DataTable> tables)
            /// <summary>
            /// This method loads the stored procedures and their parameters and their return set schema.
            /// There is a breaking change from 1.72 version of DB Compare as now the tables
            /// collection has to be passed in to help retreive the ReturnSetSchema.
            /// </summary>
            /// <param name="tables"></param>
            /// <returns></returns>
			public List<StoredProcedure> LoadStoredProcedures(List<DataTable> tables)
			{
				// Create New Stored Procedures Collection
				List<StoredProcedure> procs = new List<StoredProcedure>();
				
				// If Database Is Connected
				string sql = "Select * From SysObjects Where Type = 'P' Order By Name";

                // local
                bool skip = false;
				
				try
				{
					// Open The command Object
                    this.Command = new SqlCommand(sql, DatabaseConnection);
					SqlDataAdapter adapter = new SqlDataAdapter(this.Command);

					// Create And Open Data
					DataSet ds = new DataSet();
						
					// Fill DataAdapter
					adapter.Fill(ds,"Procedures");

                    // itereate the Rows
					foreach(System.Data.DataRow dr in ds.Tables["Procedures"].Rows)
					{
                        // reset
                        skip = false;

						// Create A New Stored Procedures
						DataJuggler.Net.StoredProcedure sp = new StoredProcedure();
							
						// Set table Name
						sp.ProcedureName = dr["Name"].ToString();
						
						// Set Parameters
						sp = LoadStoredProcedureParameters(sp);

                        // if the procedure name contains dss
                        if (IgnoreDataSync)
                        {
                            // if this procedure contains a Data sync type name
                            if (sp.ProcedureName.Contains("_dss"))
                            {
                                // we need to skip this procedure
                                skip = true;
                            }
                        }

                        // if skip equals false
                        if (!skip)
                        {
						    // Now Add This Stored procedure To procs Collection
						    procs.Add(sp);
                        }
					}
			        
                    // if there are one or more stored procedures
                    if (ListHelper.HasOneOrMoreItems(procs))
                    {
                        // iterate the storedProcedures
                        foreach (StoredProcedure storedProcedure in procs)
                        {
                            // Update for version 2; now that the ReturnSetSchema is being set,
                            // I need to look up the table(s) involved and attempt to set
                            // The PrimaryKey - this isn't going to be perfect, but it works for 
                            // comparisons sake. 
                            List<DataTable> dependantTables = FindDepenciesForStoredProcedure(storedProcedure.ProcedureName, tables);

                            // If the dependantTables collection exists and has one or more items
                            if (ListHelper.HasOneOrMoreItems(dependantTables))
                            {
                                // load a tempStoredProcedure
                                StoredProcedure tempStoredProcedure = LoadStoredProcedureWithSchema(storedProcedure, dependantTables);

                                // If the ReturnSetSchema exists
                                if (tempStoredProcedure.ReturnSetSchema != null)
                                {
                                    // Set the ReturnSetSchema
                                    storedProcedure.ReturnSetSchema = tempStoredProcedure.ReturnSetSchema;
                                }
                            }

                            // Attempt to find the text for this stored procedure
                            storedProcedure.Text = GetProcedureText(storedProcedure.ProcedureName);

                            // Get the AnsiNull info
                            SqlObject sqlObject = GetSqlObject(storedProcedure.ProcedureName);

                            // If the sqlObject object exists
                            if (NullHelper.Exists(sqlObject))
                            {
                                // Set these values
                                storedProcedure.UsesAnsiNulls = sqlObject.UsesAnsiNulls;
                                storedProcedure.UsesQuotedIdentifier = sqlObject.UsesQuotedIdentifier;
                            }
                        }
                    }

				}
				catch(Exception error)
				{
					// Set Failed Reason
                    this.FailedReason = error.ToString();
				}
				
				// Return procs
				return procs;
			}
			#endregion
			
			#region LoadStoredProcedureParameters(StoredProcedure procedure)
            /// <summary>
            /// This method returns a the StoredProcedure given with the parameters loaded
            /// </summary>
            /// <param name="procedure"></param>
            /// <returns></returns>
			public StoredProcedure LoadStoredProcedureParameters(StoredProcedure procedure)
			{
				// Create the Parameters 
				procedure.Parameters = new List<StoredProcedureParameter>();
				
				try
				{
				
					// Open The command Object
					this.Command = new SqlCommand("sp_helptext",this.DatabaseConnection);
					this.Command.CommandType = System.Data.CommandType.StoredProcedure;
					
					// Add Parameter
					this.Command.Parameters.AddWithValue("@objname",procedure.ProcedureName);
					
					// Create DataAdapter
					SqlDataAdapter adapter = new SqlDataAdapter(this.Command);

					// Create And Open Data
					DataSet dataSet = new DataSet();
						
					// Fill DataAdapter
					adapter.Fill(dataSet,"Results");

					// Create StringBuilder
					StringBuilder sb = new StringBuilder();

                    // itereate the Rows
					foreach(System.Data.DataRow dataRow in dataSet.Tables["Results"].Rows)
					{		
						// Get Parameter Name
						 string text = dataRow["Text"].ToString();
						 
						// Append To String Builder
						sb.Append(text);
					}
					
					// Get fullProcedureText
					string fullProcedureText = sb.ToString();
					
					// Set StoredProcedureType
					procedure.StoredProcedureType = SetStoredProcedureType(fullProcedureText);
					
					// Set To Lower
					fullProcedureText = fullProcedureText.ToLower();
					
					// Remove Any Formatting Characters 
					fullProcedureText = fullProcedureText.Replace("\r"," ");
					fullProcedureText = fullProcedureText.Replace("\n"," ");
					fullProcedureText = fullProcedureText.Replace("\t"," ");
					
					// Now We Have The Full Text Of The Stored procedure; Parse Out Parameters
					int index = fullProcedureText.IndexOf(procedure.ProcedureName.ToLower());
					
					// Get First SpaceChar After Index
					int spaceChar = fullProcedureText.IndexOf(" ", index);
					
					// Now Get Index Of " As "
					int end = fullProcedureText.IndexOf(" as ");
					
					// Set Length 
					int len = end - spaceChar;
					
					// Now Build Some String That Contains All Parameter Information
					string paramString = fullProcedureText.Substring(spaceChar, len);
					
					// Trim ParamString
					paramString = paramString.Trim();
					
					// String For dataType
					string dataType = "";
					
					// Start Index
					int startIndex = 0;
					
					do
					{
						
						// Set Index
						index = paramString.IndexOf("@",startIndex);
						
						// Set Start Index
						startIndex = index +1;
						
						// If Index Was Found
						if(index >= 0)
						{
							// Get Space Char
							spaceChar = paramString.IndexOf(" ",index);
							
							// If SpaceChar Was Found
							if(spaceChar >= 0)
							{
								// Set Len
								len = spaceChar - index;
								
								// Set ParamName
								string paramName = paramString.Substring(index,len);
								
								// Set Index To SpaceChar + 1
								index = spaceChar +1;
								
								// Get SpaceChar Again
								spaceChar = paramString.IndexOf(" ", spaceChar);
								
								// Get Comma Char
								int commaChar = paramString.IndexOf(",", spaceChar);
								
								// If CommaChar Was Found
								if(commaChar > 0)
								{
									// Set Len
									len = commaChar - spaceChar;
									
									// Set dataType
									dataType = paramString.Substring(spaceChar, len);
								}
								else
								{
									// Now Get dataType
									dataType = paramString.Substring(spaceChar);
								}
								
								// Trim dataType
								dataType = dataType.Trim();
								
								// Now Create Stored procedure Parameter
								StoredProcedureParameter param = new StoredProcedureParameter();
								
								// Set Parameter Properties
								param.ParameterName = paramName;
								param.DataType = ParseSqlDataType(dataType); 
								
								// Now Add To Parameters Collection
								procedure.Parameters.Add(param);
							}
						}
						else
						{
							break;
						}
						
					}while(true);
					
                    // if there are not any Parameters
				    if (!ListHelper.HasOneOrMoreItems(procedure.Parameters))
                    {
                        // This procedure does not have parameters
                        procedure.DoesNotHaveParameters = true;
                    }
				}
				catch(Exception E)
				{
					// Set Failed Reason
					this.FailedReason = E.ToString();
				}
				
				// Return procs
				return procedure;
			}
			#endregion

            #region LoadStoredProcedureParametersEx(StoredProcedure procedure)
            /// <summary>
            /// This method does the same thing as the method above but this method querries the 
            /// database to get the values.
            /// </summary>
            /// <param name="procedure"></param>
            /// <returns></returns>
            public StoredProcedure LoadStoredProcedureParametersEx(StoredProcedure procedure)
            {
                try
                {
                    if (procedure != null)
                    {
                        // Create the Parameters 
                        procedure.Parameters = new List<StoredProcedureParameter>();

                        // Create a sql statement to load the parameters
                        string sql = "Select s.id , s.name, t.name as [type], t.length from  syscolumns s inner join systypes t on s.xtype = t.xtype where id = (select id from sysobjects where name = '[PROCEDURENAME]')";

                        // set the procedure name in the commandText
                        sql = sql.Replace("[PROCEDURENAME]", procedure.ProcedureName);

                        // Create DataAdapter
                        DataSet dataSet = this.ExecuteSql(sql);

                        // return the first table
                        System.Data.DataTable dataTable = DataHelper.ReturnFirstTable(dataSet);

                        // iterate each dataRow which is itself a parameter
                        foreach (System.Data.DataRow dataRow in dataTable.Rows)
                        {
                            // create a parameter
                            StoredProcedureParameter parameter = new StoredProcedureParameter();

                            // set the ParameterName
                            parameter.ParameterName = dataRow["Name"].ToString();
                        
                            // set the dataType
                            string dataTypeString = dataRow["Type"].ToString();

                            // faking this out
                            DataField field = new DataField();
                            field.FieldName = parameter.ParameterName;

                            // Get the data type
                            parameter.DataType = ParseDataType(dataTypeString, field);

                            // set the Length (could be needed)
                            string lengthString = dataRow["Length"].ToString();
                            parameter.Length = Convert.ToInt32(lengthString);
                        
                            // add this parameter to the return value
                            procedure.Parameters.Add(parameter);
                        }
                    }
                }
                catch (Exception E)
                {
                    // Set Failed Reason
                    this.FailedReason = E.ToString();
                }

                // Return procs
                return procedure;
            }
            #endregion

            #region LoadStoredProcedureWithSchema(StoredProcedure sourceStoredProcedure, List<DataTable> dependencies)
            /// <summary>
            /// This method loads the Schema returned from a stored procedure
            /// </summary>
            /// <param name="procedureName"></param>
            public StoredProcedure LoadStoredProcedureWithSchema(StoredProcedure sourceStoredProcedure, List<DataTable> dependencies)
            {
                // initial value
                StoredProcedure storedProcedure = new StoredProcedure();

                // local
                int dependentPrimaryKeysCount = FindPrimaryKeysCount(dependencies);
                int primaryKeysFound = 0;
                
                try
                {
                    // if the sourceStoredProcedure
                    if (NullHelper.Exists(sourceStoredProcedure))
                    {  
                        // create the ReturnSetSchema
                        storedProcedure.ReturnSetSchema = new List<DataField>();

                        // Create The command Object
                        // Update for version 2.0; I found a better way to get the ReturnSetSchema
                        // This method works even if there are not any rows in the table,
                        // Which many times on a Test or Dev server there will not be any
                        // Data, but I still need to know if the databases are in sync
                        string sqlText = "SELECT * FROM sys.dm_exec_describe_first_result_set ('[ProcedureName]', NULL, 0);".Replace("[ProcedureName]", sourceStoredProcedure.ProcedureName);
                                                    
                        // Create the Command
                        SqlCommand command = new SqlCommand(sqlText, DatabaseConnection);
                        command.CommandType = CommandType.Text;
                     
                        // Create And Open Data
					    DataSet ds = new DataSet();
						
					    // Fill DataAdapter
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        // Fill the Adapter
                        adapter.Fill(ds,"Fields");

                        // Get the sourceTable
                        System.Data.DataTable sourceTable = ds.Tables["Fields"];
                        
                         // iterate the fields
                        foreach (System.Data.DataRow databaseField in sourceTable.Rows)
                        {
                            // Check for an ErrorMessage
                            string error = databaseField["error_message"].ToString();

                            // if the error exists
                            if (TextHelper.Exists(error))
                            {
                                // Create a tempDataField object
                                DataField errorField = new DataField();

                                // Set the FieldName
                                errorField.FieldName = "Error: " + error;

                                // Add to Fields Collection
                                storedProcedure.ReturnSetSchema.Add(errorField);

                                // break out of this loop
                                break;
                            }
                            else
                            {
                                // Create New DataField
                                DataJuggler.Net.DataField field = new DataField();

                                // Set fieldName
                                field.FieldName = DataTable.CapitalizeFirstChar(databaseField["Name"].ToString());
                                field.DBFieldName = databaseField["Name"].ToString();

                                // check if this field is hidden, if yes do not include it
                                string rawValue = databaseField["Is_Hidden"].ToString();
                                bool hidden = BooleanHelper.ParseBoolean(rawValue, true, true);

                                // Set IsNullable
                                field.IsNullable = (bool)databaseField["is_nullable"];
                            
                                // if the field is not hidden 
                                // solves problem of aliased fields appearing in the select statements
                                if (!hidden)
                                {
                                    // Set Properties

                                    // Set fieldOrdinal
                                    field.FieldOrdinal = (int)databaseField["Column_Ordinal"];
                                
                                    // Set dataType
                                    string dataType = databaseField["system_type_name"].ToString();

                                    // set the DBDagtaType
                                    field.DBDataType = dataType;
                                
                                    // if this is a decimal
                                    if (dataType == "System.Decimal")
                                    {
                                        // Attempt to get the Precision & Scale
                                        try
                                        {
                                            // attemp to get the money string
                                            string sqlDataType = databaseField[23].ToString();

                                            // if the money string exists
                                            if (sqlDataType == "System.Data.SqlTypes.SqlMoney")
                                            {
                                                // set the datatype
                                                field.DataType = DataManager.DataTypeEnum.Currency;
                                            }
                                            else if (sqlDataType == "System.Data.SqlTypes.SqlDecimal")
                                            {
                                                // set the dataType
                                                field.DataType = DataManager.DataTypeEnum.Decimal;

                                                // set precision and scale
                                                string precision = databaseField[3].ToString();
                                                string scale = databaseField[4].ToString();

                                                // if the precision and scale exist
                                                if ((!String.IsNullOrEmpty(precision)) && (!String.IsNullOrEmpty(scale)))
                                                {
                                                    // set the precision and scale
                                                    field.Precision = Int32.Parse(precision);
                                                    field.Scale = Int32.Parse(scale);
                                                }
                                            }
                                        }
                                        catch (Exception error2)
                                        {
                                            // for debugging only
                                            string err2 = error2.ToString();
                                        }
                                    }
                                    else
                                    {
                                        // see if this is auto increment
                                        field.IsAutoIncrement = (bool)databaseField["is_identity_column"];

                                        // Parse dataType
                                        field.DataType = ParseDataType(dataType, field);
                                    }

                                    // Attemp to set the column size
                                    if (databaseField["max_length"] != null)
                                    {
                                        rawValue = databaseField["max_length"].ToString();
                                        int rawSize = NumericHelper.ParseInteger(rawValue, 0, -1);
                                        int size = rawSize / 2;
                                        field.Size = size;
                                    }

                                   

                                    // Set IsReadOnly
                                    field.IsReadOnly = (bool)databaseField["is_updateable"];

                                    // All Fields Are Read Write
                                    field.AccessMode = DataManager.AccessMode.ReadWrite;

                                    // if all the keys have not been found yet
                                    if (primaryKeysFound < dependentPrimaryKeysCount)
                                    {
                                        // This isn't going to be perfect, but if a field name matches and the field 
                                        // is a PrimaryKey in one of the dependencies, than this field is set as a PrimaryKey
                                        field.PrimaryKey = IsFieldAPrimaryKey(field.FieldName, dependencies);

                                        // If the value for the property field.PrimaryKey is true
                                        if (field.PrimaryKey)
                                        {
                                            // Increment the value for primaryKeysFound
                                            primaryKeysFound++;
                                        }
                                    }
                            
                                    // Add to Fields Collection
                                    storedProcedure.ReturnSetSchema.Add(field);
                                }
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();

                    // Write the error to the debugger
                    DebugHelper.WriteDebugError("LoadStoredProcedureWithSchema", "SQLDatabaseConnector", error);
                }

                // return value
                return storedProcedure;
            }
            #endregion

            #region Open()
            /// <summary>
            /// This method opens a connection to the database
            /// </summary>
            public void Open()
            {
                // if the database connection exists
                if (this.HasDatabaseConnection)
                {
                    // open the connection
                    this.DatabaseConnection.Open();
                }
            } 
            #endregion
			
			#region QueryForIdentity()
			public int QueryForidentity()
			{
				// Initial Value
				int ident = 0;
				
				// If command Object Is Not Open
				if(this.command.Connection.State == 0)
				{
					// Connection Not Open
					return 0;
				}
				
				try
				{
					// sql Statement To Find identity
					string sql = "Select @@identity";
					
					// Set command Text
                    this.Command.CommandText = sql;
					
					// Retrieve the identity value and store it
					ident = (int) this.Command.ExecuteScalar();
					
					// Set identity
					this.Lastidentity = ident;
					
				}
				catch
				{
					// set to 0
					ident = 0;
				}
				
				// return value
				return ident;
			}
			#endregion
			
			#region ParseDataType(string dataType, DataField field)
            /// <summary>
            /// Parses the Data Type
            /// </summary>
            /// <param name="dataType"></param>
            /// <param name="isAutoIncrement"></param>
            /// <returns></returns>
            public DataManager.DataTypeEnum ParseDataType(string dataType, DataField field)
            {
                // initial value
                DataManager.DataTypeEnum returnValue = DataManager.DataTypeEnum.NotSupported;

                // If the field object exists
                if (NullHelper.Exists(field))
                {
                    // local
                    string fullDataType = dataType;
                
                    // if the value for isAutoIncrement is true
                    if(field.IsAutoIncrement)
	                {
		                // This Is An AutoNumber field
		                returnValue = DataManager.DataTypeEnum.Autonumber;
	                }
	                else
	                {
                        // if there is an open paren
                        int index = dataType.IndexOf("(");
                        if (index >= 0)
                        {   
                            // get the dataType before the paren (datetime2(0) becomes datetime2)
                            dataType = dataType.Substring(0, index);
                        }
					
		                // Determine Database Type
		                switch(dataType.ToLower())
		                {
			                case "system.int16":
			                case "system.int32":
                            case "system.byte":
                            case "int":
                            case "smallint":
                            case "tinyint":
						
				                // Integer
				                returnValue =  DataManager.DataTypeEnum.Integer;

                                // required
                                break;

                            case "system.int64":
                            case "bigint":

                                // BigInt
                                returnValue = DataManager.DataTypeEnum.BigInt;

                                // required
                                break;
						
			                case "system.datetime":
                            case "datetime":
                            case "datetime2":
                            case "smalldatetime":
                            case "date":
                            case "system.date":

                                // If the value for the property field.IsNullable is true
                                if (field.IsNullable)
                                {
                                    // new 
                                    returnValue = DataManager.DataTypeEnum.DateTimeNullable;
                                }
							
				                // DataTime
				                returnValue = DataManager.DataTypeEnum.DateTime;

                                // required
                                break;
						
			                case "system.string":
                            case "varchar":
                            case "nvarchar":
                            case "nchar":
                            case "char":
                            case "text":
                            case "ntext":

                                // String
                                returnValue = DataManager.DataTypeEnum.String;

                                // required
                                break;
						    
			                case "system.guid":
                            case "uniqueidentifier":
						
				                // GUID
				                returnValue = DataManager.DataTypeEnum.Guid;

                                // required
                                break;

                            case "system.decimal":
                            case "decimal":
                            case "numeric":

                                // Decimal (exact, apples to apples with the database)
                                returnValue = DataManager.DataTypeEnum.Decimal;

                                // required
                                break;

                            case "money":
                            case "smallmoney":

                                // Currency (same as LoadDataFieldsSchema uses for money)
                                returnValue = DataManager.DataTypeEnum.Currency;

                                // required
                                break;
						
			                case "system.single":
			                case "system.double":
                            case "float":
                            case "real":
						
				                // Double
				                returnValue = DataManager.DataTypeEnum.Double;

                                // required
                                break;
						
			                case "system.byte[]":
                            case "varbinary":
                            case "binary":
                            case "image":
						
				                // Binary (byte[] in the data objects)
				                returnValue = DataManager.DataTypeEnum.Binary;

                                // required
                                break;
						
			                case "system.boolean":
                            case "bit":
						
				                // Boolean
				                returnValue = DataManager.DataTypeEnum.Boolean;

                                // required
                                break;
							
			                default:
						
				                // Not Supported
				                returnValue = DataManager.DataTypeEnum.NotSupported;

                                // required
                                break;                            
		                }
	                }
                }

                // retrurn value
                return returnValue;
            }
            #endregion

            #region ParseIndexType(object type)
            /// <summary>
            /// This method parses the 'type' given and returns
            /// its matching value in the IndexTypeEnum.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            private IndexTypeEnum ParseIndexType(object type)
            {
                // initial value (default to NonClustered)
                IndexTypeEnum indexType = IndexTypeEnum.Unknown;

                try
                {
                    // if the type object exists
                    if (type != null)
                    {
                        // get a tempString
                        string typeString = type.ToString();

                        // convert the string to an int
                        int typeInt = Int32.Parse(typeString);

                        // set the return value
                        indexType = (IndexTypeEnum)typeInt;
                    }
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();
                }

                // return value
                return indexType;
            }
            #endregion
			
			#region ParseSqlDataType(string dataType)
            public DataManager.DataTypeEnum ParseSqlDataType(string dataType)
			{	
				// Trim Out Size Out Of Paranethesis ()
				int Paren = dataType.IndexOf("(");
				
				// If Paren > 0
				if(Paren > 0)
				{
					// Parentheses Are Now Gone
					dataType = dataType.Substring(0, Paren);
				}
				
				// Take Off Equal Char
				int EqualChar = dataType.IndexOf("=");
					
				// If EqualChar Was Found
				if(EqualChar > 0)
				{
					// EqualChar Is Gone Now
					dataType = dataType.Substring(0, EqualChar);
				}
				
				// Trim dataType
				dataType = dataType.Trim();
				
				// Determine Database Type
				switch(dataType.ToLower())
				{
				
					case "char":
					case "nchar":
					case "nvarchar":
					case "varchar":
					case "text":
					
						// Integer
                        return DataManager.DataTypeEnum.String;
					
					case "datetime":
						
						// DataTime
                        return DataManager.DataTypeEnum.DateTime;
					
					case "int":
					case "real":
					
						// String
                        return DataManager.DataTypeEnum.Integer;
					
					case "varbinary":
					case "image":
					
						// Image
                        return DataManager.DataTypeEnum.NotSupported;
						
					case "money":
					
						// Currency
                        return DataManager.DataTypeEnum.Currency;
						
			        case "decimal":
			        
			            // double
                        return DataManager.DataTypeEnum.Double;
						
					default:
					
						// Not Supported
                        return DataManager.DataTypeEnum.NotSupported;
							
				}
				
			}
			#endregion
			
			#region SetStoredProcedureType(string fullProcedureText)
			public StoredProcedureTypes SetStoredProcedureType(string fullProcedureText)
			{
				
				// Set To Lower Case
				fullProcedureText = fullProcedureText.ToLower();
			
				// If Select
				if(fullProcedureText.IndexOf("select") > 0)
				{
					return StoredProcedureTypes.Select;
				}
				
				// If Update
				if(fullProcedureText.IndexOf("update") > 0)
				{
					return StoredProcedureTypes.Update;
				}
				
				// If Insert
				if(fullProcedureText.IndexOf("insert") > 0)
				{
					return StoredProcedureTypes.Insert;
				}
				
				
				// If Delete
				if(fullProcedureText.IndexOf("delete") > 0)
				{
					return StoredProcedureTypes.Delete;
				}
				
				// Return Not Set
				return StoredProcedureTypes.NotSet;
				
			}	
			#endregion
			
		#endregion

		#region Properties

			#region Captureidentity
			public bool Captureidentity
			{
				get
				{
					return captureidentity;
				}
				set
				{	
					captureidentity = value;
				}
			}
			#endregion

            #region Command
			public SqlCommand Command
			{
				get
				{
					return command;
				}
				set
				{
					command = value;
				}
			}
			#endregion
			
		    #region Connected
			public bool Connected
			{
				get
				{
					// initial value
					bool connected = (this.DatabaseConnection.State == ConnectionState.Open);
					
					// return value
					return connected;
				}
			}
			#endregion

            #region ConnectionString
            /// <summary>
            /// This property gets or sets the ConnectionString.
            /// </summary>
            public string ConnectionString
            {
                get { return connectionString; }
                set 
                { 
                    // if the string exists, but does not contains MultipleActiveResultsSets = true;
                    if ((value != null) && (!value.Contains("MultipleActiveResultSets=True")))
                    {
                        // if the value does not end with a semicolon
                        if (value.EndsWith(";"))
                        {
                            // Set the value
                            value += MultipleResultSets;
                        }
                        else
                        {
                            // Set the value
                            value += ";" + MultipleResultSets;
                        }
                    }

                    // set the value
                    connectionString = value;

                    // if the DatabaseConnection exists
                    if (this.HasDatabaseConnection) 
                    {
                        // Set the ConnectionString
                        this.DatabaseConnection.ConnectionString = value;
                    }
                }
            } 
            #endregion

			#region DatabaseConnection 
			public SqlConnection DatabaseConnection 
			{
				get
				{
					return databaseconnection;
				}
				set
				{
					databaseconnection = value;
				}
			}
			#endregion

			#region FailedReason
			public string FailedReason
			{
				get
				{
					return failedreason;
				}
				set
				{
					failedreason = value;
				}
			}
			#endregion

            #region HasDatabaseConnection
            /// <summary>
            /// This read only property returns true if this object has a DatabaseConnection.
            /// </summary>
            public bool HasDatabaseConnection
            {
                get
                {
                    // initial value
                    bool hasDatabaseConnection = (this.DatabaseConnection != null);

                    // return value
                    return hasDatabaseConnection;
                }
            } 
            #endregion

            #region IgnoreDataSync
            /// <summary>
            /// This property gets or sets the value for IgnoreDataSync.
            /// </summary>
            public bool IgnoreDataSync
            {
               get { return ignoreDataSync; }
               set { ignoreDataSync = value; }
            }
            #endregion

            #region IgnoreAzureFirewallRules
            /// <summary>
            /// This property gets or sets the value for ignoreAzureFirewallRulesProcedure.
            /// </summary>
            public bool IgnoreAzureFirewallRules
            {
               get { return ignoreAzureFirewallRules; }
               set { ignoreAzureFirewallRules = value; }                       
            }
            #endregion
			
			#region IsInsert
			public bool IsInsert
			{
				get
				{
					return isinsert;
				}
				set
				{
					isinsert = value;
				}
			}
			#endregion
			
			#region Lastidentity
			public int Lastidentity
			{
				get
				{
					return lastidentity;
				}
				set
				{
					lastidentity = value;
				}
			}
			#endregion
			
			#region LastSQL
			public string LastSQL
			{
				get
				{
					return lastsql;
				}
				set
				{
					lastsql = value;
				}
			}
			#endregion

			#region LoadViews
			public bool LoadViews
			{
				get
				{
					return loadviews;
				}
				set
				{
					loadviews = value;
				}
			}
			#endregion	

		#endregion

    }
	#endregion
    
}

