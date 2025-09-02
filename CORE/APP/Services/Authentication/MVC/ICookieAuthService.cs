namespace CORE.APP.Services.Authentication.MVC
{
    /// <summary>
    /// Defines methods for handling cookie-based authentication operations.
    /// </summary>
    public interface ICookieAuthService
    {
        /// <summary>
        /// Signs in a user by creating an authentication cookie.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="userName">The username of the user.</param>
        /// <param name="userRoleNames">An array of role names assigned to the user.</param>
        /// <param name="expiration">Optional expiration date and time for the authentication cookie. If not specified, a default value (null) is used.</param>
        /// <param name="isPersistent">Indicates whether the authentication cookie should persist across browser sessions.</param>
        /// <returns>A task representing the asynchronous sign-in operation.</returns>
        public Task SignIn(int userId, string userName, string[] userRoleNames, DateTime? expiration = default, bool isPersistent = true);

        /// <summary>
        /// Signs out the current user by removing the authentication cookie.
        /// </summary>
        /// <returns>A task representing the asynchronous sign-out operation.</returns>
        public Task SignOut();
    }
}
