
namespace DataJuggler.Net.Interfaces
{

    #region interface IGridValueProvider
    /// <summary>
    /// This method is used to make it easy to get values based on the fieldName.
    /// This is used by DataJuggler.Blazor.Components.Grid and DataTier.NET
    /// </summary>
    public interface IGridValueProvider
    {
        object GetValue(string fieldName);
    }
    #endregion

}