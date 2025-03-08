using AutoMapper;
using FinTrack.Api.Domain.Models;
using FinTrack.Api.Service.DTOs.ExpenseCategories;
using FinTrack.Api.Service.DTOs.Expenses;
using FinTrack.Api.Service.DTOs.Users;

namespace FinTrack.Api.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserForUpdateDto>().ReverseMap();
        CreateMap<User, UserForResultDto>().ReverseMap();
        CreateMap<User, UserForCreationDto>().ReverseMap();

        // Expense
        CreateMap<Expense, ExpenseForUpdateDto>().ReverseMap();
        CreateMap<Expense, ExpenseForResultDto>().ReverseMap();
        CreateMap<Expense, ExpenseForCreationDto>().ReverseMap();

        // ExpenseCategory
        CreateMap<ExpenseCategory,ExpenseCategoryForUpdateDto>().ReverseMap();
        CreateMap<ExpenseCategory,ExpenseCategoryForResultDto>().ReverseMap();
        CreateMap<ExpenseCategory, ExpenseCategoryForCreationDto>().ReverseMap();
    }
}
