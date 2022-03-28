

#region using statements

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using DataJuggler.Net.Enumerations;

#endregion

namespace DataJuggler.Net
{

    #region class StoredProcedure
    [Serializable]
    public class StoredProcedure
    {

        #region Private Variables
        private string procedurename;
        private List<StoredProcedureParameter> parameters;
        private StoredProcedureTypes storedproceduretype;
        private List<DataField> returnSetSchema;
        private bool doesNotHaveParameters;
        private string text;
        #endregion

        #region Constructor
        public StoredProcedure()
        {
            // Create Parameters Collection
            this.Parameters = new List<StoredProcedureParameter>();
        }
        #endregion

        #region Methods

            #region HasNotSupportedParameterDataType()
            /// <summary>
            /// Returns True If This procedure 
            /// </summary>
            /// <returns></returns>
            public bool HasNotSupportedParameterDataType()
            {
                // Loop Through Each Parameter
                foreach (StoredProcedureParameter Param in this.Parameters)
                {
                    if (Param.DataType == DataManager.DataTypeEnum.NotSupported)
                    {
                        // Does Contain Not Supported dataType
                        return true;
                    }
                }

                // Does Not Contain Unsupported dataType
                return false;
            }
            #endregion

        #endregion
        
        #region Properties

            #region DoesNotHaveParameters
            /// <summary>
            /// This property gets or sets the value for 'DoesNotHaveParameters'.
            /// </summary>
            public bool DoesNotHaveParameters
            {
                get { return doesNotHaveParameters; }
                set { doesNotHaveParameters = value; }
            }
            #endregion
            
            #region HasParameters
            /// <summary>
            /// This read only property returns true if this 'StoredProcedure' has one or more parameters.
            /// </summary>
            public bool HasParameters
            {
                get
                {
                    // initial value
                    bool hasParameters = ((this.Parameters != null) && (this.Parameters.Count > 0));

                    // return value
                    return hasParameters;
                }
            } 
            #endregion

            #region HasReturnSetSchema
            /// <summary>
            /// This read only property returns true if this 'StoredProcedure' has a ReturnSetSchema object.
            /// </summary>
            public bool HasReturnSetSchema
            {
                get
                {
                    // initial value
                    bool hasReturnSetSchema = ((this.ReturnSetSchema != null) && (this.ReturnSetSchema.Count > 0));

                    // return value
                    return hasReturnSetSchema;
                }
            }
            #endregion

            #region Parameters
            public List<StoredProcedureParameter> Parameters
            {
                get
                {
                    return parameters;
                }
                set
                {
                    parameters = value;
                }
            }
            #endregion

            #region ProcedureName
            public string ProcedureName
            {
                get
                {
                    return procedurename;
                }
                set
                {
                    procedurename = value;
                }
            }
            #endregion

            #region ReturnSetSchema
            /// <summary>
            /// This property gets or sets the ReturnSetSchema.
            /// </summary>
            public List<DataField> ReturnSetSchema
            {
                get { return returnSetSchema; }
                set { returnSetSchema = value; }
            } 
            #endregion

            #region StoredProcedureType
            public StoredProcedureTypes StoredProcedureType
            {
                get
                {
                    return storedproceduretype;
                }
                set
                {
                    storedproceduretype = value;
                }
            }
            #endregion

            #region Text
            /// <summary>
            /// This property gets or sets the value for 'Text'.
            /// </summary>
            public string Text
            {
                get { return text; }
                set { text = value; }
            }
            #endregion
            
        #endregion

    }
    #endregion
    
}
