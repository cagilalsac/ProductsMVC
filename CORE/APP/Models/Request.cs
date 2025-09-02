namespace CORE.APP.Models
{
    /// <summary>
    /// Abstract base class for all requests.
    /// </summary>
    public abstract class Request
    {
        /// <summary>
        /// Gets or sets the ID of the request.
        /// Defined as virtual to allow overriding in derived classes.
        /// </summary>
        public virtual int Id { get; set; }
    }
}
