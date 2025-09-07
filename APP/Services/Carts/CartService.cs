using APP.Models;
using APP.Models.Carts;
using CORE.APP.Services;
using CORE.APP.Services.Session.MVC;

namespace APP.Services.Carts
{
    /// <summary>
    /// Provides operations for managing a user's shopping cart.
    /// Stores cart data in session and retrieves product details using the product service.
    /// </summary>
    public class CartService : ICartService
    {
        /// <summary>
        /// The private constant session key used to store and retrieve cart data.
        /// </summary>
        const string SESSIONKEY = "cart";

        /// <summary>
        /// Service for session management, used to store and retrieve cart items.
        /// </summary>
        private readonly SessionServiceBase _sessionService;

        /// <summary>
        /// Service for accessing product data.
        /// </summary>
        private readonly IService<ProductRequest, ProductResponse> _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartService"/> class with the injection of session and product services.
        /// </summary>
        /// <param name="sessionService">Session service for managing cart data in session.</param>
        /// <param name="productService">Product service for retrieving product details.</param>
        public CartService(SessionServiceBase sessionService, IService<ProductRequest, ProductResponse> productService)
        {
            _productService = productService;
            _sessionService = sessionService;
        }

        /// <summary>
        /// Retrieves the list of items in the user's cart by user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of <see cref="CartItem"/> objects representing the user's cart contents.</returns>
        public List<CartItem> GetCart(int userId)
        {
            var cart = _sessionService.GetSession<List<CartItem>>(SESSIONKEY);
            if (cart is not null)
                return cart.Where(c => c.UserId == userId).ToList();
            return new List<CartItem>();
        }

        /// <summary>
        /// Retrieves a grouped summary of the user's cart items by product.
        /// Groups items by user ID, product ID and product name, then aggregates the product count and total price for each product.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A list of <see cref="CartItemGroupedBy"/> objects, each representing a product in the user's cart
        /// with the total quantity and total price for that product.
        /// </returns>
        public List<CartItemGroupedBy> GetCartGroupedBy(int userId)
        {
            // Get all cart items for the specified user.
            var cart = GetCart(userId);

            // Group cart items by user ID, product ID and product name, then project each group into a summary object.
            return cart
                .GroupBy(cartItem => new // group cart items with key user ID, product ID and product name
                { 
                    cartItem.UserId, 
                    cartItem.ProductId, 
                    cartItem.ProductName 
                }) 
                .Select(cartItemGroupedBy => new CartItemGroupedBy
                {
                    UserId = cartItemGroupedBy.Key.UserId, // grouped cart item key's user ID
                    ProductId = cartItemGroupedBy.Key.ProductId, // grouped cart item key's product ID
                    ProductName = cartItemGroupedBy.Key.ProductName, // grouped cart item key's product name
                    ProductCount = cartItemGroupedBy.Count(), // Total quantity of this product according to the key
                    TotalPrice = cartItemGroupedBy.Sum(cartItem => cartItem.UnitPrice), // Aggregate price according to the key
                    TotalPriceF = cartItemGroupedBy.Sum(cartItem => cartItem.UnitPrice).ToString("C2") // Formatted price for display
                }).ToList();
        }

        /// <summary>
        /// Adds a product to the user's cart by user ID.
        /// Retrieves product details and updates the session cart.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product to add.</param>
        public void AddToCart(int userId, int productId)
        {
            var product = _productService.Item(productId);
            if (product is not null)
            {
                var cart = GetCart(userId);
                cart.Add(new CartItem
                {
                    UserId = userId,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.UnitPrice,
                    UnitPriceF = product.UnitPrice.ToString("C2")
                });
                _sessionService.SetSession(SESSIONKEY, cart);
            }
        }

        /// <summary>
        /// Removes a product from the user's cart by user ID.
        /// Updates the session cart after removal.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="productId">The unique identifier of the product to remove.</param>
        public void RemoveFromCart(int userId, int productId)
        {
            var cart = GetCart(userId);
            var cartItem = cart.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem is not null)
                cart.Remove(cartItem);
            _sessionService.SetSession(SESSIONKEY, cart);
        }

        /// <summary>
        /// Clears all items from the user's cart by user ID.
        /// Updates the session cart after clearing.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        public void ClearCart(int userId)
        {
            var cart = GetCart(userId);
            cart.RemoveAll(c => c.UserId == userId);
            _sessionService.SetSession(SESSIONKEY, cart);
        }
    }
}
