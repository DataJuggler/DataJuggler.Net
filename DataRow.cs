

#region using statements

using System.Collections.Generic;

#endregion

namespace DataJuggler.Net
{

    #region class DataRow
	public class DataRow
	{

		#region Private Variables
		private bool delete;
		private List<DataField> fields;
		private int index;
		private DataJuggler.Net.DataTable parentTable;
		private string tag;
		#endregion

		#region Constructor
		
			#region DataRow()
			public DataRow()
			{
				// Create Fields Collection
				fields = new List<DataField>();
			}
			#endregion
		
			#region DataRow(DataJuggler.Net.DataTable ParentDataTable)
			public DataRow(DataJuggler.Net.DataTable ParentDataTable)
			{
				// Set Parent table
				this.ParentTable = ParentDataTable;

				// Create Fields Collection
				fields = new List<DataField>();
			}
			#endregion
		
		#endregion

		#region Methods

			#region IsNumeric(string expression) bool
			public bool IsNumeric(string expression) 
			{ 
				// If field Is Null Return False
				if(expression == null)
				{
					// Not A Number
					return false;
				}

				// If field Is Zero Length
				if(expression.Length == 0)
				{
					// Not A Number
					return false;
				}

				// Can Only Have 1 Decimal, Dollar Sign Or % Sign or - Sign
				bool hasDecimal = false;
				bool hasDollar = false;
				bool hasPercent = false;
				bool HasNegativeSign = false;

				// Loop Through Each Char In Expression
				for(int i=0;i<expression.Length;i++) 
				{ 
					// Check for decimal
					if (expression[i] == '.')
					{
						// If Second Decimal
						if(hasDecimal)
						{
							// Not A Number
							return false;
						}
						else 
						{
							// Has Decimal 
							hasDecimal = true;
							continue;
						}
					}
					
					// Check for decimal
					if (expression[i] == '-')
					{
						// If Second Decimal
						if(HasNegativeSign)
						{
							// Not A Number
							return false;
						}
						else 
						{
							// Has Decimal 
							HasNegativeSign = true;
							continue;
						}
					}

					// Check for Dollar Sign
					if (expression[i] == '$')
					{
						// If Second Dollar Sign
						if(hasDollar)
						{
							// Not A Number
							return false;
						}
						else 
						{
							// Has Dollar = true;
							hasDollar = true;
							continue;
						}
					}

					// Check for Percent
					if (expression[i] == '%')
					{
						// If Second Decimal
						if(hasPercent)
						{
							// Not A Number
							return false;
						}
						else 
						{
							// Has Percent 
							hasPercent = true;
							continue;
						}
					}
					
					// check if number
					if(!char.IsNumber(expression[i])) 
					{
						// Not A Number
						return false; 
					}

				} 

				// This Is A Number
				return true; 
			} 
			#endregion

            #region IsNumericEx(string expression) bool
            public static bool IsNumericEx(string expression)
            {
                // initial value
                bool isNumeric = false;
                
                // verify the string exists
                if (!string.IsNullOrEmpty(expression))
                {
                    // Can Only Have 1 Decimal, Dollar Sign Or % Sign or - Sign
                    bool hasDecimal = false;
                    bool hasDollar = false;
                    bool hasPercent = false;
                    bool HasNegativeSign = false;

                    // Loop Through Each Char In Expression
                    for (int i = 0; i < expression.Length; i++)
                    {
                        // Check for decimal
                        if (expression[i] == '.')
                        {
                            // If Second Decimal
                            if (hasDecimal)
                            {
                                // Not A Number
                                isNumeric = false;

                                // break out of loop
                                break;
                            }
                            else
                            {
                                // Has Decimal 
                                hasDecimal = true;
                                continue;
                            }
                        }

                        // Check for decimal
                        if (expression[i] == '-')
                        {
                            // If Second Decimal
                            if (HasNegativeSign)
                            {
                                // Not A Number
                                isNumeric = false;

                                // break out of loop
                                break;
                            }
                            else
                            {
                                // Has Decimal 
                                HasNegativeSign = true;
                                continue;
                            }
                        }

                        // Check for Dollar Sign
                        if (expression[i] == '$')
                        {
                            // If Second Dollar Sign
                            if (hasDollar)
                            {
                                // Not A Number
                                isNumeric = false;

                                // break out of loop
                                break;
                            }
                            else
                            {
                                // Has Dollar = true;
                                hasDollar = true;
                                continue;
                            }
                        }

                        // Check for Percent
                        if (expression[i] == '%')
                        {
                            // If Second Decimal
                            if (hasPercent)
                            {
                                // Not A Number
                                isNumeric = false;
                                
                                // break out of loop
                                break;
                            }
                            else
                            {
                                // Has Percent 
                                hasPercent = true;
                                continue;
                            }
                        }

                        // check if number
                        if (!char.IsNumber(expression[i]))
                        {
                            // Not A Number
                            isNumeric = false;

                            // break out of loop
                            break;
                        }
                    }
                }

                // This Is A Number
                return isNumeric;
            }
            #endregion

