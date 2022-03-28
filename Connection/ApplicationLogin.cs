

#region using statements

using System;

#endregion

namespace DataJuggler.Net.Connection
{

    #region class ApplicationLogin
    /// <summary>
    /// This class represents a Login for an application.
    /// </summary>
    public class ApplicationLogin
    {
        
        #region Private Variables
        private string userName;
        private string password;
        private bool savePassword;
        #endregion
        
        #region Constructors
            
            #region Default Constructor
            /// <summary>
            /// Create a new instance of an ApplicationLogin object.
            /// </summary>
            public ApplicationLogin()
            {
                
            }
            #endregion
            
            #region Parameterized Constructor
            /// <summary>
            /// Create a new instance of an ApplicationLogin object and set the properties.
            /// </summary>
            public ApplicationLogin(string userName, string password)
            {
                // set the values
                this.UserName = userName;
                this.Password = password;
                this.SavePassword = false;  
            }
            #endregion
            
            #region Constructor
            /// <summary>
            /// Create a new instance of an ApplicationLogin object.
            /// </summary>
            public ApplicationLogin(string userName, string password, bool savePassword)
            {
                // set the values
                this.UserName = userName;
                this.Password = password;
                this.SavePassword = savePassword;  
            }
            #endregion
            
        #endregion

        #region Methods

            #region Validate()
            /// <summary>
            /// This method is used to validate this login.
            /// </summary>
            public bool Validate()
            {
                // initial value
                bool isValid = false;

                // if the SavePassword is true
                if (this.SavePassword)
                {
                    // set the value to include username and pasword
                    isValid = ((this.HasUserName) && (this.HasPassword));
                }
                else
                {
                    // here we just check the user name
                    isValid = this.HasUserName;
                }

                // return value
                return isValid;
            }
            #endregion
            
        #endregion
        
        #region Properties
            
            #region HasPassword
            /// <summary>
            /// This property returns true if the 'Password' exists.
            /// </summary>
            public bool HasPassword
            {
                get
                {
                    // initial value
                    bool hasPassword = (!String.IsNullOrEmpty(this.Password));
                    
                    // return value
                    return hasPassword;
                }
            }
            #endregion
            
            #region HasUserName
            /// <summary>
            /// This property returns true if the 'UserName' exists.
            /// </summary>
            public bool HasUserName
            {
                get
                {
                    // initial value
                    bool hasUserName = (!String.IsNullOrEmpty(this.UserName));
                    
                    // return value
                    return hasUserName;
                }
            }
            #endregion
            
            #region Password
            /// <summary>
            /// This property gets or sets the value for 'Password'.
            /// </summary>
            public string Password
            {
                get { return password; }
                set { password = value; }
            }
            #endregion
            
            #region SavePassword
            /// <summary>
            /// This property gets or sets the value for 'SavePassword'.
            /// </summary>
            public bool SavePassword
            {
                get { return savePassword; }
                set { savePassword = value; }
            }
            #endregion
            
            #region UserName
            /// <summary>
            /// This property gets or sets the value for 'UserName'.
            /// </summary>
            public string UserName
            {
                get { return userName; }
                set { userName = value; }
            }
            #endregion
            
        #endregion
        
    }
    #endregion

}
