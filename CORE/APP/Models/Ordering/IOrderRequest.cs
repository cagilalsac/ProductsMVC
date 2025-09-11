namespace CORE.APP.Models.Ordering
{
    /// <summary>
    /// Defines the contract for specifying ordering information in a request.
    /// </summary>
    public interface IOrderRequest
    {
        /// <summary>
        /// Gets or sets the name of the entity property to order by.
        /// </summary>
        public string OrderEntityPropertyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ordering is descending.
        /// </summary>
        public bool IsOrderDescending { get; set; }
    }
}
