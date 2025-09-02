using CORE.APP.Models;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    /// <summary>
    /// Represents a request model (DTO: Data Transfer Object) for creating or updating a Category entity.
    /// The properties of a model are generally copied from the related entity properties which are not navigation 
    /// properties or which have the columns in the related database table.
    /// Inherits from <see cref="Request"/> to include the Id property with category-specific properties.
    /// </summary>
    public class CategoryRequest : Request
    {
        /// <summary>
        /// Gets or sets the title of the category.
        /// This field is required, therefore can't be null, and has a maximum length of 100 characters.
        /// </summary>
        [Required, StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the category.
        /// This field is optional, therefore can be null.
        /// </summary>
        public string Description { get; set; }

        /*
        Some commonly used data annotation attributes in C#:
        [Required]           // Ensures the property must have a value.
        [StringLength]       // Sets maximum (and optionally minimum) length for strings.
        [Length]             // Sets maximum and minimum length for strings.
        [MinLength]          // Specifies the minimum length for strings or collections.
        [MaxLength]          // Specifies the maximum length for strings or collections.
        [Range]              // Defines the allowed range for numeric values.
        [RegularExpression]  // Validates the property value against a regex pattern.
        [EmailAddress]       // Validates that the property is a valid email address.
        [Phone]              // Validates that the property is a valid phone number.
        [Url]                // Validates that the property is a valid URL.
        [Compare]            // Compares two properties for equality (e.g., password confirmation).
        [DisplayName]        // Sets a friendly name for the property (used in error messages/UI).
        [DataType]           // Specifies the data type (e.g., DateTime) for formatting/UI hints.
        ErrorMessage parameter can be set in all data annotations to show custom validation error messages:
        Example 1: [Required(ErrorMessage = "{0} is required!")] where {0} is the DisplayName (used in MVC) if defined otherwise property name.
        Example 2: [StringLength(100, 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        where {0} is the DisplayName (used in MVC) if defined otherwise property name, {1} is the first parameter which is 100 and
        {2} is the second parameter which is 3.
        */
    }
}
