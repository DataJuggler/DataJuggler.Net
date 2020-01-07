

#region using statements

using DataJuggler.Net;
using DataJuggler.Core.UltimateHelper;
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace DataJuggler.Net.Xml.Writers
{

    #region class StoredProcedureParametersWriter
    /// <summary>
    /// This class is used to export an instance or a list of 'StoredProcedureParameter' objects to xml.
    /// </summary>
    public class StoredProcedureParametersWriter
    {

        #region Methods

            #region ExportList(List<StoredProcedureParameter> storedProcedureParameters, int indent = 0)
            // <Summary>
            // This method is used to export a list of 'StoredProcedureParameter' objects to xml
            // </Summary>
            public string ExportList(List<StoredProcedureParameter> storedProcedureParameters, int indent = 0)
            {
                // initial value
                string xml = "";

                // locals
                string storedProcedureParametersXml = String.Empty;
                string indentString = TextHelper.Indent(indent);

                // Create a new instance of a StringBuilder object
                StringBuilder sb = new StringBuilder();

                // Add the indentString
                sb.Append(indentString);

                // Add the open StoredProcedureParameter Node
                sb.Append("<StoredProcedureParameters>");

                // Add a new line
                sb.Append(Environment.NewLine);

                // If there are one or more StoredProcedureParameter objects
                if ((storedProcedureParameters != null) && (storedProcedureParameters.Count > 0))
                {
                    // Iterate the storedProcedureParameters collection
                    foreach (StoredProcedureParameter storedProcedureParameter  in storedProcedureParameters)
                    {
                        // Get the xml for this storedProcedureParameters
                        storedProcedureParametersXml = ExportStoredProcedureParameter(storedProcedureParameter, indent + 2);

                        // If the storedProcedureParametersXml string exists
                        if (TextHelper.Exists(storedProcedureParametersXml))
                        {
                            // Add this storedProcedureParameters to the xml
                            sb.Append(storedProcedureParametersXml);
                        }
                    }
                }

                // Add the close StoredProcedureParametersWriter Node
                sb.Append(indentString);
                sb.Append("</StoredProcedureParameters>");

                // Set the return value
                xml = sb.ToString();

                // return value
                return xml;
            }
            #endregion

            #region ExportStoredProcedureParameter(StoredProcedureParameter storedProcedureParameter, int indent = 0)
            // <Summary>
            // This method is used to export a StoredProcedureParameter object to xml.
            // </Summary>
            public string ExportStoredProcedureParameter(StoredProcedureParameter storedProcedureParameter, int indent = 0)
            {
                // initial value
                string storedProcedureParameterXml = "";

                // locals
                string indentString = TextHelper.Indent(indent);
                string indentString2 = TextHelper.Indent(indent + 2);

                // If the storedProcedureParameter object exists
                if (NullHelper.Exists(storedProcedureParameter))
                {
                    // Create a StringBuilder
                    StringBuilder sb = new StringBuilder();

                    // Append the indentString
                    sb.Append(indentString);

                    // Write the open storedProcedureParameter node
                    sb.Append("<StoredProcedureParameter>" + Environment.NewLine);

                    // Write out each property

                    // Write out the value for DataType

                    sb.Append(indentString2);
                    sb.Append("<DataType>" + storedProcedureParameter.DataType + "</DataType>" + Environment.NewLine);

                    // Write out the value for Length

                    sb.Append(indentString2);
                    sb.Append("<Length>" + storedProcedureParameter.Length + "</Length>" + Environment.NewLine);

                    // Write out the value for ParameterName

                    sb.Append(indentString2);
                    sb.Append("<ParameterName>" + storedProcedureParameter.ParameterName + "</ParameterName>" + Environment.NewLine);

                    // Write out the value for ParameterValue

                    sb.Append(indentString2);
                    sb.Append("<ParameterValue>" + storedProcedureParameter.ParameterValue + "</ParameterValue>" + Environment.NewLine);

                    // Append the indentString
                    sb.Append(indentString);

                    // Write out the close storedProcedureParameter node
                    sb.Append("</StoredProcedureParameter>" + Environment.NewLine);

                    // set the return value
                    storedProcedureParameterXml = sb.ToString();
                }
                // return value
                return storedProcedureParameterXml;
            }
            #endregion

        #endregion

    }
    #endregion

}
