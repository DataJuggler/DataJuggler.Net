

#region using statements

using DataJuggler.Net.Enumerations;
using System.Collections.Generic;

#endregion

namespace DataJuggler.Net.Delegates
{

    #region ItemChangedCallback(object itemChanged, ChangeTypeEnum changeType);
    /// <summary>
    /// This delegate is used to recieve notification event if an item is added, removed or changed.
    /// </summary>
    /// <param name="itemChanged"></param>
    /// <param name="changeType"></param>
    public delegate void ItemChangedCallback(object itemChanged, ChangeTypeEnum changeType);
    #endregion

    #region ProgressStatusCallback(int totalMax, int totalCurrentValue, string totalStatus, int subMax, int subCurrentValue, string subStatus);
    /// <summary>
    /// This delegate is used so that SQLSnapshot can report on the current status for an export
    /// </summary>
    /// <param name="totalMax"></param>
    /// <param name="totalCurrentValue"></param>
    /// <param name="totalStatus"></param>
    /// <param name="subMax"></param>
    /// <param name="subCurrentValue"></param>
    /// <param name="subStatus"></param>
    public delegate void ProgressStatusCallback(int totalMax, int totalCurrentValue, string totalStatus, int subMax, int subCurrentValue, string subStatus);
    #endregion

}