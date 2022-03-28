

#region using statements

using System.Collections.Generic;
using System;

#endregion

namespace DataJuggler.Net
{

    #region class StoredProcedureParameter
    [Serializable]
	public class StoredProcedureParameter
	{
	
		#region Private Variables
		private string parametername;
		private DataManager.DataTypeEnum datatype;
		private object parametervalue;
        private int length;
		#endregion
		
		#region Constructor
		public StoredProcedureParameter()
		{
		}
		#endregion
		
		#region Properties
		
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

            #region Length
            /// <summary>
            /// This property gets or sets the size of this parameter
            /// </summary>
            public int Length
            {
                get { return length; }
                set { length = value; }
            } 
            #endregion
	
			#region ParameterName
			public string ParameterName
			{
				get
				{
					return parametername;
				}
				set
				{
					parametername = value;
				}
			}
			#endregion
			
			#region ParameterValue
			public object ParameterValue
			{
				get
				{
					return parametervalue;
				}
				set
				{
					parametervalue = value;
				}
			}
			#endregion
		
		#endregion	
	
	}
	#endregion
	
}
