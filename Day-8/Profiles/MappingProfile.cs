using AutoMapper;
using FinancialApi.Models;
using FinancialApi.DTOs;
namespace FinancialApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TransactionCreateDto, Transaction>();
            CreateMap<Transaction, TransactionReadDto>();
        }
    }
}