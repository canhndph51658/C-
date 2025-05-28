using FinancialApi.DTOs;
using FinancialApi.Models;

namespace FinancialApi.Services
{
    public interface IReportService
    {
        CashFlowReportDto GetCashFlowReport(int month, int? departmentId = null);
        Dictionary<int, decimal> GetBudgetVariance();
    }
}
