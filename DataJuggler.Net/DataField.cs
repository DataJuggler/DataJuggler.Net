

#region using statements

using System;
using System.Collections.Generic;

#endregion

namespace DataJuggler.Net
{

    #region Class DataField
	/// <summary>
	/// DataField Object Used For DataJuggler.Net
	/// </summary>
	[Serializable]
	public class DataField
	{

		#region Private Variables
		private DataManager.AccessMode accessmode;
		private string caption;
		private bool changes;
        private DataManager.DataTypeEnum datatype;
		private string dbfieldname;
        private string dbDataType;
		private int decimalplaces;
		private string defaultvalue;
		private bool exclude;
		private int fieldordinal;
		private string fieldname;
		private object fieldvalue;
		private bool hasdefault;
		private int index;
		private bool isnullable;
		private bool isreadonly;
		private bool loading;
		private bool primarykey;
		private bool required;
		private DataManager.Scope scope;
		public const int DefaultFieldSize = 50;
		private int size;
		private bool isautoincrement;
	    private bool visible;
        private int precision;
        private int scale;
        private bool doNotSetCaption;
        private bool treatIntegerAsBoolean;
        private bool isEnumeration;
        private string enumDataTypeName;
        
        // used only for DataTier.Net
        private string fieldSetName;
        private int fieldId;
        private int tableId;
        #endregion

		#region Constructor
		public DataField()
		{
			// Set Defaults
			this.Scope = DataManager.Scope.Public;
			this.AccessMode = DataManager.AccessMode.ReadWrite;
			this.Visible = true;
		}
		#endregion

		#region Methods

			#region Clone() +2 override

				#region Clone()
				public DataField Clone()
				{
					return Clone(false,false);
				}
				#endregion

                #region Clone(bool includeFieldValue)
                /// <summary>
				/// Clones The field Without The fieldValue
				/// If IngoreFieldValue 
				/// </summary>
				/// <param name="IngoreFieldValue"></param>
				/// <returns></returns>
				public DataField Clone(bool includeFieldValue)
				{
					// Call Clone Method  
					return Clone(includeFieldValue,false);
				}
				#endregion

                #region Clone(bool IncludeFieldValue, bool loading)
                /// <summary>
				/// Clones The field Without The fieldValue
				/// I forget why loading is here, it might have been
                /// a client app that needed it to prevent 
                /// changes being set.
				/// </summary>
				/// <param name="IngoreFieldValue"></param>
				/// <returns></returns>
				public DataField Clone(bool includeFieldValue, bool loading)
				{
					// Create field To Return
					DataField Field = new DataField();

					// Clone Each Property
					Field.AccessMode = this.AccessMode;
					Field.DataType = this.DataType;
					Field.Exclude = this.Exclude;
					Field.FieldName = this.FieldName;
					Field.Index = this.Index;
					Field.IsNullable = this.IsNullable;
					Field.PrimaryKey = this.PrimaryKey;
					Field.Required = this.Required;
					Field.Scope = this.Scope;
					Field.Size = this.Size;

					// Do Not Clone field Value If includeFieldValue = false
					if(!includeFieldValue)
					{
						// Check If Loading Property Is True
						if(loading)
						{
							Field.Loading = true;
						}
						
						// Set field Value
						Field.FieldValue = this.FieldValue;

						// Reset Loading Property
						this.Loading = false;
					}

					// Return This field
					return Field;
						
				}
				#endregion

			#endregion

			#region ConvertAccessMode()
			public DataManager.AccessMode ConvertAccessMode(string accessMode)
			{
				if(accessMode != null)
				{
					switch(accessMode.ToLower())
					{
						case "readonly":
							return DataManager.AccessMode.ReadOnly;
						case "writeonly":
							return DataManager.AccessMode.WriteOnly;
						default:
							return DataManager.AccessMode.ReadWrite;
					}
				}
				else
				{
					// return default ReadWrite
					return DataManager.AccessMode.ReadWrite;
				}

			}
			#endregion

			#region ConvertDataType(string dataType) +1 overrides

				#region DataTypeEnum ConvertDataType(string dataType)
				public DataManager.DataTypeEnum ConvertDataType(string dataType)
				{
					if(dataType != null)
					{
						switch(dataType.ToLower())
						{
							case "autonumber":
                                return DataManager.DataTypeEnum.Autonumber;
							case "currency":
                                return DataManager.DataTypeEnum.Currency;
							case "datetime":
                                return DataManager.DataTypeEnum.DateTime;
							case "double":
                                return DataManager.DataTypeEnum.Double;
							case "integer":
                                return DataManager.DataTypeEnum.Integer;
							case "percentage":
                                return DataManager.DataTypeEnum.Percentage;
							case "string": // String, Memo, Text
                                return DataManager.DataTypeEnum.String;
							case "DataTable":
                                return DataManager.DataTypeEnum.DataTable;
							default: // Not Supported dataType
                                return DataManager.DataTypeEnum.NotSupported;
						}
					}
					else
					{
						// Return Default (String)
                        return DataManager.DataTypeEnum.String;
					}
				}
				#endregion

