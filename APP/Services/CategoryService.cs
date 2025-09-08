using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class CategoryService : Service<Category>, IService<CategoryRequest, CategoryResponse>
    {
        public CategoryService(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Category> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(c => c.Products); // include relational products in the query for delete check
        }

        public CommandResponse Create(CategoryRequest request)
        {
            if (Query().Any(c => c.Title == request.Title.Trim()))
                return Error("Category with the same title exists!");
            var entity = new Category
            {
                Title = request.Title?.Trim(),
                Description = request.Description?.Trim()
            };
            Create(entity);
            return Success("Category created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(c => c.Id == id); // isNoTracking is false for being tracked by EF Core to delete the entity
            if (entity is null)
                return Error("Category not found!");

            // Check if there are any relational Product entities data of the category.
            // If any, don't delete the category and return error command response.
            if (entity.Products.Count > 0) // if (entity.Products.Any()) can also be written
                return Error("Category can't be deleted because it has relational products!");
            
            Delete(entity);
            return Success("Category deleted successfully.", entity.Id);
        }

        public CategoryRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(c => c.Id == id);
            if (entity is null)
                return null;
            return new CategoryRequest
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };
        }

        public CategoryResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(c => c.Id == id);
            if (entity is null)
                return null;
            return new CategoryResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Title = entity.Title,
                Description = entity.Description
            };
        }

        public List<CategoryResponse> List()
        {
            return Query().Select(c => new CategoryResponse
            {
                Id = c.Id,
                Guid = c.Guid,
                Title = c.Title,
                Description = c.Description
            }).ToList();
        }

        public CommandResponse Update(CategoryRequest request)
        {
            if (Query().Any(c => c.Id != request.Id && c.Title == request.Title.Trim()))
                return Error("Category with the same title exists!");
            var entity = Query(false).SingleOrDefault(c => c.Id == request.Id); // isNoTracking is false for being tracked by EF Core to update the entity
            if (entity is null)
                return Error("Category not found!");
            entity.Title = request.Title?.Trim();
            entity.Description = request.Description?.Trim();
            Update(entity);
            return Success("Category updated successfully.", entity.Id);
        }
    }
}
