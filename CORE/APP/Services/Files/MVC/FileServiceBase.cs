using CORE.APP.Models;
using Microsoft.AspNetCore.Http;

namespace CORE.APP.Services.Files.MVC
{
    /// <summary>
    /// Abstract base service that provides common file operations for service classes by dependency injection.
    /// </summary>
    /// <remarks>
    /// Uses a GUID-based naming strategy and stores files under the application's wwwroot.
    /// Methods return client-accessible relative paths (e.g., /files/{guid}.ext).
    /// </remarks>
    public abstract class FileServiceBase : ServiceBase
    {
        /// <summary>
        /// Builds a relative web path for the uploaded file, preserving the original extension.
        /// </summary>
        /// <param name="formFile">The uploaded file.</param>
        /// <param name="filesFolder">Target folder under wwwroot (default: "files").</param>
        /// <returns>
        /// A relative path like <c>/files/{guid}.ext</c>, or <c>null</c> if the input is null or empty.
        /// </returns>
        /// <remarks>
        /// This method does not write the file to disk; it only generates a path.
        /// Use <see cref="SaveFile(IFormFile, string, double, string)"/> to persist the file.
        /// </remarks>
        public virtual string GetFilePath(IFormFile formFile, string filesFolder = "files")
        {
            if (formFile is null || formFile.Length == 0)
                return null;
            var fileExtension = Path.GetExtension(formFile.FileName);
            var fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
            return $"/{filesFolder}/{fileName}";
        }

        /// <summary>
        /// Saves the uploaded file to the specified relative path under wwwroot with validation.
        /// </summary>
        /// <param name="formFile">The uploaded file to save.</param>
        /// <param name="filePath">Relative path produced by <see cref="GetFilePath(IFormFile, string)"/> (e.g., /files/{guid}.ext).</param>
        /// <param name="maximumFileSizeInMb">Maximum allowed file size in megabytes (default: 2 MB).</param>
        /// <param name="fileExtensions">Comma-separated list of allowed extensions without dots (default: "jpg,jpeg,png").</param>
        /// <returns>
        /// A <see cref="CommandResponse"/> indicating success or providing an error message if validation fails.
        /// </returns>
        /// <remarks>
        /// Validates file size and extension (case-insensitive). Overwrites any existing file at the target path.
        /// </remarks>
        public virtual CommandResponse SaveFile(IFormFile formFile, string filePath, double maximumFileSizeInMb = 2, string fileExtensions = "jpg,jpeg,png")
        {
            if (formFile is not null && formFile.Length > 0 && !string.IsNullOrWhiteSpace(filePath))
            {
                var allowedMaximumFileSizeInMb = maximumFileSizeInMb * Math.Pow(1024, 2);
                if (formFile.Length > allowedMaximumFileSizeInMb)
                    return Error($"File size can't exceed {maximumFileSizeInMb.ToString("N1")} megabytes!");
                var formFileExtension = Path.GetExtension(formFile.FileName).TrimStart('.').ToLower();
                var allowedFileExtensions = fileExtensions.Split(',').Select(fileExtension => fileExtension.ToLower());
                if (!allowedFileExtensions.Contains(formFileExtension))
                    return Error($"Only {string.Join(", ", allowedFileExtensions)} file extensions are allowed!");
                using (var fileStream = new FileStream($"wwwroot/{filePath}", FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
            }
            return Success(string.Empty, 0);
        }

        /// <summary>
        /// Deletes a file from wwwroot if the path is valid and the file exists.
        /// </summary>
        /// <param name="filePath">Relative path of the file to delete (e.g., /files/{guid}.ext).</param>
        public virtual void DeleteFile(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists($"wwwroot/{filePath}"))
                File.Delete($"wwwroot/{filePath}");
        }
    }
}
