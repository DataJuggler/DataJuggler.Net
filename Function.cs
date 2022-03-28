

#region using statements

using DataJuggler.Net.Enumerations;

#endregion

namespace DataJuggler.Net
{

    #region class Function
    /// <summary>
    /// This class represents a Sql Function
    /// </summary>
    public class Function
    {
        
        #region Private Variables
        private string name;
        private string text;
        private FunctionTypeEnum functionType;
        #endregion

        #region Constructors

            #region Default Constructor
            /// <summary>
            /// Create a new instance of a 'Function' object.
            /// </summary>
            public Function()
            {
                
            }
            #endregion

            #region Parameterized Constructor(string name, string text, FunctionTypeEnum functionType)
            /// <summary>
            /// Create a new instance of a Function object and set the values for Name, Text and FunctionType.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="text"></param>
            /// <param name="functionType"></param>
            public Function(string name, string text, FunctionTypeEnum functionType)
            {
                // Set the value for Name
                this.Name = name;

                // Set the value for Text
                this.Text = text;

                // Set the value for FunctionType
                this.FunctionType = functionType;
            }
            #endregion

        #endregion

        #region Methods

            #region ToString()
            /// <summary>
            /// This method returns the String
            /// </summary>
            public override string ToString()
            {
                // return the name of this function
                return this.Name;
            }
            #endregion
            
        #endregion

        #region Properties

            #region FunctionType
            /// <summary>
            /// This property gets or sets the value for 'FunctionType'.
            /// </summary>
            public FunctionTypeEnum FunctionType
            {
                get { return functionType; }
                set { functionType = value; }
            }
            #endregion
            
            #region Name
            /// <summary>
            /// This property gets or sets the value for 'Name'.
            /// </summary>
            public string Name
            {
                get { return name; }
                set { name = value; }
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
