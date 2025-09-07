using APP.Models.Carts;

namespace APP.Services.Carts
{
    /// <summary>
    /// Defines operations for managing a user's shopping cart.
    /// Provides methods to retrieve, add, remove, and clear cart items.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Retrieves the list of items in the user's cart by user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of <see cref="CartItem"/> objects representing the user's cart contents.</returns>
        public List<CartItem> GetCart(int userId); // public may not be written

        /// <summary>
        /// Retrieves a grouped summary of the user's cart items by product.
        /// Groups items by user ID, product ID, aggregates the product count and total price for each product.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A list of <see cref="CartItemGroupedBy"/> objects, each representing a product in the user's cart
        /// with the total quantity and total price for that product.
        /// </returns>
        public List<CartItemGroupedBy> GetCartGroupedBy(int userId);

        /// <summary>
        /// Adds a product to the user's cart by user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product to add.</param>
        public void AddToCart(int userId, int productId);

        /// <summary>
        /// Removes a product from the user's cart by user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product to remove.</param>
        public void RemoveFromCart(int userId, int productId);

        /// <summary>
        /// Clears all items from the user's cart by user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        public void ClearCart(int userId);
    }
}
