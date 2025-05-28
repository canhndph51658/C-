using FinancialApi.DTOs;
using FinancialApi.Models;

namespace FinancialApi.Services
{
    public class ReportService : IReportService
    {
        private readonly List<Transaction> _transactions;
        private readonly List<Department> _departments;

        public ReportService(List<Transaction> transactions, List<Department> departments)
        {
            _transactions = transactions;
            _departments = departments;
        }

        public CashFlowReportDto GetCashFlowReport(int month, int? departmentId = null)
        {
            var filtered = _transactions.Where(t => t.Date.Month == month);

            if (departmentId.HasValue)
                filtered = filtered.Where(t => t.DepartmentId == departmentId.Value);

            var totalIncome = filtered.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var totalExpense = filtered.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

            var budgetVariance = new Dictionary<int, decimal>();
            foreach (var dept in _departments)
            {
                var deptExpense = _transactions
                    .Where(t => t.DepartmentId == dept.Id && t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount);
                budgetVariance[dept.Id] = dept.BudgetLimit - deptExpense;
            }

            return new CashFlowReportDto
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                NetProfit = totalIncome - totalExpense,
                BudgetVariance = budgetVariance
            };
        }

        public Dictionary<int, decimal> GetBudgetVariance()
        {
            var variance = new Dictionary<int, decimal>();
            foreach (var dept in _departments)
            {
                var deptExpense = _transactions
                    .Where(t => t.DepartmentId == dept.Id && t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount);
                variance[dept.Id] = dept.BudgetLimit - deptExpense;
            }
            return variance;
        }
    }
}