				#region DataTypeEnum ConvertDataType(int dataType)
				public DataManager.DataTypeEnum ConvertDataType(int dataType, int columnFlags)
				{
				
					switch(dataType)
					{
						case 2: // "integer":
                            return DataManager.DataTypeEnum.Integer;
						case 3: // "Integer"

							// If This Is An Autonumber field
							if(columnFlags == 90)
							{
                                return DataManager.DataTypeEnum.Autonumber;
							}
							else
							{
                                return DataManager.DataTypeEnum.Integer;
							}

						case 4: // Single
						case 5: // Double
                            return DataManager.DataTypeEnum.Double;
						case 6: // "currency":
                            return DataManager.DataTypeEnum.Currency;
						case 7: // "datetime":
                            return DataManager.DataTypeEnum.DateTime;
				        case 11:
                            return DataManager.DataTypeEnum.Boolean;
						case 130: // String
                            return DataManager.DataTypeEnum.String;
						default: // Not Supported
                            return DataManager.DataTypeEnum.NotSupported;
				
					}
				}
				#endregion

			#endregion

            #region CreateDecimalDataType()
            /// <summary>
            /// This method returns the format for a Sql Parameter
            /// if the dataType is Decimal and the Precision
            /// and Scale are set, else it returns an empty string.
            /// </summary>
            /// <returns></returns>
            public string CreateDecimalDataType()
            {
                // initial value
                string decimalString = "";
                
                // if this is a decimal
                if ((this.DataType == DataManager.DataTypeEnum.Decimal) && (this.Precision >= 0) && (this.Scale >= 0))
                {
                    // create the decimal string
                    decimalString = "Decimal(" + this.Precision.ToString() + "," + this.Scale.ToString() + ")";
                }
                
                // return value
                return decimalString;
            } 
            #endregion

			#region HasValue()
			/// <summary>
			/// Checks That The field Is Not Null And Has A Value
			/// </summary>
			/// <returns></returns>
			public bool HasValue()
			{
				// If This field IsNull Return False
				if(this.FieldValue == null)
				{
					// No Value
					return false;
				}

				// If The Length Of This field Is 0
				if(String.IsNullOrEmpty(this.FieldValue.ToString()))
				{
					// Not Updateable
					return false;
				}

				// If This Is A Numeric dataType
				if(DataJuggler.Net.DataRow.StaticIsNumericDataType(this.DataType))
				{
					// This Is A Number, If This Number = 0 Return False
					if(this.FieldValue.ToString() == "0")
					{
						// Return False
						return false;
					}
				}

				// return true
				return true;
			}
			#endregion

            #region ToString()
            /// <summary>
            /// This method shows the name when ToString is called.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                // Return the field name when ToString is called.
                return this.FieldName;
            } 
            #endregion

            #region UpdateFieldValue(string newValue, bool fieldHasChanges)
            /// <summary>
			/// Updates The fieldValue Without Setting Changes To True
			/// If FieldHasChanges = False
			/// </summary>
			/// <param name="NewValue"></param>
			/// <param name="fieldHasChanges"></param>
			public void UpdateFieldValue(string newValue, bool fieldHasChanges)
			{	
				// Prevent The Changes Property From Being Set To True
				if(!fieldHasChanges)
				{
                    // Set loading to true
					this.Loading = true;
				}

				// Set fieldValue
				this.FieldValue = newValue;

				// Set Loading Back To False
				this.Loading = false;
			}
			#endregion

		#endregion
		
		#region Properties

			#region AccessMode
			public DataManager.AccessMode AccessMode
			{
				get
				{
					return accessmode;
				}
				set
				{
					accessmode = value;
				}
			}
			#endregion

            #region Caption
			public string Caption
			{
				get
				{
					return caption;
				}
				set
				{
					caption = value;
				}
			}
			#endregion

			#region Changes
			public bool Changes
			{
				get
				{
					return changes;
				}
				set
				{
					if(!loading)
					{
						changes = value;
					}
				}
			}
			#endregion

			#region DataType
            public DataManager.DataTypeEnum DataType
			{
				get
				{
					return datatype;
				}
				set
				{
					datatype = value;
				}
			}
			#endregion

            #region DBDataType
            /// <summary>
            /// This property gets or sets the value for 'dbDataType'.
            /// </summary>
            public string DBDataType
            {
                get { return dbDataType; }
                set { dbDataType = value; }
            }
            #endregion
			
			#region DBFieldName
			public string DBFieldName
			{
				get
				{
					return dbfieldname;
				}
				set
				{
					dbfieldname = value;
				}
			}
			#endregion
			
			#region DecimalPlaces
			public int DecimalPlaces
			{
				get
				{
					return decimalplaces;
				}
				set
				{
					decimalplaces = value;
				}
			}
			#endregion

			#region DefaultValue
			public string DefaultValue
			{
				get
				{
					return defaultvalue;
				}
				set
				{
					defaultvalue = value;
				}
			}
			#endregion

            #region DoNotSetCaption
            /// <summary>
            /// This property gets or sets the value for 'DoNotSetCaption'.
            /// </summary>
            public bool DoNotSetCaption
            {
                get { return doNotSetCaption; }
                set { doNotSetCaption = value; }
            }
            #endregion

