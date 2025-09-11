using APP.Models;

namespace APP.Services
{
    public interface ILocationQueryService
    {
        /// <summary>
        /// Retrieves a list of <see cref="LocationQueryResponse"/> objects by performing an inner join
        /// between countries and cities, applying filtering, ordering and paging as specified in the request.
        /// </summary>
        /// <param name="request">The query request containing filter, order and paging options.</param>
        /// <returns>A list of location query responses according to filter, order and paging options.</returns>
        public List<LocationQueryResponse> InnerJoinList(LocationQueryRequest request);

        /// <summary>
        /// Retrieves a list of <see cref="LocationQueryResponse"/> objects by performing a left outer join
        /// between countries and cities, applying filtering, ordering and paging as specified in the request.
        /// </summary>
        /// <param name="request">The query request containing filter, order and paging options.</param>
        /// <returns>A list of location query responses according to filter, order and paging options.</returns>
        public List<LocationQueryResponse> LeftJoinList(LocationQueryRequest request);
    }
}
