

#region using statements

using System.Collections;
using System.Collections.Generic;

#endregion

namespace DataJuggler.Net
{

    #region class Reference
	public class Reference
	{

		#region Private Variables
        private int referenceId;
		private string referencename;
		#endregion

	    #region Constructor
	    public Reference(string referenceName, int referenceId)
	    {
		    this.ReferenceName = referenceName;
		    this.ReferenceId = referenceId;
	    }
        #endregion

        #region Methods
        
            #region ToString()
            /// <summary>
            /// method returns the String
            /// </summary>
            public override string ToString()
            {
                // return the referenceName when ToString() is called
                return this.ReferenceName;
            }
            #endregion
            
        #endregion

        #region Properties

            #region ReferenceId
            public int ReferenceId
            {
                get { return referenceId; }
                set { referenceId = value; }
            }
            #endregion

			#region ReferenceName
			public string ReferenceName
			{
				get
				{
					return referencename;
				}
				set
				{
					referencename = value;
				}
			}
			#endregion

		#endregion
		
	}
	#endregion

	#region class ReferencesSet
	public class ReferencesSet : CollectionBase
	{

		#region Private Variables
		private string setname;
		#endregion

		#region Constructor +1 override

			#region ReferencesSet()
			public ReferencesSet()
			{
			}
			#endregion

			#region ReferencesSet(string setName)
			public ReferencesSet(string setName)
			{
				this.SetName = setName;
			}
			#endregion

		#endregion

		#region Methods
			
			#region Add()
			public void Add(Reference component)
			{
				this.List.Add(component);
			}
			#endregion

		#endregion

		#region Properties
		
			#region Index
			public Reference this[int index]
			{
				get
				{
					return (Reference)this.List[index];
				}
			}
			#endregion

			#region SetName
			public string SetName
			{
				get
				{	
					return setname;
				}
				set
				{
					setname = value;
				}
			}
			#endregion

		#endregion

	}
	#endregion
    
}

