using CORE.APP.Models;
using System.Globalization;

namespace CORE.APP.Services
{
    /// <summary>
    /// Abstract base class for handling operations with culture-specific settings.
    /// </summary>
    public abstract class ServiceBase
    {
        // Encapsulation:
        // Backing field to store the current culture information.
        private CultureInfo _cultureInfo;

        /// <summary>
        /// Gets or sets the culture information for the current context.
        /// When set, it updates both the current thread's culture and UI culture
        /// to ensure consistent formatting and localization across the application.
        /// </summary>
        protected CultureInfo CultureInfo
        {
            get
            {
                // Return the current culture info.
                return _cultureInfo;
            }
            set
            {
                // Update the backing field with the new culture info.
                _cultureInfo = value;

                // Apply the new culture to the current thread for formatting (e.g., numbers, dates).
                Thread.CurrentThread.CurrentCulture = _cultureInfo;

                // Apply the new culture to the current thread for UI localization (e.g., resource lookups).
                Thread.CurrentThread.CurrentUICulture = _cultureInfo;
            }
        }



        /// <summary>
        /// Protected constructor for the Service class.
        /// Initializes the CultureInfo property to "en-US" by default,
        /// ensuring consistent formatting and localization behavior when a derived class is instantiated.
        /// </summary>
        protected ServiceBase()
        {
            // Set the default culture to English (United States), "tr-TR" parameter can be used for Turkish
            CultureInfo = new CultureInfo("en-US");
        }

        /// <summary>
        /// Creates a success <see cref="CommandResponse"/> with message and entity ID.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <param name="id">The entity ID associated with the success command response.</param>
        /// <returns>A success <see cref="CommandResponse"/>.</returns>
        protected CommandResponse Success(string message, int id) => new CommandResponse(true, message, id);

        /// <summary>
        /// Creates an error <see cref="CommandResponse"/> with message.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <returns>An error <see cref="CommandResponse"/>.</returns>
        protected CommandResponse Error(string message) => new CommandResponse(false, message);
    }
}
