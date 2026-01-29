 
 
#region using statements

using System.Text;
using System.Collections.Generic;

#endregion

namespace DataJuggler.Net
{

	#region Class DataManager
	/// <summary>
	/// DataManager Object Used For DataJuggler.Net
	/// </summary>
	public class DataManager
	{

		#region Private Variables
		private string classfilename;
		private DataManager.ClassOutputLanguage classlanguage;
		private DataManager.FileOptions classfileoptions;
		private string classname;
		private List<Database> databases;
		private bool exclude;
		private string namespacename;
		private string projectfolder;
		private string projectname;
		private ReferencesSet references;
		private bool serializable;
		private string xmlfilename;
		private DataManager.FileOptions xmlfileoptions;
        private SQLDatabaseConnector dataConnector;
		#endregion

		#region Constructor
		
			#region DataManager()
			public DataManager()
			{
				// Create Databases Collect
				this.databases = new List<Database>();

				// Create References Collection
				this.references = new ReferencesSet();

				// Set FileOptions Default
				this.ClassFileOptions = DataManager.FileOptions.SingleFile;

			}
			#endregion
		
			#region DataManager(string projectFolder, string projectName, DataManager.ClassOutputLanguage OutputLanguage)
			public DataManager(string projectFolder, string projectName, DataManager.ClassOutputLanguage OutputLanguage)
			{
				// Set ClassLanguage
				this.ClassLanguage = OutputLanguage;

				// Create Databases
				this.databases = new List<Database>();

				// Create References Collection
				this.references = new ReferencesSet();

				// Set FileOptions Default
				this.ClassFileOptions = DataManager.FileOptions.SingleFile;

				// Set ProjectFolder, ClassFileName & xmlFileName
				this.ProjectFolder = projectFolder;
				this.ProjectName = projectName;
			}
			#endregion
			
		#endregion

		#region Methods

