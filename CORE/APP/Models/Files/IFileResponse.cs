namespace CORE.APP.Models.Files
{
    /// <summary>
    /// Represents the result of a file operation, exposing the stored file's path.
    /// </summary>
    /// <remarks>
    /// Designed for response classes after uploads or exports.
    /// Prefer returning a web-accessible relative path when exposing this to clients.
    /// Avoid leaking sensitive absolute paths.
    /// </remarks>
    public interface IFileResponse
    {
        /// <summary>
        /// The path to the stored file.
        /// </summary>
        /// <remarks>
        /// Can be an absolute physical path for internal use or a relative web path for client access.
        /// Normalize separators and validate that the value is not null and empty if needed.
        /// </remarks>
        public string FilePath { get; set; }
    }
}
