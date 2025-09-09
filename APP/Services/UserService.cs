using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.Authentication.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class UserService : Service<User>, IService<UserRequest, UserResponse>
    {
        /// <summary>
        /// Service interface for cookie authentication including sign in and sign out methods.
        /// The injected instance in the constructor is assigned to this field to be used in Login and Logout methods below.
        /// </summary>
        private readonly ICookieAuthService _cookieAuthService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// Uses dependency injection to receive the database context and cookie authentication service.
        /// </summary>
        /// <param name="db">
        /// The <see cref="DbContext"/> instance used for database operations, which the injection is managed in the IoC Container of Program.cs.
        /// </param>
        /// <param name="cookieAuthService">
        /// The <see cref="ICookieAuthService"/> instance used for cookie authentication, which the injection is managed in the IoC Container of Program.cs.
        /// </param>
        public UserService(DbContext db, ICookieAuthService cookieAuthService) : base(db)
        {
            // The injected cookie authentication service is assigned to this field to be used in Login and Logout methods below.
            _cookieAuthService = cookieAuthService;
        }



        protected override IQueryable<User> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Include(u => u.Group)
                .Include(u => u.Country)
                .Include(u => u.City)
                .OrderByDescending(u => u.IsActive).ThenBy(u => u.RegistrationDate).ThenBy(u => u.UserName);
        }

        public CommandResponse Create(UserRequest request)
        {
            if (Query().Any(u => u.UserName == request.UserName.Trim() && u.IsActive == request.IsActive))
                return Error("Active user with the same user name exists!");
            var entity = new User
            {
                UserName = request.UserName,
                Password = request.Password,
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                RegistrationDate = DateTime.Now, // set registration date to the current date and time 
                Score = request.Score ?? 0,
                IsActive = request.IsActive,
                Address = request.Address?.Trim(),
                GroupId = request.GroupId,
                RoleIds = request.RoleIds,
                CountryId = request.CountryId,
                CityId = request.CityId
            };
            Create(entity);
            return Success("User created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(u => u.Id == id); // isNoTracking is false for being tracked by EF Core to delete the entity
            if (entity is null)
                return Error("User not found!");
            Delete(entity.UserRoles);
            Delete(entity);
            return Success("User deleted successfully.", entity.Id);
        }

        public UserRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(u => u.Id == id);
            if (entity is null)
                return null;
            return new UserRequest
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Password = entity.Password,
                IsActive = entity.IsActive,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                Score = entity.Score,
                Address = entity.Address,
                GroupId = entity.GroupId,
                RoleIds = entity.RoleIds,
                CountryId = entity.CountryId,
                CityId = entity.CityId
            };
        }

        public UserResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(u => u.Id == id);
            if (entity is null)
                return null;
            return new UserResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                UserName = entity.UserName,
                Password = entity.Password,
                IsActive = entity.IsActive,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                RegistrationDate = entity.RegistrationDate,
                Score = entity.Score,
                Address = entity.Address,
                GroupId = entity.GroupId,
                RoleIds = entity.RoleIds,
                CountryId = entity.CountryId,
                CityId = entity.CityId,

                IsActiveF = entity.IsActive ? "Active" : "Inactive",
                FullName = entity.FirstName + " " + entity.LastName,
                GenderF = entity.Gender.ToString(), // will assign Woman or Man
                BirthDateF = entity.BirthDate.HasValue ? entity.BirthDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                RegistrationDateF = entity.RegistrationDate.ToString("MM/dd/yyyy"),
                ScoreF = entity.Score.ToString("N1"),
                Group = entity.Group != null ? entity.Group.Title : null,
                Roles = entity.UserRoles.Select(ur => ur.Role.Name).ToList(),
                Country = entity.Country != null ? entity.Country.CountryName : string.Empty,
                City = entity.City != null ? entity.City.CityName : string.Empty
            };
        }

        public List<UserResponse> List()
        {
            return Query().Select(u => new UserResponse
            {
                Id = u.Id,
                Guid = u.Guid,
                UserName = u.UserName,
                Password = u.Password,
                IsActive = u.IsActive,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Gender = u.Gender,
                BirthDate = u.BirthDate,
                RegistrationDate = u.RegistrationDate,
                Score = u.Score,
                Address = u.Address,
                GroupId = u.GroupId,
                RoleIds = u.RoleIds,
                CountryId = u.CountryId,
                CityId = u.CityId,

                IsActiveF = u.IsActive ? "Active" : "Inactive",
                FullName = u.FirstName + " " + u.LastName,
                GenderF = u.Gender.ToString(), // will assign Woman or Man
                BirthDateF = u.BirthDate.HasValue ? u.BirthDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                RegistrationDateF = u.RegistrationDate.ToString("MM/dd/yyyy"),
                ScoreF = u.Score.ToString("N1"),
                Group = u.Group != null ? u.Group.Title : null,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
                Country = u.Country != null ? u.Country.CountryName : string.Empty,
                City = u.City != null ? u.City.CityName : string.Empty
            }).ToList();
        }

        public CommandResponse Update(UserRequest request)
        {
            if (Query().Any(u => u.Id != request.Id && u.UserName == request.UserName.Trim() && u.IsActive == request.IsActive))
                return Error("Active user with the same user name exists!");
            var entity = Query(false).SingleOrDefault(u => u.Id == request.Id); // isNoTracking is false for being tracked by EF Core to update the entity
            if (entity is null)
                return Error("User not found!");
            Delete(entity.UserRoles);
            entity.UserName = request.UserName;
            entity.Password = request.Password;
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.Gender = request.Gender;
            entity.BirthDate = request.BirthDate;
            entity.Score = request.Score ?? 0;
            entity.IsActive = request.IsActive;
            entity.Address = request.Address?.Trim();
            entity.GroupId = request.GroupId;
            entity.RoleIds = request.RoleIds;
            entity.CountryId = request.CountryId;
            entity.CityId = request.CityId;
            Update(entity);
            return Success("User updated successfully.", entity.Id);
        }



        // Authentication:
        /// <summary>
        /// Authenticates a user using the provided login credentials and initiates a cookie-based sign-in.
        /// </summary>
        /// <param name="request">
        /// The <see cref="UserLoginRequest"/> containing the user's login information (user name and password).
        /// </param>
        /// <returns>
        /// A <see cref="CommandResponse"/> indicating the result of the login attempt.
        /// Returns an error response if credentials are invalid; otherwise, returns a success response with the user's ID.
        /// Also a cookie named ".AspNetCore.Cookies" is created in the browser to maintain the authenticated session.
        /// </returns>
        public async Task<CommandResponse> Login(UserLoginRequest request)
        {
            /*
            Synchronous methods execute tasks one after another. Each operation must complete before the next one starts. The calling thread 
            waits (or "blocks") until the method finishes.  
            Asynchronous methods allow tasks to run in the background. The calling thread does not wait for the operation to finish and 
            can continue executing other code. In C#, asynchronous methods often use the async and await keywords, enabling non-blocking operations 
            (such as I/O or database calls) and improving application responsiveness.
            */
            // Attempt to find an active user matching the provided user name and password.
            var entity = Query().SingleOrDefault(
                u => u.UserName == request.UserName
                  && u.Password == request.Password
                  && u.IsActive);

            // If no matching user is found, return an error response.
            if (entity is null)
                return Error("Invalid user name or password!");

            // Sign in the user using cookie authentication, passing user ID, user name, and role names.
            await _cookieAuthService.SignIn(
                entity.Id,
                entity.UserName,
                entity.UserRoles.Select(ur => ur.Role.Name).ToArray());

            // Return a success response with the user's ID.
            return Success("User logged in successfully.", entity.Id);
        }

        /// <summary>
        /// Signs out the currently authenticated user by removing the ".AspNetCore.Cookies" authentication cookie.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous sign-out operation.
        /// </returns>
        public async Task Logout()
        {
            // Perform sign out using the cookie authentication service.
            await _cookieAuthService.SignOut();
        }

        /// <summary>
        /// Registers a new user with the default "User" role and "Active" status.
        /// </summary>
        /// <param name="request">
        /// The <see cref="UserRegisterRequest"/> containing the registration data including user name, password and confirm password.
        /// </param>
        /// <returns>
        /// A <see cref="CommandResponse"/> indicating the result of the registration.
        /// </returns>
        public CommandResponse Register(UserRegisterRequest request)
        {
            // Get the Role entity with Name "User" from the Roles table to use its ID below.
            var roleEntity = Query<Role>().SingleOrDefault(r => r.Name == "User");
            if (roleEntity is null)
                return Error("\"User\" role not found!");

            // Create a new User entity with request's user name and password, status "Active" and role "User".
            return Create(new UserRequest
            {
                UserName = request.UserName,
                Password = request.Password,
                IsActive = true,

                // Way 1:
                //RoleIds = new List<int> { roleEntity.Id }
                // Way 2:
                RoleIds = [ roleEntity.Id ]
            });
        }
    }
}
