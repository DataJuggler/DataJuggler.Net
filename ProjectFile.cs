

#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

#endregion

namespace DataJuggler.Net
{

    #region class ProjectFile
    /// <summary>
    /// This object is used to keep track of files that were
    /// added to a project during a build.
    /// </summary>
    public class ProjectFile
    {
    
        #region Private Variables
        private string fullFilePath;
        private DataManager.ProjectTypeEnum projectType;
        private string fileName;
        private bool exclude;
        private bool previouslyExisted;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of a ProjectFile object.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="projectType"></param>
        public ProjectFile(string fileName, DataManager.ProjectTypeEnum projectType)
        {
            // set the FileName
            this.FullFilePath = fileName;
            
            // Set the project type
            this.ProjectType = projectType;
        } 
        #endregion
        
        #region Methods

            #region SetFileName(string fullFilePath)
            /// <summary>
            /// This method sets the FileName for the fullFilePath.
            /// </summary>
            /// <param name="fullFilePath"></param>
            /// <returns></returns>
            private string SetFileName(string fullFilePath)
            {
                // initial value
                string fileName = "";
            
                try
                {
                    // create a file info object
                    FileInfo fi = new FileInfo(fullFilePath);
                    
                    // set the return value
                    fileName = fi.Name;
                }
                catch (Exception error)
                {
                    // for debugging only
                    string err = error.ToString();
                }
                
                // return value
                return fileName;
            }
            #endregion
        
            #region ToString()
            /// <summary>
            /// this method returns the FileName when ToString is called.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                // return the file name when ToString is called
                return this.FullFilePath;
            } 
            #endregion
        
        #endregion
        
        #region Properties

            #region Exclude
            /// <summary>
            /// This property gets or sets the value for 'Exclude'.
            /// </summary>
            public bool Exclude
            {
                get { return exclude; }
                set { exclude = value; }
            }
            #endregion
            
