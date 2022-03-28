

#region using statements

using DataJuggler.Net;
using DataJuggler.Core.UltimateHelper;
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace DataJuggler.Net.Xml.Writers
{

    #region class DatabasesWriter
    /// <summary>
    /// This class is used to export an instance or a list of 'Database' objects to xml.
    /// </summary>
    public class DatabasesWriter
    {

        #region Methods

            #region ExportList(List<Database> databases, int indent = 0)
            // <Summary>
            // This method is used to export a list of 'Database' objects to xml
            // </Summary>
            public string ExportList(List<Database> databases, int indent = 0)
            {
                // initial value
                string xml = "";

                // locals
                string databasesXml = String.Empty;
                string indentString = TextHelper.Indent(indent);

                // Create a new instance of a StringBuilder object
                StringBuilder sb = new StringBuilder();

                // Add the indentString
                sb.Append(indentString);

                // Add the open Database Node
                sb.Append("<Databases>");

                // Add a new line
                sb.Append(Environment.NewLine);

                // If there are one or more Database objects
                if ((databases != null) && (databases.Count > 0))
                {
                    // Iterate the databases collection
                    foreach (Database database  in databases)
                    {
                        // Get the xml for this databases
                        databasesXml = ExportDatabase(database, indent + 2);

                        // If the databasesXml string exists
                        if (TextHelper.Exists(databasesXml))
                        {
                            // Add this databases to the xml
                            sb.Append(databasesXml);
                        }
                    }
                }

                // Add the close DatabasesWriter Node
                sb.Append(indentString);
                sb.Append("</Databases>");

                // Set the return value
                xml = sb.ToString();

                // return value
                return xml;
            }
            #endregion

            #region ExportDatabase(Database database, int indent = 0)
            // <Summary>
            // This method is used to export a Database object to xml.
            // </Summary>
            public string ExportDatabase(Database database, int indent = 0)
            {
                // initial value
                string databaseXml = "";

                // locals
                string indentString = TextHelper.Indent(indent);
                string indentString2 = TextHelper.Indent(indent + 2);

                // If the database object exists
                if (NullHelper.Exists(database))
                {
                    // Create a StringBuilder
                    StringBuilder sb = new StringBuilder();

                    // Append the indentString
                    sb.Append(indentString);

                    // Write the open database node
                    sb.Append("<Database>" + Environment.NewLine);

                    // Write out each property

                    // Write out the value for ClassFileName

                    sb.Append(indentString2);
                    sb.Append("<ClassFileName>" + database.ClassFileName + "</ClassFileName>" + Environment.NewLine);

                    // Write out the value for ClassName

                    sb.Append(indentString2);
                    sb.Append("<ClassName>" + database.ClassName + "</ClassName>" + Environment.NewLine);

                    // Write out the value for ConnectionString

                    sb.Append(indentString2);
                    sb.Append("<ConnectionString>" + database.ConnectionString + "</ConnectionString>" + Environment.NewLine);

                    // Write out the value for Exclude

                    sb.Append(indentString2);
                    sb.Append("<Exclude>" + database.Exclude + "</Exclude>" + Environment.NewLine);

                    // Write out the value for Functions

                    // Create the FunctionsWriter
                    FunctionsWriter functionsWriter = new FunctionsWriter();

                    // Export the Functions collection to xml
                    string functionXml = functionsWriter.ExportList(database.Functions, indent + 2);
                    sb.Append(functionXml);
                    sb.Append(Environment.NewLine);

                    // Write out the value for HasFunctions

                    sb.Append(indentString2);
                    sb.Append("<HasFunctions>" + database.HasFunctions + "</HasFunctions>" + Environment.NewLine);

                    // Write out the value for HasOneOrMoreFunctions

                    sb.Append(indentString2);
                    sb.Append("<HasOneOrMoreFunctions>" + database.HasOneOrMoreFunctions + "</HasOneOrMoreFunctions>" + Environment.NewLine);

                    // Write out the value for HasOneOrMoreStoredProcedures

                    sb.Append(indentString2);
                    sb.Append("<HasOneOrMoreStoredProcedures>" + database.HasOneOrMoreStoredProcedures + "</HasOneOrMoreStoredProcedures>" + Environment.NewLine);

                    // Write out the value for HasOneOrMoreTables

                    sb.Append(indentString2);
                    sb.Append("<HasOneOrMoreTables>" + database.HasOneOrMoreTables + "</HasOneOrMoreTables>" + Environment.NewLine);

                    // Write out the value for HasStoredProcedures

                    sb.Append(indentString2);
                    sb.Append("<HasStoredProcedures>" + database.HasStoredProcedures + "</HasStoredProcedures>" + Environment.NewLine);

                    // Write out the value for HasTables

                    sb.Append(indentString2);
                    sb.Append("<HasTables>" + database.HasTables + "</HasTables>" + Environment.NewLine);

                    // Write out the value for Name

                    sb.Append(indentString2);
                    sb.Append("<Name>" + database.Name + "</Name>" + Environment.NewLine);

                    // Write out the value for ParentDataManager

                    sb.Append(indentString2);
                    sb.Append("<ParentDataManager>" + database.ParentDataManager + "</ParentDataManager>" + Environment.NewLine);

                    // Write out the value for Password

                    sb.Append(indentString2);
                    sb.Append("<Password>" + database.Password + "</Password>" + Environment.NewLine);

                    // Write out the value for Path

                    sb.Append(indentString2);
                    sb.Append("<Path>" + database.Path + "</Path>" + Environment.NewLine);

                    // Write out the value for Serializable

                    sb.Append(indentString2);
                    sb.Append("<Serializable>" + database.Serializable + "</Serializable>" + Environment.NewLine);

                    // Write out the value for StoredProcedures

                    // Create the StoredProceduresWriter
                    StoredProceduresWriter storedProceduresWriter = new StoredProceduresWriter();

                    // Export the StoredProcedures collection to xml
                    string storedProcedureXml = storedProceduresWriter.ExportList(database.StoredProcedures, indent + 2);
                    sb.Append(storedProcedureXml);
                    sb.Append(Environment.NewLine);

                    // Write out the value for Tables

                    // Create the TablesWriter
                    DataTablesWriter tablesWriter = new DataTablesWriter();

                    // Export the Tables collection to xml
                    string dataTableXml = tablesWriter.ExportList(database.Tables, indent + 2);
                    sb.Append(dataTableXml);
                    sb.Append(Environment.NewLine);

                    // Write out the value for XmlFileName

                    sb.Append(indentString2);
                    sb.Append("<XmlFileName>" + database.XmlFileName + "</XmlFileName>" + Environment.NewLine);

                    // Append the indentString
                    sb.Append(indentString);

                    // Write out the close database node
                    sb.Append("</Database>" + Environment.NewLine);

                    // set the return value
                    databaseXml = sb.ToString();
                }
                // return value
                return databaseXml;
            }
            #endregion

        #endregion

    }
    #endregion

}
