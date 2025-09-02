namespace CORE.APP.Models
{
    /// <summary>
    /// Represents a base response object that contains an integer unique identifier and a string unique identifier.
    /// This abstract class can be used as a base for various response models that require the identifier properties.
    /// </summary>
    public abstract class Response
    {
        /// <summary>
        /// Gets or sets the integer unique identifier of the response.
        /// Typically used to correlate responses with database entities.
        /// Defined as virtual to allow overriding in derived classes.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the string unique identifier of the response.
        /// Defined as virtual to allow overriding in derived classes.
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Constructor with parameter to set the Id from a sub (child) class
        /// constructor using Constructor Chaining.
        /// </summary>
        /// <param name="id">The integer unique identifier parameter.</param>
        protected Response(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Default constructor (constructor without any parameters)
        /// that will set the Id to the integer default value (0).
        /// </summary>
        protected Response()
        {
        }
    }
}
