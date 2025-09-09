using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class CountryService : Service<Country>, IService<CountryRequest, CountryResponse>
    {
        public CountryService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Country> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(c => c.Cities).OrderBy(c => c.CountryName);
        }

        public CommandResponse Create(CountryRequest request)
        {
            if (Query().Any(c => c.CountryName == request.CountryName.Trim()))
                return Error("Country with the same name exists!");
            var country = new Country
            {
                CountryName = request.CountryName?.Trim()
            };
            Create(country);
            return Success("Country created successfully.", country.Id);
        }

        public CommandResponse Delete(int id)
        {
            var country = Query(false).SingleOrDefault(c => c.Id == id); // isNoTracking is false for being tracked by EF Core to delete the entity
            if (country is null)
                return Error("Country not found!");
            if (country.Cities.Any())
                return Error("Country can't be deleted because it has relational cities!");
            Delete(country);
            return Success("Country deleted successfully.", country.Id);
        }

        public CountryRequest Edit(int id)
        {
            var country = Query().SingleOrDefault(c => c.Id == id);
            if (country is null)
                return null;
            return new CountryRequest
            { 
                Id = country.Id,
                CountryName = country.CountryName
            };
        }

        public CountryResponse Item(int id)
        {
            var country = Query().SingleOrDefault(c => c.Id == id);
            if (country is null)
                return null;
            return new CountryResponse
            {
                Id = country.Id,
                Guid = country.Guid,
                CountryName = country.CountryName,
                Cities = country.Cities.OrderBy(city => city.CityName).Select(city => new CityResponse
                {
                    Id = city.Id,
                    Guid = city.Guid,
                    CityName = city.CityName
                }).ToList()
            };
        }

        public List<CountryResponse> List()
        {
            return Query().Select(country => new CountryResponse
            {
                Id = country.Id,
                Guid = country.Guid,
                CountryName = country.CountryName,
                Cities = country.Cities.OrderBy(city => city.CityName).Select(city => new CityResponse
                {
                    Id = city.Id,
                    Guid = city.Guid,
                    CityName = city.CityName
                }).ToList()
            }).ToList();
        }

        public CommandResponse Update(CountryRequest request)
        {
            if (Query().Any(c => c.Id != request.Id && c.CountryName == request.CountryName.Trim()))
                return Error("Country with the same name exists!");
            var country = Query(false).SingleOrDefault(c => c.Id == request.Id); // isNoTracking is false for being tracked by EF Core to update the entity
            if (country is null)
                return Error("Country not found!");
            country.CountryName = request.CountryName?.Trim();
            Update(country);
            return Success("Country updated successfully.", country.Id);
        }
    }
}
