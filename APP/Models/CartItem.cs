using System.ComponentModel;

namespace APP.Models
{
    /// <summary>
    /// Represents an item in a user's shopping cart.
    /// Contains product details and pricing information for cart operations.
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// The unique identifier of the user who owns the cart item.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The unique identifier of the product added to the cart.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// The name of the product.
        /// Used for display purposes in the views.
        /// </summary>
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        /// <summary>
        /// The unit price of the product as a decimal value.
        /// Used for calculations and backend operations.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// The formatted unit price of the product as a string.
        /// Used for display purposes in the views.
        /// </summary>
        [DisplayName("Unit Price")]
        public string UnitPriceF { get; set; }
    }
}
