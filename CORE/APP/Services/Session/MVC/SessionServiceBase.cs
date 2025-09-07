using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace CORE.APP.Services.Session.MVC
{
    /// <summary>
    /// Abstract base class for session management.
    /// Provides methods to store, retrieve, and remove complex objects in session using JSON serialization.
    /// </summary>
    public abstract class SessionServiceBase
    {
        /// <summary>
        /// Provides access to the current HTTP context, enabling session operations.
        /// Will be inherited by derived classes to be used in overridden virtual methods.
        /// </summary>
        protected IHttpContextAccessor HttpContextAccessor { get; }

        /// <summary>
        /// Initializes the session service with an IHttpContextAccessor.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
        protected SessionServiceBase(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves a session value by key and deserializes it to the specified type.
        /// Returns null if the key does not exist or the value is empty.
        /// Defined as virtual to allow overriding, therefore changing the method behavior, in derived classes if needed.
        /// </summary>
        /// <typeparam name="T">Type to deserialize the session value to.</typeparam>
        /// <param name="key">Session key.</param>
        /// <returns>Deserialized object or null.</returns>
        public virtual T GetSession<T>(string key) where T : class
        {
            var value = HttpContextAccessor.HttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(value))
                return null;
            return JsonSerializer.Deserialize<T>(value); // Converts JSON string to object of type T
        }

        /// <summary>
        /// Serializes the provided object and stores it in session under the specified key.
        /// Does nothing if the instance is null.
        /// Defined as virtual to allow overriding, therefore changing the method behavior, in derived classes if needed.
        /// </summary>
        /// <typeparam name="T">Type of the object to store.</typeparam>
        /// <param name="key">Session key.</param>
        /// <param name="instance">Object to serialize and store.</param>
        public virtual void SetSession<T>(string key, T instance) where T : class
        {
            if (instance is not null)
            {
                var value = JsonSerializer.Serialize(instance); // Converts object of type T to JSON string
                HttpContextAccessor.HttpContext.Session.SetString(key, value);
            }
        }

        /// <summary>
        /// Removes the session value associated with the specified key.
        /// Defined as virtual to allow overriding, therefore changing the method behavior, in derived classes if needed.
        /// </summary>
        /// <param name="key">Session key to remove.</param>
        public virtual void RemoveSession(string key)
        {
            HttpContextAccessor.HttpContext.Session.Remove(key);
        }
    }
}
