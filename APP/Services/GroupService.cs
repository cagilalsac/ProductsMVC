using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class GroupService : Service<Group>, IService<GroupRequest, GroupResponse>
    {
        public GroupService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Group> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(g => g.Users);
        }

        public CommandResponse Create(GroupRequest request)
        {
            if (Query().Any(g => g.Title == request.Title.Trim()))
                return Error("Group with the same title exists!");
            var entity = new Group
            {
                Title = request.Title?.Trim()
            };
            Create(entity);
            return Success("Group created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(g => g.Id == id); // isNoTracking is false for being tracked by EF Core to delete the entity
            if (entity is null)
                return Error("Group not found!");
            if (entity.Users.Any())
                return Error("Group can't be deleted because it has relational users!");
            Delete(entity);
            return Success("Group deleted successfully.", entity.Id);
        }

        public GroupRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity is null)
                return null;
            return new GroupRequest
            { 
                Id = entity.Id, 
                Title = entity.Title 
            };
        }

        public GroupResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity is null)
                return null;
            return new GroupResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Title = entity.Title
            };
        }

        public List<GroupResponse> List()
        {
            return Query().Select(g => new GroupResponse
            {
                Id = g.Id,
                Guid = g.Guid,
                Title = g.Title
            }).ToList();
        }

        public CommandResponse Update(GroupRequest request)
        {
            if (Query().Any(g => g.Id != request.Id && g.Title == request.Title.Trim()))
                return Error("Group with the same title exists!");
            var entity = Query(false).SingleOrDefault(g => g.Id == request.Id); // isNoTracking is false for being tracked by EF Core to update the entity
            if (entity is null)
                return Error("Group not found!");
            entity.Title = request.Title?.Trim();
            Update(entity);
            return Success("Group updated successfully.", entity.Id);
        }
    }
}
