

#region using statements

using DataJuggler.Net;
using DataJuggler.Core.UltimateHelper;
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace DataJuggler.Net.Xml.Writers
{

    #region class DataTablesWriter
    /// <summary>
    /// This class is used to export an instance or a list of 'DataTable' objects to xml.
    /// </summary>
    public class DataTablesWriter
    {

        #region Methods

            #region ExportList(List<DataTable> dataTables, int indent = 0)
            // <Summary>
            // This method is used to export a list of 'DataTable' objects to xml
            // </Summary>
            public string ExportList(List<DataTable> dataTables, int indent = 0)
            {
                // initial value
                string xml = "";

                // locals
                string dataTablesXml = String.Empty;
                string indentString = TextHelper.Indent(indent);

                // Create a new instance of a StringBuilder object
                StringBuilder sb = new StringBuilder();

                // Add the indentString
                sb.Append(indentString);

                // Add the open DataTable Node
                sb.Append("<DataTables>");

                // Add a new line
                sb.Append(Environment.NewLine);

                // If there are one or more DataTable objects
                if ((dataTables != null) && (dataTables.Count > 0))
                {
                    // Iterate the dataTables collection
                    foreach (DataTable dataTable  in dataTables)
                    {
                        // Get the xml for this dataTables
                        dataTablesXml = ExportDataTable(dataTable, indent + 2);

                        // If the dataTablesXml string exists
                        if (TextHelper.Exists(dataTablesXml))
                        {
                            // Add this dataTables to the xml
                            sb.Append(dataTablesXml);
                        }
                    }
                }

                // Add the close DataTablesWriter Node
                sb.Append(indentString);
                sb.Append("</DataTables>");

                // Set the return value
                xml = sb.ToString();

                // return value
                return xml;
            }
            #endregion

            #region ExportDataTable(DataTable dataTable, int indent = 0)
            // <Summary>
            // This method is used to export a DataTable object to xml.
            // </Summary>
            public string ExportDataTable(DataTable dataTable, int indent = 0)
            {
                // initial value
                string dataTableXml = "";

                // locals
                string indentString = TextHelper.Indent(indent);
                string indentString2 = TextHelper.Indent(indent + 2);

                // If the dataTable object exists
                if (NullHelper.Exists(dataTable))
                {
                    // Create a StringBuilder
                    StringBuilder sb = new StringBuilder();

                    // Append the indentString
                    sb.Append(indentString);

                    // Write the open dataTable node
                    sb.Append("<DataTable>" + Environment.NewLine);

                    // Write out each property

                    // Write out the value for Changes

                    sb.Append(indentString2);
                    sb.Append("<Changes>" + dataTable.Changes + "</Changes>" + Environment.NewLine);

                    // Write out the value for CheckConstraints

                    // Create the CheckConstraintsWriter
                    CheckConstraintsWriter checkConstraintsWriter = new CheckConstraintsWriter();

                    // Export the CheckConstraints collection to xml
                    string checkConstraintXml = checkConstraintsWriter.ExportList(dataTable.CheckConstraints, indent + 2);
                    sb.Append(checkConstraintXml);
                    sb.Append(Environment.NewLine);

                    // Write out the value for ClassFileName

                    sb.Append(indentString2);
                    sb.Append("<ClassFileName>" + dataTable.ClassFileName + "</ClassFileName>" + Environment.NewLine);

                    // Write out the value for ClassName

                    sb.Append(indentString2);
                    sb.Append("<ClassName>" + dataTable.ClassName + "</ClassName>" + Environment.NewLine);

                    // Write out the value for ConnectionString

                    sb.Append(indentString2);
                    sb.Append("<ConnectionString>" + dataTable.ConnectionString + "</ConnectionString>" + Environment.NewLine);

                    // Write out the value for ContainsBinaryData

                    sb.Append(indentString2);
                    sb.Append("<ContainsBinaryData>" + dataTable.ContainsBinaryData + "</ContainsBinaryData>" + Environment.NewLine);

                    // Write out the value for CreateCollectionClass

                    sb.Append(indentString2);
                    sb.Append("<CreateCollectionClass>" + dataTable.CreateCollectionClass + "</CreateCollectionClass>" + Environment.NewLine);

                    // Write out the value for Exclude

                    sb.Append(indentString2);
                    sb.Append("<Exclude>" + dataTable.Exclude + "</Exclude>" + Environment.NewLine);

                    // Write out the value for Fields

                    // Create the FieldsWriter
                    FieldsWriter fieldsWriter = new FieldsWriter();

                    // Export the Fields collection to xml
                    string dataFieldXml = fieldsWriter.ExportList(dataTable.Fields, indent + 2);
                    sb.Append(dataFieldXml);
                    sb.Append(Environment.NewLine);

                    // Write out the value for ForeignKeys

                    // Create the ForeignKeysWriter
                    ForeignKeyConstraintsWriter foreignKeysWriter = new ForeignKeyConstraintsWriter();

                    // Export the ForeignKeys collection to xml
                    string foreignKeyConstraintXml = foreignKeysWriter.ExportList(dataTable.ForeignKeys, indent + 2);
                    sb.Append(foreignKeyConstraintXml);
                    sb.Append(Environment.NewLine);

                    // Write out the value for HasCheckConstraints

                    sb.Append(indentString2);
                    sb.Append("<HasCheckConstraints>" + dataTable.HasCheckConstraints + "</HasCheckConstraints>" + Environment.NewLine);

                    // Write out the value for HasIndexes

                    sb.Append(indentString2);
                    sb.Append("<HasIndexes>" + dataTable.HasIndexes + "</HasIndexes>" + Environment.NewLine);

                    // Write out the value for HasParentDatabase

                    sb.Append(indentString2);
                    sb.Append("<HasParentDatabase>" + dataTable.HasParentDatabase + "</HasParentDatabase>" + Environment.NewLine);

                    // Write out the value for HasPrimaryKey

                    sb.Append(indentString2);
                    sb.Append("<HasPrimaryKey>" + dataTable.HasPrimaryKey + "</HasPrimaryKey>" + Environment.NewLine);

                    // Write out the value for HasSchemaName

                    sb.Append(indentString2);
                    sb.Append("<HasSchemaName>" + dataTable.HasSchemaName + "</HasSchemaName>" + Environment.NewLine);

                    // Write out the value for Indexes

                    // Create the IndexesWriter
                    IndexesWriter indexesWriter = new IndexesWriter();

                    // Export the Indexes collection to xml
                    string dataIndexXml = indexesWriter.ExportList(dataTable.Indexes, indent + 2);
                    sb.Append(dataIndexXml);
                    sb.Append(Environment.NewLine);

                    // Write out the value for IsView

                    sb.Append(indentString2);
                    sb.Append("<IsView>" + dataTable.IsView + "</IsView>" + Environment.NewLine);

                    // Write out the value for Name

                    sb.Append(indentString2);
                    sb.Append("<Name>" + dataTable.Name + "</Name>" + Environment.NewLine);

                    // Write out the value for ObjectNameSpaceName

                    sb.Append(indentString2);
                    sb.Append("<ObjectNameSpaceName>" + dataTable.ObjectNameSpaceName + "</ObjectNameSpaceName>" + Environment.NewLine);

                    // Write out the value for ParentDatabase

                    sb.Append(indentString2);
                    sb.Append("<ParentDatabase>" + dataTable.ParentDatabase + "</ParentDatabase>" + Environment.NewLine);

                    // Write out the value for PrimaryKey

                    sb.Append(indentString2);
                    sb.Append("<PrimaryKey>" + dataTable.PrimaryKey + "</PrimaryKey>" + Environment.NewLine);

                    // Write out the value for Rows

                    // Create the DataRowsWriter
                    DataRowsWriter rowsWriter = new DataRowsWriter();

                    // Export the Rows collection to xml
                    string dataRowXml = rowsWriter.ExportList(dataTable.Rows, indent + 2);
                    sb.Append(dataRowXml);
                    sb.Append(Environment.NewLine);

                    // Write out the value for SchemaName

                    sb.Append(indentString2);
                    sb.Append("<SchemaName>" + dataTable.SchemaName + "</SchemaName>" + Environment.NewLine);

                    // Write out the value for Scope

                    sb.Append(indentString2);
                    sb.Append("<Scope>" + dataTable.Scope + "</Scope>" + Environment.NewLine);

                    // Write out the value for Serializable

                    sb.Append(indentString2);
                    sb.Append("<Serializable>" + dataTable.Serializable + "</Serializable>" + Environment.NewLine);

                    // Write out the value for SQLGenerator

                    sb.Append(indentString2);
                    sb.Append("<SQLGenerator>" + dataTable.SQLGenerator + "</SQLGenerator>" + Environment.NewLine);

                    // Write out the value for TableId

                    sb.Append(indentString2);
                    sb.Append("<TableId>" + dataTable.TableId + "</TableId>" + Environment.NewLine);

                    // Write out the value for XmlFileName

                    sb.Append(indentString2);
                    sb.Append("<XmlFileName>" + dataTable.XmlFileName + "</XmlFileName>" + Environment.NewLine);

                    // Append the indentString
                    sb.Append(indentString);

                    // Write out the close dataTable node
                    sb.Append("</DataTable>" + Environment.NewLine);

                    // set the return value
                    dataTableXml = sb.ToString();
                }
                // return value
                return dataTableXml;
            }
            #endregion

        #endregion

    }
    #endregion

}
