using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class RoleService : Service<Role>, IService<RoleRequest, RoleResponse>
    {
        public RoleService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Role> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(r => r.UserRoles).ThenInclude(ur => ur.User).OrderBy(r => r.Name);
        }

        public CommandResponse Create(RoleRequest request)
        {
            if (Query().Any(r => r.Name == request.Name.Trim()))
                return Error("Role with the same name exists!");
            var entity = new Role
            {
                Name = request.Name?.Trim()
            };
            Create(entity);
            return Success("Role created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(r => r.Id == id); // isNoTracking is false for being tracked by EF Core to delete the entity
            if (entity is null)
                return Error("Role not found!");
            Delete(entity.UserRoles);
            Delete(entity);
            return Success("Role deleted successfully.", entity.Id);
        }

        public RoleRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(r => r.Id == id);
            if (entity is null)
                return null;
            return new RoleRequest
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public RoleResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(r => r.Id == id);
            if (entity is null)
                return null;
            return new RoleResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.Name,
                UserCount = entity.UserRoles.Count,
                Users = string.Join("<br>", entity.UserRoles.Select(ur => ur.User.UserName))
            };
        }

        public List<RoleResponse> List()
        {
            return Query().Select(r => new RoleResponse
            {
                Id = r.Id,
                Guid = r.Guid,
                Name = r.Name,
                UserCount = r.UserRoles.Count,
                Users = string.Join(", ", r.UserRoles.Select(ur => ur.User.UserName))
            }).ToList();
        }

        public CommandResponse Update(RoleRequest request)
        {
            if (Query().Any(r => r.Id != request.Id && r.Name == request.Name.Trim()))
                return Error("Role with the same name exists!");
            var entity = Query(false).SingleOrDefault(r => r.Id == request.Id); // isNoTracking is false for being tracked by EF Core to update the entity
            if (entity is null)
                return Error("Role not found!");
            entity.Name = request.Name?.Trim();
            Update(entity);
            return Success("Role updated successfully.", entity.Id);
        }
    }
}
