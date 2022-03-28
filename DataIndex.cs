

#region using statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataJuggler.Net.Enumerations;

#endregion

namespace DataJuggler.Net
{

    #region class DataIndex
    /// <summary>
    /// This class represents an Index for a table or a view
    /// </summary>
    [Serializable]
    public class DataIndex
    {
        
        #region Private Variables
        private int objectId;
        private string name;
        private int indexId;
        private IndexTypeEnum indexType;
        private string typeDescription;
        private bool clustered;
        private bool isUnique;
        private bool isUniqueConstraint;
        private bool isPrimary;
        private int dataSpaceId;
        private bool ignoreDuplicateKey;
        private int fillFactor;
        private bool isPadded;
        private bool isDisabled;
        private bool isHypothetical;
        private bool allowRowLocks;
        private bool allowPageLocks;
        private bool hasFilter;
        private string filterDefinition;
        #endregion

        #region Properties
            
            #region AllowPageLocks
            /// <summary>
            /// This property gets or sets the value for 'AllowPageLocks'.
            /// </summary>
            public bool AllowPageLocks
            {
                get { return allowPageLocks; }
                set { allowPageLocks = value; }
            }
            #endregion
            
            #region AllowRowLocks
            /// <summary>
            /// This property gets or sets the value for 'AllowRowLocks'.
            /// </summary>
            public bool AllowRowLocks
            {
                get { return allowRowLocks; }
                set { allowRowLocks = value; }
            }
            #endregion
            
            #region Clustered
            /// <summary>
            /// This property gets or sets the value for 'Clustered'.
            /// </summary>
            public bool Clustered
            {
                get { return clustered; }
                set { clustered = value; }
            }
            #endregion
            
            #region DataSpaceId
            /// <summary>
            /// This property gets or sets the value for 'DataSpaceId'.
            /// </summary>
            public int DataSpaceId
            {
                get { return dataSpaceId; }
                set { dataSpaceId = value; }
            }
            #endregion
            
            #region FillFactor
            /// <summary>
            /// This property gets or sets the value for 'FillFactor'.
            /// </summary>
            public int FillFactor
            {
                get { return fillFactor; }
                set { fillFactor = value; }
            }
            #endregion
            
            #region FilterDefinition
            /// <summary>
            /// This property gets or sets the value for 'FilterDefinition'.
            /// </summary>
            public string FilterDefinition
            {
                get { return filterDefinition; }
                set { filterDefinition = value; }
            }
            #endregion
            
            #region HasFilter
            /// <summary>
            /// This property gets or sets the value for 'HasFilter'.
            /// </summary>
            public bool HasFilter
            {
                get { return hasFilter; }
                set { hasFilter = value; }
            }
            #endregion
            
            #region IgnoreDuplicateKey
            /// <summary>
            /// This property gets or sets the value for 'IgnoreDuplicateKey'.
            /// </summary>
            public bool IgnoreDuplicateKey
            {
                get { return ignoreDuplicateKey; }
                set { ignoreDuplicateKey = value; }
            }
            #endregion
            
            #region IndexId
            /// <summary>
            /// This property gets or sets the value for 'IndexId'.
            /// </summary>
            public int IndexId
            {
                get { return indexId; }
                set { indexId = value; }
            }
            #endregion
            
            #region IndexType
            /// <summary>
            /// This property gets or sets the value for 'IndexType'.
            /// </summary>
            public IndexTypeEnum IndexType
            {
                get { return indexType; }
                set { indexType = value; }
            }
            #endregion
            
            #region IsDisabled
            /// <summary>
            /// This property gets or sets the value for 'IsDisabled'.
            /// </summary>
            public bool IsDisabled
            {
                get { return isDisabled; }
                set { isDisabled = value; }
            }
            #endregion
            
            #region IsHypothetical
            /// <summary>
            /// This property gets or sets the value for 'IsHypothetical'.
            /// </summary>
            public bool IsHypothetical
            {
                get { return isHypothetical; }
                set { isHypothetical = value; }
            }
            #endregion
            
            #region IsPadded
            /// <summary>
            /// This property gets or sets the value for 'IsPadded'.
            /// </summary>
            public bool IsPadded
            {
                get { return isPadded; }
                set { isPadded = value; }
            }
            #endregion
            
            #region IsPrimary
            /// <summary>
            /// This property gets or sets the value for 'IsPrimary'.
            /// </summary>
            public bool IsPrimary
            {
                get { return isPrimary; }
                set { isPrimary = value; }
            }
            #endregion
            
            #region IsUnique
            /// <summary>
            /// This property gets or sets the value for 'IsUnique'.
            /// </summary>
            public bool IsUnique
            {
                get { return isUnique; }
                set { isUnique = value; }
            }
            #endregion
            
            #region IsUniqueConstraint
            /// <summary>
            /// This property gets or sets the value for 'IsUniqueConstraint'.
            /// </summary>
            public bool IsUniqueConstraint
            {
                get { return isUniqueConstraint; }
                set { isUniqueConstraint = value; }
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
            
            #region ObjectId
            /// <summary>
            /// This property gets or sets the value for 'ObjectId'.
            /// </summary>
            public int ObjectId
            {
                get { return objectId; }
                set { objectId = value; }
            }
            #endregion
            
            #region TypeDescription
            /// <summary>
            /// This property gets or sets the value for 'TypeDescription'.
            /// </summary>
            public string TypeDescription
            {
                get { return typeDescription; }
                set { typeDescription = value; }
            }
            #endregion
            
        #endregion
        
    }
    #endregion

}
