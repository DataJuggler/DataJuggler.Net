

#region using statements

using System.Data;

#endregion

namespace DataJuggler.Net
{

    #region class DataHelper
    /// <summary>
    /// Thjis class is used to help with certain data functions
    /// </summary>
    public class DataHelper
    {

        #region Static Methods

            #region ReturnFirstRow(StoredProcedure storedProcedure)
            /// <summary>
            /// This method returns the first data row of the first data table
            /// in the data set passed in.
            /// </summary>
            /// <param name="dataSet"></param>
            /// <returns></returns>
            public static System.Data.DataRow ReturnFirstRow(DataSet dataSet)
            {
                // Initial Value
                System.Data.DataRow row = null;

                // if userDataSet exists
                if ((dataSet != null) && (dataSet.Tables != null) && (dataSet.Tables.Count > 0))
                {
                    // test for rows
                    if ((dataSet.Tables[0].Rows != null) && (dataSet.Tables[0].Rows.Count > 0))
                    {
                        // Create DataRow from data set
                        row = dataSet.Tables[0].Rows[0];
                    }
                }

                // Return Value
                return row;
            }
            #endregion

            #region ReturnFirstTable(DataSet dataSet)
            /// <summary>
            /// This method returns first data table
            /// in the data set passed in.
            /// </summary>
            /// <param name="dataSet">The 'DataSet' to return the first table of.</param>
            /// <returns>The first DataTable of the DataSet if it exists.</returns>
            public static System.Data.DataTable ReturnFirstTable(DataSet dataSet)
            {
                // Initial Value
                System.Data.DataTable table = null;

                // if userDataSet exists
                if ((dataSet != null) && (dataSet.Tables != null) && (dataSet.Tables.Count > 0))
                {
                    // Set table
                    table = dataSet.Tables[0];
                }

                // Return Value
                return table;
            }
            #endregion

        #endregion

    } 
    #endregion

}
