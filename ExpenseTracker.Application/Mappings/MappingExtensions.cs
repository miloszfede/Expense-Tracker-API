using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using System;

namespace ExpenseTracker.Application.Mappings
{
    public static class MappingExtensions
    {
        public static readonly MapperConfiguration MappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<CreateUserDto, User>();
            cfg.CreateMap<UpdateUserDto, User>();

            cfg.CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
            cfg.CreateMap<CreateCategoryDto, Category>();
            cfg.CreateMap<UpdateCategoryDto, Category>();

            cfg.CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
            cfg.CreateMap<CreateExpenseDto, Expense>();
            cfg.CreateMap<UpdateExpenseDto, Expense>();

            cfg.CreateMap<Income, IncomeDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
            cfg.CreateMap<CreateIncomeDto, Income>();
            cfg.CreateMap<UpdateIncomeDto, Income>();
        });

        public static IMapper CreateMapper()
        {
            return MappingConfiguration.CreateMapper();
        }
    }
}
