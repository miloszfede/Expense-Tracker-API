using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateUserMappings();
            CreateCategoryMappings();
            CreateExpenseMappings();
            CreateIncomeMappings();
            CreateCustomTypeConverters();
        }

        private void CreateUserMappings()
        {
            CreateMap<User, UserDto>();
            
            CreateMap<CreateUserDto, User>()
                .IgnoreBaseEntityProperties()
                .AfterMap((src, dest) =>
                {
                    var now = DateTime.Now;
                    dest.SetTimestamps(now, now);
                });
            
            CreateMap<UpdateUserDto, User>()
                .IgnoreBaseEntityProperties()
                .AfterMap((src, dest) =>
                {
                    dest.UpdateTimestamp();
                });
        }

        private void CreateCategoryMappings()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
            
            CreateMap<CreateCategoryDto, Category>()
                .IgnoreBaseEntityProperties()
                .AfterMap((src, dest) =>
                {
                    if (Enum.TryParse<CategoryType>(src.Type, out var categoryType))
                    {
                        dest.UpdateType(categoryType);
                    }
                });
            
            CreateMap<UpdateCategoryDto, Category>()
                .IgnoreBaseEntityProperties()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }

        private void CreateExpenseMappings()
        {
            CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName));
            
            CreateMap<CreateExpenseDto, Expense>()
                .IgnoreBaseEntityProperties()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore());
            
            CreateMap<UpdateExpenseDto, Expense>()
                .IgnoreBaseEntityProperties()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore());
        }

        private void CreateIncomeMappings()
        {
            CreateMap<Income, IncomeDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName));
            
            CreateMap<CreateIncomeDto, Income>()
                .IgnoreBaseEntityProperties()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore());
            
            CreateMap<UpdateIncomeDto, Income>()
                .IgnoreBaseEntityProperties()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore());
        }

        private void CreateCustomTypeConverters()
        {
            CreateMap<CategoryType, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, CategoryType>().ConvertUsing(src => src.ToCategoryType());
        }
    }
}
