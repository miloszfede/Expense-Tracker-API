using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Queries.Handlers
{
    public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(request.Id);
            return category == null ? null : mapper.Map<CategoryDto>(category);
        }
    }

    public class GetCategoriesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetCategoriesByUserIdQuery, IEnumerable<CategoryDto>>
    {
        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.Categories.GetByUserIdAsync(request.UserId);
            return mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }

    public class GetCategoriesByTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetCategoriesByTypeQuery, IEnumerable<CategoryDto>>
    {
        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesByTypeQuery request, CancellationToken cancellationToken)
        {
            if (Enum.TryParse<CategoryType>(request.Type, out var categoryType))
            {
                var categories = await unitOfWork.Categories.GetByTypeAsync(categoryType);
                return mapper.Map<IEnumerable<CategoryDto>>(categories);
            }
            return [];
        }
    }

    public class GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.Categories.GetAllAsync();
            return mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }
}
