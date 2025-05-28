namespace FinancialApi.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BudgetLimit { get; set; }
    }
}
