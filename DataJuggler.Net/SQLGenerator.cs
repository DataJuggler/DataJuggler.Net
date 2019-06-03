

#region using statements

using System;
using System.Text;
using System.Collections.Generic;
using DataJuggler.Core.UltimateHelper;

#endregion

namespace DataJuggler.Net
{

    #region class SQLGenerator
	public class SQLGenerator
	{
	
		#region private variables
		#endregion

		#region Constructors
		#endregion

		#region Methods

            #region AppendField(StringBuilder sb, DataField field, bool firstField)
            /// <summary>
            /// This method appends a field in the make of a FieldsList.
            /// </summary>
            /// <param name="sb"></param>
            /// <param name="field"></param>
            /// <param name="firstField"></param>
            private void AppendField(ref StringBuilder sb, DataField field, bool firstField)
            {
                // if this is the first field
                if (!firstField)
                {
                    sb.Append(",");
                }

                // set firstField to false so the commas are added
                firstField = false;

                // append open [
                sb.Append("[");

                // Append 
                sb.Append(field.FieldName);

                // Append Closing ]
                sb.Append("]");
            }
            #endregion

			#region AppendFieldValue(string fieldValue)
			public string AppendFieldValue(string FieldValue)
			{
				// If String Is Not Null Replace Any Apostraphe's 
				if(FieldValue != null)
				{
					// Replace Any Apostraphe's
					FieldValue = FieldValue.Replace("'","''");
				}
				
				// Return fieldValue
				return FieldValue;
				
				
			}
			#endregion

			#region BuildFieldClause(DataJuggler.Net.DataField field)
			public string BuildFieldClause(DataJuggler.Net.DataRow FieldRow, DataJuggler.Net.DataField Field, int FieldCount)
			{
				// if FieldCount > 0
				StringBuilder sb = new StringBuilder();
				
				// If FieldCount > 0
				if(FieldCount > 1)
				{
					// Append The Word And
					sb.Append(" And ");
				}
			
				// Append fieldName
				sb.Append(Field.FieldName);
				sb.Append(" = ");

				// If This Is Not Numeric Data Add Single Quote '
				if(!FieldRow.IsNumericDataType(Field.DataType))
				{
					sb.Append("'");			
				}

				// Append fieldValue
				sb.Append(Field.FieldValue);

				// If This Is Not Numeric Data Add Single Quote '
				if(!FieldRow.IsNumericDataType(Field.DataType))
				{
					sb.Append("'");
							
				}

				// Return New String
				return sb.ToString();
				
			}
			#endregion

			#region BuildWhereClause(DataJuggler.Net.DataRow row)
			public string BuildWhereClause(DataJuggler.Net.DataRow Row)
			{
				// Create StringBuilder
				StringBuilder sb = new StringBuilder(" Where ");
				
				// String 
				string FieldClause = "";
			
				// int FieldCount
				int PrimaryKeyCount = 0;
					
				// Loop Through Each field						
				foreach(DataJuggler.Net.DataField Field in Row.Fields)
				{
					// If This field Is A Primary Key
					if(Field.PrimaryKey)
					{	
						// Increment Primary Key Count
						PrimaryKeyCount++;
							
						// Build FieldClause
						FieldClause = BuildFieldClause(Row, Field, PrimaryKeyCount);
						
						// Append FieldClause
						sb.Append(FieldClause);
							
					}
				}
				
				// Return 
				return sb.ToString();
				
			}
			#endregion

			#region CreateDeleteSQL(DataJuggler.Net.DataRow row)
			public string CreateDeleteSQL(DataJuggler.Net.DataRow Row)
			{
				// Verify There Are Changes In This row & The row Is Updateable
				if(IsUpdateable(Row))
				{
					// Create StringBuilder
					StringBuilder sb = new StringBuilder("Delete From [");
			
					// Append tableName
					sb.Append(Row.ParentTable.Name);
					
					// Append Closing Bracket
					sb.Append("]");
					
					// Add Where Clause
					string WhereClause = BuildWhereClause(Row);
					sb.Append(WhereClause);

					// Return sql Statement To Insert This row
					return sb.ToString();
				}
				else
				{
					return "void";
				}
			}
			#endregion

