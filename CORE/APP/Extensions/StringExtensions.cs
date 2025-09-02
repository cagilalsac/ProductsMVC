namespace CORE.APP.Extensions
{
    /// <summary>
    /// Provides extension methods for string validation and default value handling.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the specified <paramref name="defaultValue"/> if the string is null, empty, or consists only of white-space characters; otherwise, returns the original string.
        /// Can be used instead of string.IsNullOrWhiteSpace method.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="defaultValue">The value to return if <paramref name="value"/> is null, empty, or white-space.</param>
        /// <returns>
        /// <paramref name="defaultValue"/> if <paramref name="value"/> is null, empty, or white-space; otherwise, <paramref name="value"/>.
        /// </returns>
        public static string HasNotAny(this string value, string defaultValue)
        {
            return HasNotAny(value) ? defaultValue : value;
        }

        /// <summary>
        /// Determines whether the string is null, empty, or consists only of white-space characters.
        /// Can be used instead of string.IsNullOrWhiteSpace method.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>
        /// <c>true</c> if the string is null, empty, or white-space; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNotAny(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Determines whether the string is not null, not empty, and contains at least one non-white-space character.
        /// Can be used instead of string.IsNullOrWhiteSpace method.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>
        /// <c>true</c> if the string contains at least one non-white-space character; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAny(this string value)
        {
            return !HasNotAny(value);
        }
    }
}
