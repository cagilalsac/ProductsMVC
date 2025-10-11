namespace CORE.APP.Services.Files.MVC
{
    /// <summary>
    /// Concrete file service built on <see cref="FileServiceBase"/> for handling file operations.
    /// </summary>
    /// <remarks>
    /// Intended for use in service classes by dependency injection to resolve paths, save uploads, and delete files.
    /// Override base methods to customize storage location, allowed extensions, validation or naming strategy.
    /// </remarks>
    public class FileService : FileServiceBase
    {
    }
}
