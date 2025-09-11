using APP.Domain;
using APP.Models;
using CORE.APP.Extensions;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    /// <summary>
    /// Service class for querying location data (countries and cities) using join operations.
    /// Inherits generic repository operations (Query methods will be used) from Service of type Country.
    /// </summary>
    public class LocationQueryService : Service<Country>, ILocationQueryService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationQueryService"/> class.
        /// </summary>
        /// <param name="db">The database context used for data access.</param>
        public LocationQueryService(DbContext db) : base(db)
        {
        }

        /// <summary>
        /// Retrieves a list of <see cref="LocationQueryResponse"/> objects by performing an inner join
        /// between countries and cities, applying filtering, ordering and paging as specified in the request.
        /// </summary>
        /// <param name="request">The query request containing filter, order and paging options.</param>
        /// <returns>A list of location query responses according to filter, order and paging options.</returns>
        public List<LocationQueryResponse> InnerJoinList(LocationQueryRequest request)
        {
            // Retrieve the queryable collections for countries and cities from the database context.
            var countryQuery = Query();
            var cityQuery = Query<City>();

            // Perform an inner join between countries and cities on the CountryId field.
            // Project the query result into a LocationQueryResponse DTO (Model).
            var innerJoinQuery = from country in countryQuery
                                 join city in cityQuery on country.Id equals city.CountryId
                                 select new LocationQueryResponse
                                 {
                                     CountryId = country.Id,
                                     CountryName = country.CountryName,
                                     CityId = city.Id,
                                     CityName = city.CityName
                                 };

            // Apply ordering based on the requested entity property and direction.
            if (request.OrderEntityPropertyName == nameof(Country.CountryName))
            {
                // Order by country name, descending or ascending.
                if (request.IsOrderDescending)
                    innerJoinQuery = innerJoinQuery.OrderByDescending(location => location.CountryName);
                else
                    innerJoinQuery = innerJoinQuery.OrderBy(location => location.CountryName);
            }
            else if (request.OrderEntityPropertyName == nameof(City.CityName))
            {
                // Order by city name, descending or ascending.
                if (request.IsOrderDescending)
                    innerJoinQuery = innerJoinQuery.OrderByDescending(location => location.CityName);
                else
                    innerJoinQuery = innerJoinQuery.OrderBy(location => location.CityName);
            }

            // Apply filtering by country name if provided in the request.
            // Way 1:
            //if (!string.IsNullOrWhiteSpace(request.CountryName))
            // Way 2: use the string extension method defined in CORE\APP\Extensions\StringExtensions.cs
            if (request.CountryName.HasAny())
            {
                // Filter results to include only those where the country name contains the provided value 
                // (case-sensitive, no white space characters in the beginning and at the end).
                innerJoinQuery = innerJoinQuery.Where(location => location.CountryName.Contains(request.CountryName.Trim()));
            }

            // Apply filtering by city name if provided in the request.
            // Way 1:
            //if (!string.IsNullOrWhiteSpace(request.CityName))
            // Way 2: use the string extension method defined in CORE\APP\Extensions\StringExtensions.cs
            if (request.CityName.HasAny())
            {
                // Filter results to include only those where the city name contains the provided value 
                // (case-sensitive, no white space characters in the beginning and at the end).
                innerJoinQuery = innerJoinQuery.Where(location => location.CityName.Contains(request.CityName.Trim()));
            }

            // Set the total count of filtered records for client-side paging information.
            request.TotalCountForPaging = innerJoinQuery.Count();

            // Apply paging if both PageNumber and CountPerPage are specified and greater than zero.
            if (request.PageNumber > 0 && request.CountPerPage > 0)
            {
                // Calculate the number of records to skip and take for the current page.
                var skipValue = (request.PageNumber - 1) * request.CountPerPage;
                var takeValue = request.CountPerPage;

                // Apply Skip and Take for paging.
                innerJoinQuery = innerJoinQuery.Skip(skipValue).Take(takeValue);
            }

            // Execute the query and return the list of LocationQueryResponse DTOs.
            return innerJoinQuery.ToList();
        }

        /// <summary>
        /// Retrieves a list of <see cref="LocationQueryResponse"/> objects by performing a left outer join
        /// between countries and cities, applying filtering, ordering and paging as specified in the request.
        /// </summary>
        /// <param name="request">The query request containing filter, order and paging options.</param>
        /// <returns>A list of location query responses according to filter, order and paging options.</returns>
        public List<LocationQueryResponse> LeftJoinList(LocationQueryRequest request)
        {
            // Retrieve the queryable collections for countries and cities from the database context.
            var countryQuery = Query();
            var cityQuery = Query<City>();

            // Perform a left outer join between countries and cities on the CountryId field.
            // Project the query result into a LocationQueryResponse DTO (Model).
            var leftJoinQuery = from country in countryQuery
                                join city in cityQuery on country.Id equals city.CountryId into countryCity
                                from city in countryCity.DefaultIfEmpty() // left outer join to include countries without cities
                                select new LocationQueryResponse
                                {
                                    CountryId = country.Id,
                                    CountryName = country.CountryName,
                                    CityId = city.Id,
                                    CityName = city.CityName
                                };

            // Apply ordering based on the requested entity property and direction.
            if (request.OrderEntityPropertyName == nameof(Country.CountryName))
            {
                // Order by country name, descending or ascending.
                if (request.IsOrderDescending)
                    leftJoinQuery = leftJoinQuery.OrderByDescending(location => location.CountryName);
                else
                    leftJoinQuery = leftJoinQuery.OrderBy(location => location.CountryName);
            }
            else if (request.OrderEntityPropertyName == nameof(City.CityName))
            {
                // Order by city name, descending or ascending.
                if (request.IsOrderDescending)
                    leftJoinQuery = leftJoinQuery.OrderByDescending(location => location.CityName);
                else
                    leftJoinQuery = leftJoinQuery.OrderBy(location => location.CityName);
            }

            // Apply filtering by country name if provided in the request.
            // Way 1:
            //if (!string.IsNullOrWhiteSpace(request.CountryName))
            // Way 2: use the string extension method defined in CORE\APP\Extensions\StringExtensions.cs
            if (request.CountryName.HasAny())
            {
                // Filter results to include only those where the country name contains the provided value 
                // (case-sensitive, no white space characters in the beginning and at the end).
                leftJoinQuery = leftJoinQuery.Where(location => location.CountryName.Contains(request.CountryName.Trim()));
            }

            // Apply filtering by city name if provided in the request.
            // Way 1:
            //if (!string.IsNullOrWhiteSpace(request.CityName))
            // Way 2: use the string extension method defined in CORE\APP\Extensions\StringExtensions.cs
            if (request.CityName.HasAny())
            {
                // Filter results to include only those where the city name contains the provided value 
                // (case-sensitive, no white space characters in the beginning and at the end).
                leftJoinQuery = leftJoinQuery.Where(location => location.CityName.Contains(request.CityName.Trim()));
            }

            // Set the total count of filtered records for client-side paging information.
            request.TotalCountForPaging = leftJoinQuery.Count();

            // Apply paging if both PageNumber and CountPerPage are specified and greater than zero.
            if (request.PageNumber > 0 && request.CountPerPage > 0)
            {
                // Calculate the number of records to skip and take for the current page.
                var skipValue = (request.PageNumber - 1) * request.CountPerPage;
                var takeValue = request.CountPerPage;

                // Apply Skip and Take for paging.
                leftJoinQuery = leftJoinQuery.Skip(skipValue).Take(takeValue);
            }

            // Execute the query and return the list of LocationQueryResponse DTOs.
            return leftJoinQuery.ToList();
        }
    }
}
