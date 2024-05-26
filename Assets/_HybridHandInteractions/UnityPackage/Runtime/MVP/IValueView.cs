namespace HybridHandInteractions
{
    /// <summary>
    /// Base interface for all views that display a value
    /// </summary>
    public interface IValueView<TValue>: IView
    {
        /// <summary>
        /// The value to display
        /// </summary>
        public TValue Value { get; set; }
    }
}