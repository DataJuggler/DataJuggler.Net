

#region using statements

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

#endregion

namespace DataJuggler.Net
{

    #region class ProjectFileManager
    /// <summary>
    /// This class is used to keep track of new files that 
    /// were created during a build.
    /// </summary>
    public class ProjectFileManager
    {
        
        #region Private variables
        private ObservableCollection<ProjectFile> files;
        private List<string> gatewayMethodNames;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of a 'ProjectFileManager' object.
        /// </summary>
        public ProjectFileManager()
        {
            // perform initializations for this object
            Init();
        }
        #endregion

        #region Events

            #region Files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            /// <summary>
            /// event is fired when Files _ Collection Changed
            /// </summary>
            private void Files_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                // if the actiion is Add
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    // get the index of where the change occurred
                    int index = e.NewStartingIndex;

                    // if the index is in range
                    if ((index >= 0) && (index <= Files.Count))
                    {
                        // get a reference to the new file
                        ProjectFile newFile = Files[index];

                        // Set the value for PreviouslyExisted
                        Files[index].PreviouslyExisted = ((newFile.HasFullFilePath) && (File.Exists(newFile.FullFilePath)));
                    }
                }
            }
            #endregion
            
        #endregion

        #region Methods

        #region Init()
        /// <summary>
        /// Perform initializations for this object
        /// </summary>
        private void Init()
            {
                // create the NewFiles list
                this.Files = new ObservableCollection<ProjectFile>();
                this.Files.CollectionChanged += Files_CollectionChanged;

                // Create a new collection of 'string' objects.
                this.GatewayMethodNames = new List<string>();
            }
            #endregion

        #endregion

        #region Properties

            #region ActiveFiles
            /// <summary>
            /// This read only property returns the Files that have Exclude = False
            /// </summary>
            public IList<ProjectFile> ActiveFiles
            {
                get
                { 
                    // initial value
                    IList<ProjectFile> activeFiles = null;

                    // If the Files object exists
                    if (this.HasFiles)
                    {
                        // set the return value
                        activeFiles = this.Files.Where(x => x.Exclude == false).ToList();
                    }

                    // return value
                    return activeFiles;
                }
            } 
            #endregion

            #region Files
            /// <summary>
            /// This property gets or sets the value for 'Files'.
            /// </summary>
            public ObservableCollection<ProjectFile> Files
            {
                get { return files; }
                set { files = value; }
            }
            #endregion
          
            #region GatewayMethodNames
            /// <summary>
            /// This property gets or sets the value for 'GatewayMethodNames'.
            /// </summary>
            public List<string> GatewayMethodNames
            {
                get { return gatewayMethodNames; }
                set { gatewayMethodNames = value; }
            }
            #endregion
            
            #region HasActiveFiles
            /// <summary>
            /// This property returns true if this object has an 'ActiveFiles'.
            /// </summary>
            public bool HasActiveFiles
            {
                get
                {
                    // initial value
                    bool hasActiveFiles = (this.ActiveFiles != null);
                    
                    // return value
                    return hasActiveFiles;
                }
            }
            #endregion
            
            #region HasFiles
            /// <summary>
            /// This property returns true if this object has a 'Files'.
            /// </summary>
            public bool HasFiles
            {
                get
                {
                    // initial value
                    bool hasFiles = (this.Files != null);
                    
                    // return value
                    return hasFiles;
                }
            }
            #endregion
            
            #region HasGatewayMethodNames
            /// <summary>
            /// This property returns true if this object has a 'GatewayMethodNames'.
            /// </summary>
            public bool HasGatewayMethodNames
            {
                get
                {
                    // initial value
                    bool hasGatewayMethodNames = (this.GatewayMethodNames != null);
                    
                    // return value
                    return hasGatewayMethodNames;
                }
            }
            #endregion

            #region HasNewFiles
            /// <summary>
            /// This property returns true if this object has a 'NewFiles'.
            /// </summary>
            public bool HasNewFiles
            {
                get
                {
                    // initial value
                    bool hasNewFiles = (this.NewFiles != null);
                    
                    // return value
                    return hasNewFiles;
                }
            }
            #endregion
            
            #region HasOneOrMoreNewFiles
            /// <summary>
            /// This read only property returns the Files that have one or more ProjectFiles with IsNew = true'
            /// (previouslyExisted = false).
            /// </summary>
            public bool HasOneOrMoreNewFiles
            {
                get
                { 
                    // initial value
                    bool hasOneOrMoreNewFiles = ((HasNewFiles) && (NewFiles.Count > 0));

                    // return value
                    return hasOneOrMoreNewFiles;
                }
            } 
            #endregion

            #region NewFiles
            /// <summary>
            /// This read only property returns the Files that have IsNew = true
            /// </summary>
            public List<ProjectFile> NewFiles
            {
                get
                { 
                    // initial value
                    List<ProjectFile> newFiles = null;

                    // If the Files object exists
                    if (this.HasFiles)
                    {
                        // set the return value
                        newFiles = this.Files.Where(x => x.IsNew == true).ToList();
                    }

                    // return value
                    return newFiles;
                }
            } 
            #endregion
            
            #region WereNewFilesCreated
            /// <summary>
            /// This read only property returns the value for 'WereNewFilesCreated'.
            /// </summary>
            public bool WereNewFilesCreated
            {
                get
                {
                    // initial value
                    bool wereNewFilesCreated = HasOneOrMoreNewFiles;

                    // return value
                    return wereNewFilesCreated;
                }
            }
            #endregion
            
        #endregion
        
    } 
    #endregion
    
}
