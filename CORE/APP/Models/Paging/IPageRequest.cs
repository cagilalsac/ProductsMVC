namespace CORE.APP.Models.Paging
{
    /// <summary>
    /// Defines the contract for a paginated data request.
    /// </summary>
    public interface IPageRequest
    {
        /// <summary>
        /// Gets or sets the current page number (1-based index).
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of records to return per page.
        /// </summary>
        public int CountPerPage { get; set; }

        /// <summary>
        /// Gets or sets the total number of records available (for informational purposes).
        /// JsonIgnore attribute can be defined to ignore this property during JSON serialization for API requests.
        /// </summary>
        public int TotalCountForPaging { get; set; }
    }
}