            #region CreateFieldList(DataTable dataTable, bool isSelectStatement)
            /// <summary>
            /// This method builds a list of fields comma seperated
            /// enclosed in parentheses
            /// </summary>
            /// <param name="dataList"></param>
            /// <returns></returns>
            public string CreateFieldList(DataTable dataTable, bool isSelectStatement)
            {
                // Create StringBuilder
                StringBuilder sb = new StringBuilder("");

                // if this is a select statement
                if (isSelectStatement)
                {
                    // add Select
                    sb.Append("Select ");
                }
                else
                {
                    // Add Open Paren (
                    sb.Append("(");
                }

                // bool firstField
                bool firstField = true;

                // Get the dataList
                List<DataField> fields = null;

                // local
                bool containsBinaryData;

                // if the data table exists
                if (dataTable != null)
                {
                    // Get the fields collection
                    fields = dataTable.Fields;

                    // This table does contain binary data.
                    containsBinaryData = dataTable.ContainsBinaryData;
                }

                // Get a BinaryField
                DataField binaryField = null;

                // if fields collection exist
                if (ListHelper.HasOneOrMoreItems(fields))
                {
                    // loop through each field
                    foreach (DataField field in fields)
                    {
                        // Only add the Primary key for select statements
                        if ((!field.PrimaryKey) || (isSelectStatement))
                        {
                            // If this is not a Binary field,
                            // Binary Fields must be last.
                            if (field.DataType != DataManager.DataTypeEnum.Binary)
                            {
                                // Append This field
                                AppendField(ref sb, field, firstField);
                                
                                // the first field is no longer true
                                firstField = false;
                            }
                            else
                            {
                                // This is a BinaryField
                                binaryField = field;
                            }
                        }
                    }
                }

                // If the BinaryFieldExists
                if (binaryField != null)
                {
                    // Append the field
                    AppendField(ref sb, binaryField, firstField);
                }

                // if this is not a select statement
                if (!isSelectStatement)
                {
                    // Add closing )
                    sb.Append(")");
                }

                // return value
                return sb.ToString();
            }
            #endregion
			
			#region CreateInsertSQL(DataJuggler.Net.DataRow row) +1 override
			
				#region CreateInsertSQL(DataJuggler.Net.DataRow row)
				public string CreateInsertSQL(DataJuggler.Net.DataRow Row)
				{
					// Return CreateInsertSQL
					return CreateInsertSQL(Row,false);
					
				}
				#endregion 
				
				#region CreateInsertSQL(DataJuggler.Net.DataRow row, bool WithOrWithoutChanges)
				public string CreateInsertSQL(DataJuggler.Net.DataRow Row, bool WithOrWithoutChanges)
				{
					// Boolean For Is This The First field
					bool FirstField = true;

					// Verify There Are Changes In This row
					if((Row.Changes) || (WithOrWithoutChanges))
					{
						// Create StringBuilder
						StringBuilder sb = new StringBuilder("Insert Into [");
				
						// Append tableName
						sb.Append(Row.ParentTable.Name);
						sb.Append("] (");

						// Add Each fieldName
						foreach(DataJuggler.Net.DataField Field in Row.Fields)
						{
							// If This field.dataType Is Supported
							if(DataManager.IsSupported(Field.DataType))
							{
								// If This field Is Not An AutoNumber && This Is Not A Null Date (Had To Be And)
								if((Field.DataType != DataManager.DataTypeEnum.Autonumber) && (!IsNullDateField(Field)) && (Field.HasValue()))
								{
									// If This Is Not The First Item Add A Comma
									if(!FirstField)
									{
										
										sb.Append(",");
									}
							
									// Append fieldName
									sb.Append(Field.FieldName);

									// Set FirstField To False
									FirstField = false;
								}
							}
						}

						// Append Values
						sb.Append(") Values(");

						// Reset First field
						FirstField = true;

						// Add Each fieldName
						foreach(DataJuggler.Net.DataField Field in Row.Fields)
						{
							// If This field.dataType Is Supported
							if(DataManager.IsSupported(Field.DataType))
							{
								// If This field Is Not An AutoNumber && This Is Not A Null Date (Had To Be And)
								if((Field.DataType != DataManager.DataTypeEnum.Autonumber) && (!IsNullDateField(Field)) && (Field.HasValue()))
								{
									// If This Is Not The First Item Add A Comma
									if(!FirstField)
									{
										sb.Append(",");
									}

									// If This Is A String Add A Single Quote
									if(!Row.IsNumericDataType(Field.DataType))
									{
										sb.Append("'");
									}

									// Add fieldValue
									sb.Append(AppendFieldValue(Field.FieldValue.ToString()));
									
									// If This Is A String Add A Single Quote
									if(!Row.IsNumericDataType(Field.DataType))
									{
										sb.Append("'");
									}

									// Set FirstField To False
									FirstField = false;
							
								}
							}
						}

						// Append Closing Parenthese
						sb.Append(")");

						// Return sql Statement To Insert This row
						return sb.ToString();
						
					}
					else
					{
						return "void";
					}
				}
				#endregion 
				
