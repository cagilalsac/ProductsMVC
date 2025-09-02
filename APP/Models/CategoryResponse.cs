using CORE.APP.Models;

namespace APP.Models
{
    /// <summary>
    /// Represents a response model (DTO: Data Trasfer Object) for querying Category entities.
    /// The properties of a model are generally copied from the related entity properties which are not navigation 
    /// properties or which have the columns in the related database table.
    /// Inherits from <see cref="Response"/> to include the Id and Guid properties with category-specific properties.
    /// </summary>
    public class CategoryResponse : Response
    {
        /// <summary>
        /// Gets or sets the title of the category.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the category.
        /// </summary>
        public string Description { get; set; }
    }
}
