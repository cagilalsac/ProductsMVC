using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    /// <summary>
    /// Represents a product category in the application.
    /// Inherits from <see cref="Entity"/> to provide common entity properties such as Id and Guid.
    /// </summary>
    public class Category : Entity
    {
        /// <summary>
        /// Gets or sets the title of the category.
        /// </summary>
        /// <remarks>
        /// The <see cref="RequiredAttribute"/> ensures that the category title must be provided.
        /// The <see cref="StringLengthAttribute"/> restricts the maximum length to 100 characters.
        /// </remarks>
        // Required and StringLength are called attributes and they gain new features to the fields, properties, methods or classes.
        // When they are used in entities or requests, they are also called data annotations which provide data validations.
        [Required] 
        [StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the category.
        /// </summary>
        public string Description { get; set; }



        // for category-products one to many relationship
        public List<Product> Products { get; set; } = new List<Product>(); // navigation property for retrieving related Product entities data
                                                                           // of the Category entity data in queries,
                                                                           // initialized for preventing null reference exception
    }
}