			#endregion

			#region CreateSelectAllSQL(DataJuggler.Net.DataTable table) 
			/// <summary>
			/// Creates A Valid sql Statement To Select All Records From A Given table
			/// </summary>
			/// <param name="table">table To Create sql Statement For</param>
			/// <returns>sql Statement To Select All Records From A Given table</returns>
			public string CreateSelectAllSQL(DataJuggler.Net.DataTable dataTable)
			{		
				// If tableName Is Null
                if ((dataTable == null) || (dataTable.Name == null))
				{
					return "void";
				}
					
				StringBuilder sb = new StringBuilder();
				
				// Create StringBuilder 
				string selectAllSQL = dataTable.SQLGenerator.CreateFieldList(dataTable, true);
				
				// append 
				sb.Append(selectAllSQL);
					
				// Append Open Brack
				sb.Append(" From [");
				
				// Append tableName
                sb.Append(dataTable.Name);
					
				// Append Where
                sb.Append("]");
					
				// return SelectSQL
                return sb.ToString();
				
			}
			#endregion

			#region CreateSelectSQL(DataJuggler.Net.DataTable table, int primaryKeyValue) +3 overrides
			
				#region CreateSelectSQL(DataJuggler.Net.DataTable table, int primaryKeyValue) 
				/// <summary>
				/// Returns a valid sql statement to load an object from a table based upon the primary key.
				/// This is for a single field primary key.
				/// </summary>
				/// <param name="table">DataJuggler.Net.DataTable</param>
				/// <param name="primaryKeyValue">An int value in where clause</param>
				/// <returns>A valid sql statement if the table has a primary key</returns>
				public string CreateSelectSQL(DataJuggler.Net.DataTable table, int primaryKeyValue)
				{
					// Verify table Has A Primary Key
					if(table.PrimaryKey == null)
					{
						return "void";
					}
				
					// Verify table Has A Primary Key
					if(table.PrimaryKey.FieldName == null)
					{
						return "void";
					}
					
					// Create StringBuilder 
					StringBuilder sb = new StringBuilder("Select * From [");
					
					// Append tableName
					sb.Append(table.Name);
					
					// Append Where
					sb.Append("] Where ");
					
					// Append PrimaryKey.fieldName
					sb.Append(table.PrimaryKey.FieldName);
					
					// Append = 
					sb.Append(" = ");
					
					// Append Value
					sb.Append(primaryKeyValue.ToString());
					
					// return SelectSQL
					return sb.ToString();
				
				}
				#endregion
				
				#region CreateSelectSQL(DataJuggler.Net.DataTable table, double primaryKeyValue) 
				/// <summary>
				/// Returns a valid sql statement to load an object from a table based upon the primary key.
				/// This is for a single field primary key.
				/// </summary>
				/// <param name="table">DataJuggler.Net.DataTable</param>
				/// <param name="primaryKeyValue">A double value in where clause</param>
				/// <returns>A valid sql statement if the table has a primary key</returns>
				public string CreateSelectSQL(DataJuggler.Net.DataTable table, double primaryKeyValue)
				{
					// Verify table Has A Primary Key
					if(table.PrimaryKey != null)
					{
						return "void";
					}
				
					// Verify table Has A Primary Key
					if(table.PrimaryKey.FieldName != null)
					{
						return "void";
					}
					
					// Create StringBuilder 
					StringBuilder sb = new StringBuilder("Select * From [");
					
					// Append tableName
					sb.Append(table.Name);
					
					// Append Where
					sb.Append("] Where ");
					
					// Append PrimaryKey.fieldName
					sb.Append(table.PrimaryKey.FieldName);
					
					// Append = 
					sb.Append(" = ");
					
					// Append Value
					sb.Append(primaryKeyValue.ToString());
					
					// return SelectSQL
					return sb.ToString();
				
				}
				#endregion
				
