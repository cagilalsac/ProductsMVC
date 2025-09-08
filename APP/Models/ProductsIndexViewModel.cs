using Microsoft.AspNetCore.Mvc.Rendering;

namespace APP.Models
{
    /// <summary>
    /// ViewModel for the Products List view returned from the Products controller's List action.
    /// Encapsulates the data required for displaying and filtering products,
    /// as well as populating categories drop down and stores list box selection lists.
    /// </summary>
    public class ProductsIndexViewModel
    {
        /// <summary>
        /// List of product responses to be displayed.
        /// </summary>
        public List<ProductResponse> List { get; set; }

        /// <summary>
        /// Query request containing filter criteria for products.
        /// </summary>
        public ProductQueryRequest Filter { get; set; }

        /// <summary>
        /// SelectList for categories, used to populate categories drop down list (select HTML tag without multiple attribute).
        /// </summary>
        public SelectList CategorySelectList { get; set; }

        /// <summary>
        /// MultiSelectList for stores, used to populate stores list box (select HTML tag with multiple attribute).
        /// </summary>
        public MultiSelectList StoreMultiSelectList { get; set; }
    }
}
