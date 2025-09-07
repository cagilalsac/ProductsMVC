using APP.Services.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers.Carts
{
    /// <summary>
    /// Controller for managing shopping cart operations for authenticated users.
    /// Provides actions to view, add, remove, and clear cart items.
    /// </summary>
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ICartService _cartService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartsController"/> class.
        /// </summary>
        /// <param name="cartService">Service for cart operations.</param>
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Retrieves the current user's ID from claims.
        /// </summary>
        /// <returns>The unique identifier of the authenticated user.</returns>
        private int GetUserId() => Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == "Id")?.Value);

        /// <summary>
        /// Displays the grouped cart items for the current user.
        /// </summary>
        /// <returns>The cart view with grouped cart items.</returns>
        public IActionResult Index()
        {
            var cartGroupedBy = _cartService.GetCartGroupedBy(GetUserId());
            return View(cartGroupedBy);
        }

        /// <summary>
        /// Clears all items from the current user's cart.
        /// Sets a message and redirects to the cart index view.
        /// </summary>
        /// <returns>Redirects to the cart index view.</returns>
        public IActionResult Clear()
        {
            _cartService.ClearCart(GetUserId());
            TempData["Message"] = "Cart cleared.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Removes a specific product from the current user's cart.
        /// Sets a message and redirects to the cart index view.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to remove.</param>
        /// <returns>Redirects to the cart index view.</returns>
        public IActionResult Remove(int productId)
        {
            _cartService.RemoveFromCart(GetUserId(), productId);
            TempData["Message"] = "Product removed from cart.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Adds a specific product to the current user's cart.
        /// Sets a message and redirects to the products index view.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to add.</param>
        /// <returns>Redirects to the products index view.</returns>
        public IActionResult Add(int productId)
        {
            _cartService.AddToCart(GetUserId(), productId);
            TempData["Message"] = "Product added to cart.";
            return RedirectToAction("Index", "Products");
        }
    }
}