				#region CreateSelectSQL(DataJuggler.Net.DataTable table, string primaryKeyValue) 
				/// <summary>
				/// Returns a valid sql statement to load an object from a table based upon the primary key.
				/// This is for a single field primary key.
				/// </summary>
				/// <param name="table">DataJuggler.Net.DataTable</param>
				/// <param name="primaryKeyValue">A string value in where clause</param>
				/// <returns>A valid sql statement if the table has a primary key</returns>
				public string CreateSelectSQL(DataJuggler.Net.DataTable table, string primaryKeyValue)
				{
					// Verify table Has A Primary Key
					if(table.PrimaryKey != null)
					{
						return "void";
					}
				
					// Verify table Has A Primary Key
					if(table.PrimaryKey.FieldName != null)
					{
						return "void";
					}
					
					// Create StringBuilder 
					StringBuilder sb = new StringBuilder("Select * From [");
					
					// Append tableName
					sb.Append(table.Name);
					
					// Append Where
					sb.Append("] Where ");
					
					// Append PrimaryKey.fieldName
					sb.Append(table.PrimaryKey.FieldName);
					
					// Append = 
					sb.Append(" = '");
					
					// Append Value
					sb.Append(primaryKeyValue.ToString());
					
					// Append Closing Single Quote
					sb.Append("'");
					
					// return SelectSQL
					return sb.ToString();
				
				}
				#endregion
				
				#region CreateSelectSQL(DataJuggler.Net.DataTable table, string fieldName, string fieldValue) 
				/// <summary>
				/// Returns a valid sql statement to load an object from a table based upon a fieldName &
				/// field value passed in. This method is for a single field >> value pair.
				/// </summary>
				/// <param name="fieldName">field name that where clause will include</param>
				/// <param name="fieldValue">value in where clause</param>
				/// <returns>A valid sql statement to select a row with a certain field, value</returns>
				public string CreateSelectSQL(DataJuggler.Net.DataTable table, string fieldName, string fieldValue)
				{
					// bool UseQuotes
					bool UseQuotes = IsString(table, fieldName);
					
					// If tableName Is Null
					if(table.Name == null)
					{
						return "void";
					}
					
					// fieldValue Exists
					if(fieldValue == null)
					{
						return "void";
					}
				
					// Verify fieldName Is Not Null
					if(fieldName == null)
					{
						return "void";
					}
					
					// Create StringBuilder 
					StringBuilder sb = new StringBuilder("Select * From [");
					
					// Append tableName
					sb.Append(table.Name);
					
					// Append Where
					sb.Append("] Where ");
					
					// Append field.fieldName
					sb.Append(fieldName);
					
					// Append = 
					sb.Append(" = ");
					
					// If This Is A String
					if(UseQuotes)
					{
						// Append Single Quote
						sb.Append("'");
					}
					
					// Append Value
					sb.Append(fieldValue);
					
					// If This Is A String
					if(UseQuotes)
					{
						// Append Single Quote
						sb.Append("'");
					}
					
					// return SelectSQL
					return sb.ToString();
				
				}
				#endregion
				
			#endregion
			
