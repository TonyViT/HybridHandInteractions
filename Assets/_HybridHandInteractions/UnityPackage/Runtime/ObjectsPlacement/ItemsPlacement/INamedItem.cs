
namespace HybridHandInteractions
{
    /// <summary>
    /// Interface for items that have a name
    /// </summary>
    public interface INamedItem
    {
        /// <summary>
        /// The name that identifies the item
        /// </summary>
        string ItemName { get; set; }
    }
}