            #region IsNumericDataType(DataManager.DataTypeEnum dataType) bool
            public bool IsNumericDataType(DataManager.DataTypeEnum dataType) 
			{ 
				switch(dataType)
				{
                    case DataManager.DataTypeEnum.Autonumber:
						return true;
                    case DataManager.DataTypeEnum.Currency:
						return true;
                    case DataManager.DataTypeEnum.Double:
						return true;
                    case DataManager.DataTypeEnum.Integer:
						return true;
                    case DataManager.DataTypeEnum.Percentage:
						return true;
				}
				
				// This Is Not A Numeric dataType
				return false;
			} 
			#endregion

            #region StaticIsNumericDataType(DataJuggler.Net.DataTypeEnum dataType) bool
            public static bool StaticIsNumericDataType(DataManager.DataTypeEnum dataType) 
			{ 
				switch(dataType)
				{
                    case DataManager.DataTypeEnum.Autonumber:
						return true;
                    case DataManager.DataTypeEnum.Currency:
						return true;
                    case DataManager.DataTypeEnum.Double:
						return true;
                    case DataManager.DataTypeEnum.Integer:
						return true;
                    case DataManager.DataTypeEnum.Percentage:
						return true;
				}
				
				// This Is Not A Numeric dataType
				return false;
			} 
			#endregion

			#region ParseStringToDouble(string fieldValue) double
			public double ParseStringToDouble(string FieldValue)
			{
				// Determine If fieldValue Is Numeric
				if(IsNumeric(FieldValue))
				{
					// This Is A Number Parse Number
					return System.Double.Parse(FieldValue);
				}
				else
				{
					// Not A Number Return 0
					return 0;
				}
			}
			#endregion

			#region ParseStringToInteger(string fieldValue) int
			public int ParseStringToInteger(string FieldValue)
			{
				// Determine If fieldValue Is Numeric
				if(IsNumeric(FieldValue))
				{
					// This Is A Number Parse Number
					return System.Int32.Parse(FieldValue);
				}
				else
				{
					// Not A Number Return 0
					return 0;
				}
			}
			#endregion

			#region PrimaryKey()
			public DataJuggler.Net.DataField PrimaryKey()
			{
				// Create DataField 
				DataJuggler.Net.DataField NoField = new DataField();
				NoField.FieldName = "void";

				// Search For PrimaryKey
				foreach(DataJuggler.Net.DataField Field in this.Fields)
				{
					// Check For PrimaryKey
					if(Field.PrimaryKey)
					{
						// Return This field
						return Field;
					}

				}

				// Return NoField
				return NoField;
			}
        #endregion

        #endregion

        #region Properties

			#region Changes
			public bool Changes
			{
				get
				{
					// If Any field Has Changes Then The Fields Collection Has Changes
					foreach(DataJuggler.Net.DataField Field in this.Fields)
					{
						if(Field.Changes)
						{
							return true;
						}
					}

					// If Delete = True 
					if(Delete)
					{
						return true;
					}

					// No Changes
					return false;
				}
				set
				{
					// If Any field Has Changes Then The Fields Collection Has Changes
					foreach(DataJuggler.Net.DataField Field in this.Fields)
					{
						Field.Changes = value;
					}

				}
			}
			#endregion

			#region Delete
			public bool Delete
			{
				get
				{
					return delete;
				}
				set
				{
					delete = value;
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
	
			#region Index
			public int Index
			{
				get
				{
					return index;
				}
				set
				{
					index = value;
				}
			}
			#endregion

			#region ParentTable
			public DataJuggler.Net.DataTable ParentTable
			{
				get
				{
					return parentTable;
				}
				set
				{
					parentTable = value;
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

		#endregion

	}
	#endregion
    
}