            #region FileName
            /// <summary>
            /// This is the FileName only without the full path.
            /// </summary>
            public string FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }
            #endregion
            
            #region FullFilePath
            /// <summary>
            /// This property gets or sets the full path of a
            /// the file being added.
            /// </summary>
            public string FullFilePath
            {
                get { return fullFilePath; }
                set 
                { 
                    // set the full File Path
                    fullFilePath = value;
                    
                    // set the fileName
                    this.FileName = SetFileName(fullFilePath);
                }
            }
            #endregion

            #region HasFileName
            /// <summary>
            /// This property returns true if the 'FileName' exists.
            /// </summary>
            public bool HasFileName
            {
                get
                {
                    // initial value
                    bool hasFileName = (!String.IsNullOrEmpty(this.FileName));
                    
                    // return value
                    return hasFileName;
                }
            }
            #endregion
            
            #region HasFullFilePath
            /// <summary>
            /// This property returns true if the 'FullFilePath' exists.
            /// </summary>
            public bool HasFullFilePath
            {
                get
                {
                    // initial value
                    bool hasFullFilePath = (!String.IsNullOrEmpty(this.FullFilePath));
                    
                    // return value
                    return hasFullFilePath;
                }
            }
            #endregion

            #region IsController
            /// <summary>
            /// This read only property returns true if the FileName ends with Controller.cs
            /// </summary>
            public bool IsController
            {
                get
                {
                    // initial value
                    bool isController = false;

                    // if this isController
                    isController = ((this.ProjectType == DataManager.ProjectTypeEnum.ALC) && (this.HasFileName) && (this.FileName.EndsWith("Controller.cs")));

                    // return value
                    return isController;
                }
            }
            #endregion

            #region IsDataManager
            /// <summary>
            /// This read only property returns true if the ProjecType is DataAccessCompoent and FileName ends with Manager.cs
            /// </summary>
            public bool IsDataManager
            {
                get
                {
                    // initial value
                    bool isDataManager = false;

                    // if this is a DataAccessComponent.DataManager file
                    isDataManager = ((this.ProjectType == DataManager.ProjectTypeEnum.DAC) && (this.HasFileName) && (this.FileName.EndsWith("Manager.cs")));

                    // return value
                    return isDataManager;
                }
            }
            #endregion

            #region IsDataOperation
            /// <summary>
            /// This read only property returns true if the ProjectType is ApplicationLogicComponent is a Data the FileName ends with Methods.cs
            /// </summary>
            public bool IsDataOperation
            {
                get
                {
                    // initial value
                    bool isDataOperation = false;

                    // if this isController
                    isDataOperation = ((this.ProjectType == DataManager.ProjectTypeEnum.ALC) && (this.HasFileName) && (this.FileName.EndsWith("Methods.cs")));

                    // return value
                    return isDataOperation;
                }
            }
            #endregion

            #region IsDeleteStoredProcedure
            /// <summary>
            /// This read only property returns true if this file is a StoredProcedure (ends with StoredProcedure.cs) and starts with the word Delete.
            /// Example: DeleteProjectStoredProcedure.cs
            /// </summary>
            public bool IsDeleteStoredProcedure
            {
                get
                {
                    // initial value
                    bool isDeleteStoredProcedure = false;

                    // if this is a StoredProcedure and the ProcedureStarts with Delete
                    isDeleteStoredProcedure = ((this.IsStoredProcedure) && (this.FileName.StartsWith("Delete")));

                    // return value
                    return isDeleteStoredProcedure;
                }
            }
            #endregion

            #region IsFetchAllStoredProcedure
            /// <summary>
            /// This read only property returns true if this file is a StoredProcedure (ends with StoredProcedure.cs) and starts with the word FetchAll.
            /// Example: FetchAllProjectsStoredProcedure.cs
            /// </summary>
            public bool IsFetchAllStoredProcedure
            {
                get
                {
                    // initial value
                    bool isFetchAllStoredProcedure = false;

                    // if this is a StoredProcedure and the ProcedureStarts with FetchAll
                    isFetchAllStoredProcedure = ((this.IsStoredProcedure) && (this.FileName.StartsWith("FetchAll")));

                    // return value
                    return isFetchAllStoredProcedure;
                }
            }
            #endregion

            #region IsFindStoredProcedure
            /// <summary>
            /// This read only property returns true if this file is a StoredProcedure (ends with StoredProcedure.cs) and starts with the word Find.
            /// Example: FindProjectStoredProcedure.cs
            /// </summary>
            public bool IsFindStoredProcedure
            {
                get
                {
                    // initial value
                    bool isFindStoredProcedure = false;

                    // if this is a StoredProcedure and the ProcedureStarts with Find
                    isFindStoredProcedure = ((this.IsStoredProcedure) && (this.FileName.StartsWith("Find")));

                    // return value
                    return isFindStoredProcedure;
                }
            }
            #endregion

            #region IsInsertStoredProcedure
            /// <summary>
            /// This read only property returns true if this file is a StoredProcedure (ends with StoredProcedure.cs) and starts with the word Insert.
            /// Example: InsertProjectStoredProcedure.cs
            /// </summary>
            public bool IsInsertStoredProcedure
            {
                get
                {
                    // initial value
                    bool isInsertStoredProcedure = false;

                    // if this is a StoredProcedure and the ProcedureStarts with Insert
                    isInsertStoredProcedure = ((this.IsStoredProcedure) && (this.FileName.StartsWith("Insert")));

                    // return value
                    return isInsertStoredProcedure;
                }
            }
            #endregion

            #region IsNew
            /// <summary>
            /// This read only property returns the value for 'IsNew'.
            /// </summary>
            public bool IsNew
            {
                get
                {
                    // initial value
                    bool isNew = (!PreviouslyExisted);
                    
                    // return value
                    return isNew;
                }
            }
            #endregion
            
            #region IsReader
            /// <summary>
            /// This read only property returns true if the ProjecType is AccessCompoent and FileName ends with Reader.cs
            /// </summary>
            public bool IsReader
            {
                get
                {
                    // initial value
                    bool isReader = false;

                    // if this is a DataAccessComponent.Manager file
                    isReader = ((this.ProjectType == DataManager.ProjectTypeEnum.DAC) && (this.HasFileName) && (this.FileName.EndsWith("Reader.cs")));

                    // return value
                    return isReader;
                }
            }
            #endregion

            #region IsStoredProcedure
            /// <summary>
            /// This read only property returns true if the ProjecType is DataAccessCompoent and FileName ends with StoredProcedure.cs
            /// </summary>
            public bool IsStoredProcedure
            {
                get
                {
                    // initial value
                    bool isStoredProcedure = false;

                    // if this is a StoredProcedure
                    isStoredProcedure = ((this.ProjectType == DataManager.ProjectTypeEnum.DAC) && (this.HasFileName) && (this.FileName.EndsWith("StoredProcedure.cs")));

                    // return value
                    return isStoredProcedure;
                }
            }
            #endregion

            #region IsUpdateStoredProcedure
            /// <summary>
            /// This read only property returns true if this file is a StoredProcedure (ends with StoredProcedure.cs) and starts with the word Update.
            /// Example: UpdateProjectStoredProcedure.cs
            /// </summary>
            public bool IsUpdateStoredProcedure
            {
                get
                {
                    // initial value
                    bool isUpdateStoredProcedure = false;

                    // if this is a StoredProcedure and the ProcedureStarts with Update
                    isUpdateStoredProcedure = ((this.IsStoredProcedure) && (this.FileName.StartsWith("Update")));

                    // return value
                    return isUpdateStoredProcedure;
                }
            }
            #endregion

            #region IsWriter
            /// <summary>
            /// This read only property returns true if the ProjecType is AccessCompoent and FileName ends with Writer.cs
            /// </summary>
            public bool IsWriter
            {
                get
                {
                    // initial value
                    bool isWriter = false;

                    // if this is a DataAccessComponent.Manager file
                    isWriter = ((this.ProjectType == DataManager.ProjectTypeEnum.DAC) && (this.HasFileName) && ((this.FileName.EndsWith("Writer.cs")) || (this.FileName.EndsWith("WriterBase.cs"))));

                    // return value
                    return isWriter;
                }
            }
            #endregion
            
            #region PreviouslyExisted
            /// <summary>
            /// This property gets or sets the value for 'PreviouslyExisted'.
            /// </summary>
            public bool PreviouslyExisted
            {
                get { return previouslyExisted; }
                set { previouslyExisted = value; }
            }
            #endregion
            
            #region ProjectType
            /// <summary>
            /// This property is used to determine which 
            /// project new files should get added to.
            /// </summary>
            public DataManager.ProjectTypeEnum ProjectType
            {
                get { return projectType; }
                set { projectType = value; }
            } 
            #endregion
        
            #region ShortFilePath
            /// <summary>
            /// This read only property returns the value for 'ShortFilePath'.
            /// This value is determined by the ProjectType and the Folder
            /// this file resides in.
            /// </summary>
            public string ShortFilePath
            {
                get
                {
                    // initial value
                    string shortFilePath = "";

                    // Create the StringBuilder
                    StringBuilder sb = new StringBuilder();

                    // determine the shortFilePath for each project types 
                    if (ProjectType == DataManager.ProjectTypeEnum.ALC)
                    {
                        // Append the ProjectType
                        sb.Append(@"ApplicationLogicComponent\");

                        // if this is a Controller
                        if (IsController)
                        {
                            // Append the FolderName
                            sb.Append(@"Controllers\");
                        }
                        // if this is a DataOperation
                        else if (IsDataOperation)
                        {
                            // Append the FolderName
                            sb.Append(@"DataOperations\");
                        }
                    }
                    else if (projectType == DataManager.ProjectTypeEnum.DAC)
                    {
                        // Append the ProjectType
                        sb.Append(@"DataAccessComponent\");

                        // if this is a DataManager
                        if ((IsDataManager) || (IsReader) || (IsWriter))
                        {  
                            // Append the parent folder
                            sb.Append(@"Data\");

                            // if the value for IsReader is true
                            if (IsReader)
                            {
                                // Append Readers
                                sb.Append(@"Readers\");
                            }
                            else if (IsWriter)
                            {
                                // Append Writers
                                sb.Append(@"Writers\");
                            }
                        }
                        else if (this.IsStoredProcedure)
                        {
                            // Append the parent folder
                            sb.Append(@"StoredProcedures\");

                            // Determine the SubFolder by the StoredProcedureType
                            if (IsDeleteStoredProcedure)
                            {
                                // Append the parent folder
                                sb.Append(@"DeleteProcedures\");
                            }
                            else if ((IsFetchAllStoredProcedure) || (IsFindStoredProcedure))
                            {
                                // Append the parent folder
                                sb.Append(@"FetchProcedures\");
                            }
                            else if (IsInsertStoredProcedure)
                            {
                                // Append the parent folder
                                sb.Append(@"InsertProcedures\");
                            }
                            else if (IsUpdateStoredProcedure)
                            {
                                // Append the parent folder
                                sb.Append(@"UpdateProcedures\");
                            }
                        }
                    }
                    else if (projectType == DataManager.ProjectTypeEnum.ObjectLibrary)
                    {
                        // Append the ProjectType
                        sb.Append(@"ObjectLibrary\BusinessObjects\");
                    }

                    // Append The FileName
                    sb.Append(FileName);
                    
                    // set the return value
                    shortFilePath = sb.ToString();
                    
                    // return value
                    return shortFilePath;
                }
            }
            #endregion
            
        #endregion
    
    } 
    #endregion
    
}