            #region EnumDataTypeName
            /// <summary>
            /// This property gets or sets the value for EnumDataTypeName
            /// </summary>
            public string EnumDataTypeName
            {
                get { return enumDataTypeName; }
                set { enumDataTypeName = value; }

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

            #region FieldId
            /// <summary>
            /// This field is not actually mapped to a database table. This is used to update the FieldId
            /// before converting to a DTNField, if an existing field was already saved with this name.
            /// This is only used when reopening an existing project. 
            /// </summary>
            public int FieldId
			{
				get
				{
                    // return the fieldId
					return fieldId;
				}
				set
				{
                    // store the value
					fieldId = value;
				}
			}
			#endregion

            #region FieldName
            public string FieldName
			{
				get
				{
					return fieldname;
				}
				set
				{
					// Capitalize First Character
					this.fieldname = DataJuggler.Net.DataTable.CapitalizeFirstChar(value);
					
					// Set DBFieldName
					this.dbfieldname = fieldname;
					
					// Now Replace fieldName
					this.fieldname = value.Replace(" ","");
					
					// Set Caption
					this.Caption = fieldname;
					
				}
			}
			#endregion

            #region FieldOrdinal
            public int FieldOrdinal
			{
				get
				{
					return fieldordinal;
				}
				set
				{
					fieldordinal = value;
				}
			}
			#endregion

            #region FieldSetName
            /// <summary>
            /// This field is used by DataTier.Net to pass in a fieldSetName to the ProcedureWriter.
            /// </summary>
			public string FieldSetName
			{
				get
				{
                    // return value
					return fieldSetName;
				}
				set
				{	
                    // set the value
					fieldSetName = value;
				}
			}
			#endregion

            #region FieldValue
            public object FieldValue
			{
				get
				{
					return fieldvalue;
				}
				set
				{
					fieldvalue = value;
				}
			}
			#endregion

			#region HasDefault
			public bool HasDefault
			{
				get
				{
					return hasdefault;
				}
				set
				{
					hasdefault = value;
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

            #region IsEnumeration
            /// <summary>
            /// This property gets or sets the value for IsEnumeration
            /// </summary>
            public bool IsEnumeration
            {
                get { return isEnumeration; }
                set { isEnumeration = value; }
            }
            #endregion

			#region isAutoIncrement
			public bool IsAutoIncrement
			{
				get
				{
					return isautoincrement;
				}
				set
				{
					isautoincrement = value;
				}
			}
			#endregion

			#region IsNullable
			public bool IsNullable
			{
				get
				{
					return isnullable;
				}
				set
				{
					isnullable = value;
				}
			}
			#endregion
			
			#region IsReadOnly
			public bool IsReadOnly
			{
				get
				{
					return isreadonly;
				}
				set
				{
					isreadonly = value;
				}
			}
			#endregion

			#region Loading
			public bool Loading	
			{
				get
				{
					return loading;
				}
				set
				{
					loading = value;
				}
			}
			#endregion

            #region Precision
            /// <summary>
            /// The total number of digits in this number.
            /// This is only used for Decimal data types
            /// </summary>
            public int Precision
            {
                get { return precision; }
                set { precision = value; }
            } 
            #endregion

			#region PrimaryKey
			public bool PrimaryKey
			{
				get
				{
					return primarykey;
				}
				set
				{
					primarykey = value;
				}
			}
			#endregion

			#region Required
			public bool Required
			{
				get
				{
					return required;
				}
				set
				{
					required = value;
				}
			}
			#endregion

            #region Scale
            /// <summary>
            /// This is the number of places after the decimal point.
            /// This value is only used for Decimal data type objects
            /// </summary>
            public int Scale
            {
                get { return scale; }
                set 
                { 
                    scale = value;

                    // The value for DecimalPlaces is the same as Scale
                    DecimalPlaces = value;
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

			#region Size
			public int Size
			{
				get
				{
					return size;
				}
				set
				{
					size = value;
				}
			}
	        #endregion

            #region TableId
            /// <summary>
            /// This is used only in DataTier.Net to make sure an existing tables value for Exclude is preserved
            /// when converting to and from a DTNField.
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

            #region TreatIntegerAsBoolean
            /// <summary>
            /// Set this property to true if an integer should be treated the same as a Boolean.
            /// Only 0 and 1 values will be allowed. This was created to set a field as a check
            /// box in the ScreenBuilder even though the field is an integer.
            /// </summary>
            public bool TreatIntegerAsBoolean
            {
                get { return this.treatIntegerAsBoolean; }
                set { this.treatIntegerAsBoolean = value; }
            } 
            #endregion
	   
	        #region Visible
	        /// <summary>
	        /// A property for storing if this field is visible
            /// when displayed in a DataClassGridColumn.
	        /// </summary>
			public bool Visible
			{
				get
				{
					return visible;
				}
				set
				{
					visible = value;
				}
			}
	        #endregion
			
		#endregion

	}
	#endregion	
    
}