            #region ReturnFullPath(string fileName)
            /// <summary>
            /// Appends the fileName passed in to the ProjectFolder
			/// </summary>
			/// <param name="FileName"></param>
			/// <returns></returns>
			public string ReturnFullPath(string fileName)
			{
				// Create StringBuilder To Build Path
				StringBuilder sb = new StringBuilder(this.ProjectFolder);
				
				// if the project folder does not end with a backslash
				if(!this.ProjectFolder.EndsWith(@"\"))
				{
				    // append backslash
				    sb.Append(@"\");
				}

				// Append FileName
				sb.Append(fileName);

				// Return FullPath
				return sb.ToString();

			}
			#endregion

			#region AppendExtension(string fileName, string extension)
			/// <summary>
			/// Appends the Extension passed in to the FileName passed in
			/// </summary>
			/// <param name="FileName"></param>
			/// <param name="Extension"></param>
			/// <returns></returns>
			public string AppendExtension(string fileName, string extension)
			{
				// Create StringBuilder To Build Path
				StringBuilder sb = new StringBuilder(fileName);

				// Check If Period Was Included In Extension
				int period = extension.IndexOf(".");
				if(period < 0)
				{
					// Append Period
					sb.Append(".");
				}

				// Append FileName
				sb.Append(extension);

				// Return FullPath
				return sb.ToString();
			}
			#endregion

            #region AppendText(string sourceText, string appendText)
            /// <summary>
			/// Appends text to the string passed in 
			/// </summary>
			/// <param name="SourceText"></param>
			/// <param name="AppendText"></param>
			/// <returns></returns>
			public string AppendText(string sourceText, string appendText)
			{
				// Create StringBuilder To Return Text
				StringBuilder sb = new StringBuilder(sourceText);

				// Append AppendText
				sb.Append(appendText);

				// Return Appended String
				return sb.ToString();

			}
			#endregion

			#region ConvertScope() +1 override

				#region public static DataManager.Scope ConvertScope(string scope)
				public static DataManager.Scope ConvertScope(string scope)
				{
					if(scope != null)
					{
						switch(scope.ToLower())
						{
							case "private":
								return DataManager.Scope.Private;
							case "internal":
								return DataManager.Scope.Internal;
							case "protected":
								return DataManager.Scope.Protected;	
							default: // Public
								return DataManager.Scope.Public;
						}
					}
					else
					{
						// Return Default Public
						return DataManager.Scope.Public;
					}
				}
				#endregion

                #region public static string ConvertScope(DataManager.Scope scope)
                public static string ConvertScope(DataManager.Scope scope)
				{
					switch(scope)
					{
						case DataManager.Scope.Private:
							return "private";
						case DataManager.Scope.Internal:
							return "internal";
						case DataManager.Scope.Protected:
							return "protected";	
						default: // Public
							return "public";
					}
				}
				#endregion

			#endregion

            #region IsSupported(DataManager.DataTypeEnum dataType)
            public static bool IsSupported(DataManager.DataTypeEnum dataType)
			{
                // initial value
                bool isSupported = false;

				switch(dataType)
				{
					case DataTypeEnum.Autonumber:
					case DataTypeEnum.Currency:
					case DataTypeEnum.Double:
					case DataTypeEnum.String:
					case DataTypeEnum.Integer:
					case DataTypeEnum.DateTime:
			        case DataTypeEnum.Boolean:
			        case DataTypeEnum.Guid:
					
					// Added Object for supporting custom code generations (outside of DataTier.Net)
					case DataTypeEnum.Object:

                        // these fields are supported
                        isSupported = true;

                        // required
                        break;
				}

                // return value
                return isSupported;
			}
			#endregion

			#region Extension
			public string Extension()
			{
				switch(this.ClassLanguage)
				{
					case DataManager.ClassOutputLanguage.VBNet:
						return ".vb";
					case DataManager.ClassOutputLanguage.CSharp:
						return ".cs";
					default:
						return "void";
				}
			}
			#endregion 

            #region ValidPath(string projectFolder)
            public bool ValidPath(string projectFolder)
			{
				return System.IO.Directory.Exists(projectFolder);
			}
			#endregion

		#endregion
		
		#region Properties

			#region ClassName
			public string ClassName
			{
				get
				{
					return classname;
				}
				set
				{
					classname = value;
				}
			}
			#endregion	

			#region ClassFileName
			public string ClassFileName
			{
				get
				{
					return classfilename;
				}
				set
				{
					classfilename = value;
				}
			}
			#endregion

			#region ClassFileOptions
			public DataManager.FileOptions ClassFileOptions
			{
				get
				{
					return classfileoptions;
				}
				set
				{
					classfileoptions = value;
				}
			}
			#endregion	

			#region Databases
			public List<Database> Databases
			{
				get
				{
					return databases;
				}
				set
				{
					databases = value;
				}
			}
			#endregion

            #region DataConnector
            /// <summary>
            /// This property gets or sets teh value for DataConnector.
            /// </summary>
            public SQLDatabaseConnector DataConnector
            {
                get { return dataConnector; }
                set { dataConnector = value; }
            } 
            #endregion

			#region Exclude
			public bool Exclude
			{
				get
				{
					return exclude;
				}
				set
				{
					exclude = value;
				}
			}
			#endregion	
			
			#region NamespaceName
			public string NamespaceName
			{
				get
				{
					return namespacename;
				}
				set
				{
					namespacename = value;
				}
			}
			#endregion
			
			#region ProjectFolder
			public string ProjectFolder
			{
				get
				{
					return projectfolder;
				}
				set
				{
					projectfolder = value;
				}
			}
			#endregion

			#region ProjectName
			public string ProjectName
			{
				get
				{
					return projectname;
				}
				set
				{
					projectname = value;

					// Derive ClassFileName & xmlFileName From ProjectName
					this.ClassFileName = this.AppendExtension(projectname, this.Extension());
					this.ClassName = this.AppendText(this.ProjectName,"DataManager");
					this.XmlFileName = this.AppendExtension(projectname, ".xml");

				}
			}
			#endregion

			#region References
			public ReferencesSet References
			{
				get
				{
					return references;
				}
				set
				{
					references = value;
				}
			}
			#endregion	

			#region Serializable
			public bool Serializable
			{
				get
				{
					return serializable;
				}
				set
				{
					serializable = value;
				}
			}
			#endregion

			#region xmlFileName
			public string XmlFileName
			{
				get
				{
					return xmlfilename;
				}
				set
				{
					xmlfilename = value;
				}
			}
			#endregion

			#region XmlFileOptions	
			public DataManager.FileOptions XmlFileOptions
			{
				get
				{
					return xmlfileoptions;
				}
				set
				{
					xmlfileoptions = value;
				}
			}
			#endregion	

			#region ClassLanguage	
			public DataManager.ClassOutputLanguage ClassLanguage
			{
				get
				{
					return classlanguage;
				}
				set
				{
					classlanguage = value;
				}
			}
        #endregion

        #endregion

        #region Enumerations

        #region Enum AccessMode
        public enum AccessMode : int
        {
            ReadOnly = 0,
            WriteOnly = 1,
            ReadWrite = 2
        }
        #endregion

        #region Enum ClassReadingMode
        public enum ClassReadingMode : int
        {
            NotSet = 0,
            PrivateVariablesMode = 1,
            ConstructorsMode = 2,
            MethodsMode = 3,
            PropertiesMode = 4
        }
        #endregion

        #region Enum ClassOutputLanguage
        public enum ClassOutputLanguage : int
        {
            NotSet = 0,
            CSharp = 1,
            VBNet = 2
        }
        #endregion

        #region Enum DataTypeEnum
        public enum DataTypeEnum : int
        {
            NotSupported = 0,
            Object = 1,
            Autonumber = 3,
            Currency = 6,
            DateTime = 7,
            Double = 5,
            Integer = 2,
            BigInt = 15,
            Percentage = 4,
            String = 130,
            YesNo = 11,
            Decimal = 12,
            DataTable = 10000,
            Binary = 10001,
            Boolean = 10002,
            Guid = 10003,
            Custom = 10004,
            Enumeration = 10005
        }
        #endregion

        #region Enum FileOptions
        public enum FileOptions : int
        {
            SeperateFilesPerTable = 0,
            SingleFile = 1,
            SeperateFilesPerDatabase = 2
        }
        #endregion

        #region enum ProjectTypeEnum : int
        /// <summary>
        /// This enumeration is used to determine which 
        /// project files should get added to after a 
        /// build.
        /// </summary>
        public enum ProjectTypeEnum : int
        {
            NotSet = 0,
            ALC = 1,
            DAC = 2,
            ObjectLibrary = 3,
            Gateway = 4
        }
        #endregion

        #region Enum Scope
        public enum Scope : int
        {
            Private = 0,
            Protected = 1,
            Internal = 2,
            Public = 3
        }
        #endregion

        #endregion

    }
    #endregion

}
