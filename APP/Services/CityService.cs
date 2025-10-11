using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.Files.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class CityService : Service<City>, IService<CityRequest, CityResponse>
    {
        private readonly FileServiceBase _fileService;

        public CityService(DbContext db, FileServiceBase fileService) : base(db)
        {
            _fileService = fileService;
        }

        protected override IQueryable<City> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(c => c.Country).OrderBy(c => c.Country.CountryName).ThenBy(c => c.CityName);
        }

        public CommandResponse Create(CityRequest request)
        {
            if (Query().Any(c => c.CityName == request.CityName.Trim()))
                return Error("City with the same name exists!");

            // save city's image file to wwwroot/files folder if an image file is chosen by the user
            var filePath = _fileService.GetFilePath(request.FormFile);
            var fileResponse = _fileService.SaveFile(request.FormFile, filePath);
            if (!fileResponse.IsSuccessful)
                return Error(fileResponse.Message);

            var city = new City
            {
                CityName = request.CityName?.Trim(),
                CountryId = request.CountryId ?? 0,
                FilePath = filePath // set the saved city's image file path if an image file is chosen by the user, otherwise null is set
            };
            Create(city);
            return Success("City created successfully.", city.Id);
        }

        public CommandResponse Delete(int id)
        {
            var city = Query(false).SingleOrDefault(c => c.Id == id); // isNoTracking is false for being tracked by EF Core to delete the entity
            if (city is null)
                return Error("City not found!");

            // delete the city's image file from wwwroot/files folder if an image file exists
            _fileService.DeleteFile(city.FilePath);

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
                },
                FilePath = city.FilePath // city image file's path in wwwroot/files folder
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
                },
                FilePath = city.FilePath // city image file's path in wwwroot/files folder
            }).ToList();
        }

        public CommandResponse Update(CityRequest request)
        {
            if (Query().Any(c => c.Id != request.Id && c.CityName == request.CityName.Trim()))
                return Error("City with the same name exists!");
            var city = Query(false).SingleOrDefault(c => c.Id == request.Id); // isNoTracking is false for being tracked by EF Core to update the entity
            if (city is null)
                return Error("City not found!");

            // save city's new image file to wwwroot/files folder and delete the old image file, then update the City entity's file path
            // with the new image file's path if a new image file is chosen by the user
            var filePath = _fileService.GetFilePath(request.FormFile);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var fileResponse = _fileService.SaveFile(request.FormFile, filePath);
                if (!fileResponse.IsSuccessful)
                    return Error(fileResponse.Message);
                _fileService.DeleteFile(city.FilePath);
                city.FilePath = filePath;
            }

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

        // delete a city's image file by city ID and image file path, set the city entity's FilePath property to null and update the Cities table
        public void DeleteFile(int id, string filePath)
        {
            _fileService.DeleteFile(filePath); // delete the image file from wwwroot/files folder

            // set the city entity's FilePath property to null
            var city = Query(false).SingleOrDefault(c => c.Id == id); // isNoTracking is false for being tracked by EF Core to update the entity
            if (city is not null)
                city.FilePath = null; 

            Update(city); // update the Cities table with no image file
        }
    }
}
