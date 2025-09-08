using System.ComponentModel;

namespace APP.Models
{
    /// <summary>
    /// Represents a grouped summary of cart items for a user and product.
    /// Contains aggregated product count and total price information for display and calculations.
    /// </summary>
    public class CartItemGroupedBy
    {
        /// <summary>
        /// The unique identifier of the user associated with the grouped cart items.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The unique identifier of the product being grouped in the cart.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// The name of the product being grouped in the cart.
        /// </summary>
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        /// <summary>
        /// The total count of this product in the user's cart.
        /// Used for display and summary operations in the views.
        /// </summary>
        [DisplayName("Product Count")]
        public int ProductCount { get; set; }

        /// <summary>
        /// The total price for all instances of this product in the cart (unformatted).
        /// Used for calculations and backend operations.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The formatted total price for display purposes in the views.
        /// </summary>
        [DisplayName("Total Price")]
        public string TotalPriceF { get; set; }
    }
}
