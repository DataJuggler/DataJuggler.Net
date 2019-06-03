

#region using statements

using DataJuggler.Core.UltimateHelper;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace DataJuggler.Net
{

    #region class ForeignKeyConstraintHelper
    /// <summary>
    /// This clas is used to make it simpler to take the string values for foreign key constraint info
    /// and create a ForeignKeyConstraint object
    /// </summary>
    public class ForeignKeyConstraintHelper
    {
        
        #region Methods
        
            #region FindForeignKey(string foreignKeyName, DataTable table)
            /// <summary>
            /// This method attempts to find a foreign key by name if it exists in the table given.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="table"></param>
            /// <returns></returns>
            public static ForeignKeyConstraint FindForeignKey(string foreignKeyName, DataTable table)
            {
                // initial value
                ForeignKeyConstraint foreignKey = null;

                // if the foreignKeyName exists and the table exists and the table has one or more fields
                if ((TextHelper.Exists(foreignKeyName)) && (NullHelper.Exists(table)) && (ListHelper.HasOneOrMoreItems(table.Fields)))
                {
                    // if the foreign keys exist for this table
                    if (NullHelper.Exists(table.ForeignKeys))
                    {
                        // return the foreignKey if it exists
                        foreignKey = table.ForeignKeys.FirstOrDefault(x => x.Name == foreignKeyName);
                    }
                }

                // return value
                return foreignKey;
            }
            #endregion

            #region FindForeignKeysForTable(DataTable table, List<ForeignKeyConstraint> allForeignKeys)
            /// <summary>
            /// This method is used to load all foreign keys for the table given
            /// </summary>
            /// <param name="table"></param>
            /// <param name="allForeignKeys"></param>
            /// <returns></returns>
            public static List<ForeignKeyConstraint> FindForeignKeysForTable(DataTable table, List<ForeignKeyConstraint> allForeignKeys)
            {
                // intial value
                List<ForeignKeyConstraint> keys = null;

                // If the keys object exists
                if ((NullHelper.Exists(table)) && (ListHelper.HasOneOrMoreItems(allForeignKeys)))
                {   
                    // look  up the keys for this table
                    keys = allForeignKeys.Where(x => x.Table == table.Name).ToList();
                }

                // return value
                return keys;
            }
            #endregion

        #endregion
        
    }
    #endregion

}
