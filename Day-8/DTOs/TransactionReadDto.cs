using FinancialApi.Models;

namespace FinancialApi.DTOs
{
    public class TransactionReadDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DepartmentId { get; set; }
    }
}
