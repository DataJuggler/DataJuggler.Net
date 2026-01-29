

#region using statements

#endregion

namespace DataJuggler.Net.Enumerations
{

    #region enum ChangeTypeEnum : int
    /// <summary>
    /// This enum contains choices that describe what type of a change occurred.
    /// </summary>
    public enum ChangeTypeEnum : int
    {
        ItemRemoved = -1,
        Unknown = 0,
        ItemAdded = 1,
        ItemChanged = 2
    }
    #endregion

}