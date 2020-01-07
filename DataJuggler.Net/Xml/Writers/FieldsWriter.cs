

#region using statements

using DataJuggler.Net;
using DataJuggler.Core.UltimateHelper;
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace DataJuggler.Net.Xml.Writers
{

    #region class FieldsWriter
    /// <summary>
    /// This class is used to export an instance or a list of 'DataField' objects to xml.
    /// </summary>
    public class FieldsWriter
    {

        #region Methods

            #region ExportList(List<DataField> fields, int indent = 0)
            // <Summary>
            // This method is used to export a list of 'DataField' objects to xml
            // </Summary>
            public string ExportList(List<DataField> fields, int indent = 0)
            {
                // initial value
                string xml = "";

                // locals
                string fieldsXml = String.Empty;
                string indentString = TextHelper.Indent(indent);

                // Create a new instance of a StringBuilder object
                StringBuilder sb = new StringBuilder();

                // Add the indentString
                sb.Append(indentString);

                // Add the open DataField Node
                sb.Append("<Fields>");

                // Add a new line
                sb.Append(Environment.NewLine);

                // If there are one or more DataField objects
                if ((fields != null) && (fields.Count > 0))
                {
                    // Iterate the fields collection
                    foreach (DataField dataField  in fields)
                    {
                        // Get the xml for this fields
                        fieldsXml = ExportDataField(dataField, indent + 2);

                        // If the fieldsXml string exists
                        if (TextHelper.Exists(fieldsXml))
                        {
                            // Add this fields to the xml
                            sb.Append(fieldsXml);
                        }
                    }
                }

                // Add the close FieldsWriter Node
                sb.Append(indentString);
                sb.Append("</Fields>");

                // Set the return value
                xml = sb.ToString();

                // return value
                return xml;
            }
            #endregion

            #region ExportDataField(DataField dataField, int indent = 0)
            // <Summary>
            // This method is used to export a DataField object to xml.
            // </Summary>
            public string ExportDataField(DataField dataField, int indent = 0)
            {
                // initial value
                string dataFieldXml = "";

                // locals
                string indentString = TextHelper.Indent(indent);
                string indentString2 = TextHelper.Indent(indent + 2);

                // If the dataField object exists
                if (NullHelper.Exists(dataField))
                {
                    // Create a StringBuilder
                    StringBuilder sb = new StringBuilder();

                    // Append the indentString
                    sb.Append(indentString);

                    // Write the open dataField node
                    sb.Append("<DataField>" + Environment.NewLine);

                    // Write out each property

                    // Write out the value for AccessMode

                    sb.Append(indentString2);
                    sb.Append("<AccessMode>" + dataField.AccessMode + "</AccessMode>" + Environment.NewLine);

                    // Write out the value for Caption

                    sb.Append(indentString2);
                    sb.Append("<Caption>" + dataField.Caption + "</Caption>" + Environment.NewLine);

                    // Write out the value for Changes

                    sb.Append(indentString2);
                    sb.Append("<Changes>" + dataField.Changes + "</Changes>" + Environment.NewLine);

                    // Write out the value for DataType

                    sb.Append(indentString2);
                    sb.Append("<DataType>" + dataField.DataType + "</DataType>" + Environment.NewLine);

                    // Write out the value for DBDataType

                    sb.Append(indentString2);
                    sb.Append("<DBDataType>" + dataField.DBDataType + "</DBDataType>" + Environment.NewLine);

                    // Write out the value for DBFieldName

                    sb.Append(indentString2);
                    sb.Append("<DBFieldName>" + dataField.DBFieldName + "</DBFieldName>" + Environment.NewLine);

                    // Write out the value for DecimalPlaces

                    sb.Append(indentString2);
                    sb.Append("<DecimalPlaces>" + dataField.DecimalPlaces + "</DecimalPlaces>" + Environment.NewLine);

                    // Write out the value for DefaultValue

                    sb.Append(indentString2);
                    sb.Append("<DefaultValue>" + dataField.DefaultValue + "</DefaultValue>" + Environment.NewLine);

                    // Write out the value for DoNotSetCaption

                    sb.Append(indentString2);
                    sb.Append("<DoNotSetCaption>" + dataField.DoNotSetCaption + "</DoNotSetCaption>" + Environment.NewLine);

                    // Write out the value for EnumDataTypeName

                    sb.Append(indentString2);
                    sb.Append("<EnumDataTypeName>" + dataField.EnumDataTypeName + "</EnumDataTypeName>" + Environment.NewLine);

                    // Write out the value for Exclude

                    sb.Append(indentString2);
                    sb.Append("<Exclude>" + dataField.Exclude + "</Exclude>" + Environment.NewLine);

                    // Write out the value for FieldId

                    sb.Append(indentString2);
                    sb.Append("<FieldId>" + dataField.FieldId + "</FieldId>" + Environment.NewLine);

                    // Write out the value for FieldName

                    sb.Append(indentString2);
                    sb.Append("<FieldName>" + dataField.FieldName + "</FieldName>" + Environment.NewLine);

                    // Write out the value for FieldOrdinal

                    sb.Append(indentString2);
                    sb.Append("<FieldOrdinal>" + dataField.FieldOrdinal + "</FieldOrdinal>" + Environment.NewLine);

                    // Write out the value for FieldSetName

                    sb.Append(indentString2);
                    sb.Append("<FieldSetName>" + dataField.FieldSetName + "</FieldSetName>" + Environment.NewLine);

                    // Write out the value for FieldValue

                    sb.Append(indentString2);
                    sb.Append("<FieldValue>" + dataField.FieldValue + "</FieldValue>" + Environment.NewLine);

                    // Write out the value for HasDefault

                    sb.Append(indentString2);
                    sb.Append("<HasDefault>" + dataField.HasDefault + "</HasDefault>" + Environment.NewLine);

                    // Write out the value for Index

                    sb.Append(indentString2);
                    sb.Append("<Index>" + dataField.Index + "</Index>" + Environment.NewLine);

                    // Write out the value for IsAutoIncrement

                    sb.Append(indentString2);
                    sb.Append("<IsAutoIncrement>" + dataField.IsAutoIncrement + "</IsAutoIncrement>" + Environment.NewLine);

                    // Write out the value for IsEnumeration

                    sb.Append(indentString2);
                    sb.Append("<IsEnumeration>" + dataField.IsEnumeration + "</IsEnumeration>" + Environment.NewLine);

                    // Write out the value for IsNullable

                    sb.Append(indentString2);
                    sb.Append("<IsNullable>" + dataField.IsNullable + "</IsNullable>" + Environment.NewLine);

                    // Write out the value for IsReadOnly

                    sb.Append(indentString2);
                    sb.Append("<IsReadOnly>" + dataField.IsReadOnly + "</IsReadOnly>" + Environment.NewLine);

                    // Write out the value for Loading

                    sb.Append(indentString2);
                    sb.Append("<Loading>" + dataField.Loading + "</Loading>" + Environment.NewLine);

                    // Write out the value for Precision

                    sb.Append(indentString2);
                    sb.Append("<Precision>" + dataField.Precision + "</Precision>" + Environment.NewLine);

                    // Write out the value for PrimaryKey

                    sb.Append(indentString2);
                    sb.Append("<PrimaryKey>" + dataField.PrimaryKey + "</PrimaryKey>" + Environment.NewLine);

                    // Write out the value for Required

                    sb.Append(indentString2);
                    sb.Append("<Required>" + dataField.Required + "</Required>" + Environment.NewLine);

                    // Write out the value for Scale

                    sb.Append(indentString2);
                    sb.Append("<Scale>" + dataField.Scale + "</Scale>" + Environment.NewLine);

                    // Write out the value for Scope

                    sb.Append(indentString2);
                    sb.Append("<Scope>" + dataField.Scope + "</Scope>" + Environment.NewLine);

                    // Write out the value for Size

                    sb.Append(indentString2);
                    sb.Append("<Size>" + dataField.Size + "</Size>" + Environment.NewLine);

                    // Write out the value for TableId

                    sb.Append(indentString2);
                    sb.Append("<TableId>" + dataField.TableId + "</TableId>" + Environment.NewLine);

                    // Write out the value for TreatIntegerAsBoolean

                    sb.Append(indentString2);
                    sb.Append("<TreatIntegerAsBoolean>" + dataField.TreatIntegerAsBoolean + "</TreatIntegerAsBoolean>" + Environment.NewLine);

                    // Write out the value for Visible

                    sb.Append(indentString2);
                    sb.Append("<Visible>" + dataField.Visible + "</Visible>" + Environment.NewLine);

                    // Append the indentString
                    sb.Append(indentString);

                    // Write out the close dataField node
                    sb.Append("</DataField>" + Environment.NewLine);

                    // set the return value
                    dataFieldXml = sb.ToString();
                }
                // return value
                return dataFieldXml;
            }
            #endregion

        #endregion

    }
    #endregion

}
