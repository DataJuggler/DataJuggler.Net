using System;

namespace DataJuggler.Net.Enumerations
{

    #region enum FunctionTypeEnum
    /// <summary>
    /// This enumeration is used to designate the type of function
    /// </summary>
    public enum FunctionTypeEnum
    {
        Unknown = 0,
        Table = 1,
        Scalar = 2,
        Aggregate = 3,
        System = 4
    }
    #endregion

    #region enum IndexTypeEnum
    /// <summary>
    /// This enumeration is the types of stored procedures
    /// </summary>
    public enum IndexTypeEnum : int
    {
        Unknown = 1,
        Heap = 0,
        Clustered = 1,
        Nonclustered = 2,
        XML = 3,
        Spatial = 4,
        XVelocity = 5,
        ColumnStore = 6
    }
    #endregion

    #region enum SqlConnectionOwnership
    /// <summary>
    /// This enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
    /// we can set the appropriate CommandBehavior when calling ExecuteReader()
    /// </summary>
    public enum SqlConnectionOwnership
    {
        /// <summary>Connection is owned and managed by SqlHelper</summary>
        Internal,
        /// <summary>Connection is owned and managed by the caller</summary>
        External
    }
    #endregion

	#region enum StoredProcedureTypes
	public enum StoredProcedureTypes : int
	{
		NotSet = 0,
		Delete = 10, 
        Select = 20,
        FetchAll = 21,
		Find = 22,
		Insert = 30,
		Update = 40
	}
    #endregion

}
