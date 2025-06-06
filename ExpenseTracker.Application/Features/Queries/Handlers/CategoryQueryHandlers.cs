using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Interfaces;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{
    public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetByIdQueryHandler<GetCategoryByIdQuery, CategoryDto, Category>(unitOfWork, mapper)
    {
        protected override int GetEntityId(GetCategoryByIdQuery query) => query.Id;
        
        protected override async Task<Category?> GetEntityByIdAsync(int id) => 
            await UnitOfWork.Categories.GetByIdAsync(id);
    }

    public class GetCategoriesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetCategoriesByUserIdQuery, CategoryDto, Category>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Category>> GetEntitiesAsync(GetCategoriesByUserIdQuery query) =>
            await UnitOfWork.Categories.GetByUserIdAsync(query.UserId);
    }

    public class GetCategoriesByTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetCategoriesByTypeQuery, CategoryDto, Category>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Category>> GetEntitiesAsync(GetCategoriesByTypeQuery query)
        {
            if (Enum.TryParse<CategoryType>(query.Type, out var categoryType))
            {
                return await UnitOfWork.Categories.GetByTypeAsync(categoryType);
            }
            return [];
        }
    }

    public class GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : BaseGetCollectionQueryHandler<GetAllCategoriesQuery, CategoryDto, Category>(unitOfWork, mapper)
    {
        protected override async Task<IEnumerable<Category>> GetEntitiesAsync(GetAllCategoriesQuery query) =>
            await UnitOfWork.Categories.GetAllAsync();
    }
}
