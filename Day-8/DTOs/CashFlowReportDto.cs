using AutoMapper;
using FinancialApi.DTOs;
using FinancialApi.Models;

namespace FinancialApi.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TransactionCreateDto, Transaction>();
            CreateMap<Transaction, TransactionReadDto>();
            // Nếu bạn có thêm Summary DTO
            // CreateMap<Transaction, TransactionSummaryDto>();
        }
    }
       public class CashFlowReportDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetProfit { get; set; }
        public Dictionary<int, decimal> BudgetVariance { get; set; }
    }
}
