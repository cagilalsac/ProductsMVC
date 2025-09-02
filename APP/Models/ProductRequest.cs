using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    // request properties are created according to the data that will be retrieved from APIs or UIs
    public class ProductRequest : Request
    {
        // copy all the non navigation properties from Product entity
        /*
        ErrorMessage parameter can be set in all data annotations to show custom validation error messages:  
        Example 1: [Required(ErrorMessage = "{0} is required!")] where {0} is the DisplayName if defined otherwise property name
        which is "Product Name".
        Example 2: [StringLength(150, 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")] where {0} is 
        the DisplayName if defined otherwise property name which is "Product Name", {1} is the first parameter which is 150 and {2} is 
        the second parameter which is 3.
        DisplayName attribute can be used to show user friendly names in both validation error messages and views using DisplayNameFor
        HTML Helper.
        // Product Name is required and can be minimum 3 maximum 150 characters.
        */
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [DisplayName("Product Name")]
        public string Name { get; set; }



        [Range(0.01, double.MaxValue, ErrorMessage = "{0} must be a positive decimal number!")] // minimum value can be 0.01,
                                                                                                // maximum value is the largest possible
                                                                                                // value of double type
        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }



        [Range(0, 1000000, ErrorMessage = "{0} must be between {1} and {2}!")] // minimum value can be 0, maximum value can be 1 million
        [DisplayName("Stock Amount")]
        public int? StockAmount { get; set; }



        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }



        [DisplayName("Category")]
        [Required]
        /* 
        The type changed from int to int? to be able to use [Required] attribute. If the type is int, 
        the default value will be 0 and the validation will always be successful even if no value is assigned.
        Also we wan't to get a null value from the drop down list (select HTML tag) when the "-- Select --" option is selected,
        therefore the application user must select a category option other than "-- Select --".
        */
        public int? CategoryId { get; set; }



        //[Required] // can be defined if each product must have at least one store
        [DisplayName("Stores")]
        public List<int> StoreIds { get; set; } = new List<int>();
    }
}
