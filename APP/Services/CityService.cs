using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class CityService : Service<City>, IService<CityRequest, CityResponse>
    {
        public CityService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<City> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(c => c.Country).OrderBy(c => c.Country.CountryName).ThenBy(c => c.CityName);
        }

        public CommandResponse Create(CityRequest request)
        {
            if (Query().Any(c => c.CityName == request.CityName.Trim()))
                return Error("City with the same name exists!");
            var city = new City
            {
                CityName = request.CityName?.Trim(),
                CountryId = request.CountryId ?? 0
            };
            Create(city);
            return Success("City created successfully.", city.Id);
        }

        public CommandResponse Delete(int id)
        {
            var city = Query(false).SingleOrDefault(c => c.Id == id); // isNoTracking is false for being tracked by EF Core to delete the entity
            if (city is null)
                return Error("City not found!");
            Delete(city);
            return Success("City deleted successfully.", city.Id);
        }

        public CityRequest Edit(int id)
        {
            var city = Query().SingleOrDefault(c => c.Id == id);
            if (city is null)
                return null;
            return new CityRequest
            {
                Id = city.Id,
                CityName = city.CityName,
                CountryId = city.CountryId
            };
        }

        public CityResponse Item(int id)
        {
            var city = Query().SingleOrDefault(c => c.Id == id);
            if (city is null)
                return null;
            return new CityResponse
            {
                Id = city.Id,
                Guid = city.Guid,
                CityName = city.CityName,
                Country = new CountryResponse
                {
                    Id = city.Country.Id,
                    Guid = city.Country.Guid,
                    CountryName = city.Country.CountryName
                }
            };
        }

        public List<CityResponse> List()
        {
            return Query().Select(city => new CityResponse
            {
                Id = city.Id,
                Guid = city.Guid,
                CityName = city.CityName,
                Country = new CountryResponse
                {
                    Id = city.Country.Id,
                    Guid = city.Country.Guid,
                    CountryName = city.Country.CountryName
                }
            }).ToList();
        }

        public CommandResponse Update(CityRequest request)
        {
            if (Query().Any(c => c.Id != request.Id && c.CityName == request.CityName.Trim()))
                return Error("City with the same name exists!");
            var city = Query(false).SingleOrDefault(c => c.Id == request.Id); // isNoTracking is false for being tracked by EF Core to update the entity
            if (city is null)
                return Error("City not found!");
            city.CityName = request.CityName?.Trim();
            city.CountryId = request.CountryId ?? 0;
            Update(city);
            return Success("City updated successfully.", city.Id);
        }



        // get a filtered list of City response items filtered by country ID,
        // if countryId is null, return an empty City response list
        public List<CityResponse> List(int? countryId)
        {
            // if countryId is null, return an empty City response list
            if (!countryId.HasValue) // if (countryId == null) or if (countryId is null) may also be written
                return new List<CityResponse>();

            // filter the City entity query by countryId value and project the City entity query to City response query then return the list
            return Query().Where(c => c.CountryId == countryId.Value).Select(c => new CityResponse
            {
                Id = c.Id,
                Guid = c.Guid,
                CityName = c.CityName
            }).ToList();
        }
    }
}