			#region CreateUpdateSQL(DataJuggler.Net.DataRow row)
			public string CreateUpdateSQL(DataJuggler.Net.DataRow Row)
			{
				// Boolean For Is This The First field
				bool FirstField = true;

				// Verify There Are Changes In This row & The row Is Updateable
				if((Row.Changes) && (IsUpdateable(Row)))
				{
					// Create StringBuilder
					StringBuilder sb = new StringBuilder("Update [");
			
					// Append tableName
					sb.Append(Row.ParentTable.Name);
					sb.Append("] Set ");

					// Add Each fieldName
					foreach(DataJuggler.Net.DataField Field in Row.Fields)
					{
						// If This field.dataType Is Supported
						if(DataManager.IsSupported(Field.DataType))
						{
							// If This Is Not The Primary Key
							if(!Field.PrimaryKey)
							{
								// If This field Has Changes
								if(Field.Changes)
								{
									// If This Is Not The First Item Add A Comma
									if(!FirstField)
									{
										sb.Append(",");
									}
							
									// Append fieldName
									sb.Append(Field.FieldName);

									// Append Equal Sign =
									sb.Append(" = ");

									// If This Is Not Numberic Data Add Single Quote '
									if(!Row.IsNumericDataType(Field.DataType))
									{
										sb.Append("'");
									}
									
									// Add field Value
									sb.Append(AppendFieldValue(Field.FieldValue.ToString()));
									
									// If This Is Not Numberic Data Add Single Quote '
									if(!Row.IsNumericDataType(Field.DataType))
									{
										sb.Append("'");
									}

									// Set FirstField To False
									FirstField = false;
								}
							}
						}
					}

					// Verify 1 Or More Fields Has Been Updated
					if(!FirstField)
					{
						// Add Where Clause
						string WhereClause = BuildWhereClause(Row);
						sb.Append(WhereClause);

						// Return sql Statement To Insert This row
						return sb.ToString();
					}
					else
					{
						return "void";
					}
				}
				else
				{
					return "void";
				}
			}
			#endregion

            #region IsDate(string dateString)
            /// <summary>
			/// Checks whether or not a date is a valid date.
			/// </summary>
			/// <param name="DateString"></param>
			/// <returns>True If A Valid Date</returns>
			public bool IsDate(string dateString) 
			{
				// If this String Is Null Return false
				if(dateString == null)
				{
					// return false
					return false;
				}
				
				// If DateString has no length 
				if(dateString.Length < 4)
				{
					// return false
					return false;
				}

				try 
				{
					// Local Variable For Date
					DateTime dt;
					
					// Attempt To Parse Date
					dt = DateTime.Parse(dateString);  
				}
				catch 
				{
					// Not A Date
					return false;
				} 
				
				// Is A Valid Date
				return true;
			} 
			#endregion

			#region IsNullDateField(DataField field)
			public bool IsNullDateField(DataField field)
			{
				// Determine dataType
                if (field.DataType == DataManager.DataTypeEnum.DateTime)
				{
					// If fieldValue Is Null
                    if (field.FieldValue == null)
					{
						// fieldValue Is Null return true
						return true;
						
					}
					
					// If Not A Date
                    if (!IsDate(field.FieldValue.ToString()))
					{
						// Not A Date
						return true;
					}
					
				}
				
				// Not A Null Date field
				return false;
			}
			#endregion

			#region IsString(string fieldName)
			public bool IsString(DataJuggler.Net.DataTable table, string fieldName)
			{
                // initial value
                bool isString = false;
                
				// Create Generic row
				DataJuggler.Net.DataRow row = new DataRow();
				
				// Get the Index Of Fieldname
                int Index = SQLDatabaseConnector.FindFieldIndex(table.ActiveFields, fieldName); 
				
				// If Index Was Found
				if(Index >= 0)
				{	
                    // if this field is a string
					if(table.ActiveFields[Index].DataType == DataManager.DataTypeEnum.String)
					{
						// Use Quotes For Strings
						isString = true;
					}
				}
				
				// return value
				return isString;
				
			}
			#endregion

			#region IsUpdateable(DataJuggler.Net.DataRow row)
			public bool IsUpdateable(DataJuggler.Net.DataRow Row)
			{
                // Initial Value
                bool updateable = false;

				// Check If There Is A PrimaryKey
				DataJuggler.Net.DataField PrimaryKey = Row.PrimaryKey();

				// Check If PrimaryKey Exists
				if(PrimaryKey.FieldName == "void")
				{
					// Not Updateable
                    updateable = false;
				}
				else
				{
					// Check If PrimaryKey Has A Value
					// If It Does Have A Value Then It Is Updateable
					// Else It Is Not
					updateable = PrimaryKey.HasValue();
				}

                // return value
                return updateable;

			}
			#endregion

		#endregion

		#region Properties
		#endregion

	}
	#endregion
    
}
