using Microsoft.AspNetCore.Http;

namespace CORE.APP.Models.Files
{
    /// <summary>
    /// Represents a request payload that carries a single uploaded file.
    /// </summary>
    /// <remarks>
    /// Intended for request classes with multipart/form-data submissions from Razor views.
    /// The <see cref="FormFile"/> contains the uploaded file stream and metadata.
    /// </remarks>
    public interface IFileRequest
    {
        /// <summary>
        /// The file uploaded by the client.
        /// </summary>
        /// <remarks>
        /// Model binding populates this from a multipart/form-data request.
        /// Validate size and content type before processing.
        /// Also validate that the instance is not null if needed.
        /// </remarks>
        public IFormFile FormFile { get; set; }
    }
}
