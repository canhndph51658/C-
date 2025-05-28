using System.ComponentModel.DataAnnotations;
using FinancialApi.Models;

namespace FinancialApi.DTOs
{
    public class TransactionCreateDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
