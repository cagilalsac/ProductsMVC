using Microsoft.AspNetCore.Http;

namespace CORE.APP.Services.Session.MVC
{
    /// <summary>
    /// Concrete implementation of SessionServiceBase for session management.
    /// Inherits base functionality to store, retrieve, and remove complex objects in session using JSON serialization.
    /// </summary>
    public class SessionService : SessionServiceBase
    {
        /// <summary>
        /// Initializes a new instance of SessionService with the specified IHttpContextAccessor.
        /// Passes the accessor to the base class to enable session operations.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
        public SessionService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
