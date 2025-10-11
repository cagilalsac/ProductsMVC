namespace CORE.APP.Domain.Files
{
    /// <summary>
    /// Represents a domain entity that references a file by its storage path.
    /// </summary>
    /// <remarks>
    /// Intended for entity classes. Use relative paths for web exposure
    /// and avoid leaking absolute physical paths to clients.
    /// </remarks>
    public interface IFileEntity
    {
        /// <summary>
        /// The storage path of the file.
        /// </summary>
        /// <remarks>
        /// Can be absolute (for internal use) or relative (for client-facing scenarios).
        /// Normalize directory separators and validate that the value is not null and empty if needed.
        /// </remarks>
        public string FilePath { get; set; }
    }
}
