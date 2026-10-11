

#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace DataJuggler.Net
{

    #region class SqlObject
    /// <summary>
    /// This class is used to get AnsiSQL information about a View or Stored Procedure
    /// </summary>
    public class SqlObject
    {
        
        #region Private Variables
        private string defintion;
        private bool usesAnsiNulls;
        private bool usesQuotedIdentifier;
        private bool isSchemaBound;
        #endregion

        #region Properties

            #region Defintion
            /// <summary>
            /// This property gets or sets the value for 'Defintion'.
            /// </summary>
            public string Defintion
            {
                get { return defintion; }
                set { defintion = value; }
            }
            #endregion
            
            #region IsSchemaBound
            /// <summary>
            /// This property gets or sets the value for 'IsSchemaBound'.
            /// </summary>
            public bool IsSchemaBound
            {
                get { return isSchemaBound; }
                set { isSchemaBound = value; }
            }
            #endregion
            
            #region UsesAnsiNulls
            /// <summary>
            /// This property gets or sets the value for 'UsesAnsiNulls'.
            /// </summary>
            public bool UsesAnsiNulls
            {
                get { return usesAnsiNulls; }
                set { usesAnsiNulls = value; }
            }
            #endregion
            
            #region UsesQuotedIdentifier
            /// <summary>
            /// This property gets or sets the value for 'UsesQuotedIdentifier'.
            /// </summary>
            public bool UsesQuotedIdentifier
            {
                get { return usesQuotedIdentifier; }
                set { usesQuotedIdentifier = value; }
            }
            #endregion
            
        #endregion
        
    }
    #endregion

}
