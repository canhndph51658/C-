using AutoMapper;
using FinancialApi.DTOs;
using FinancialApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private static List<Transaction> _transactions = new();
        private static List<Department> _departments = new()
        {
            new Department { Id = 1, Name = "IT", BudgetLimit = 50000 },
            new Department { Id = 2, Name = "Marketing", BudgetLimit = 30000 }
        };

        private readonly IMapper _mapper;

        public TransactionsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateTransaction([FromBody] TransactionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Date > DateTime.UtcNow.Date)
                return BadRequest("Date cannot be in the future.");

            var deptExpense = _transactions
                .Where(t => t.DepartmentId == dto.DepartmentId && t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            var dept = _departments.FirstOrDefault(d => d.Id == dto.DepartmentId);
            if (dept == null)
                return BadRequest("Department not found.");

            if (dto.Type == TransactionType.Expense && (deptExpense + dto.Amount) > dept.BudgetLimit)
                return BadRequest("Expense exceeds department budget limit.");

            var transaction = _mapper.Map<Transaction>(dto);
            transaction.Id = _transactions.Count + 1;

            _transactions.Add(transaction);

            return CreatedAtAction(nameof(GetTransactions), new { id = transaction.Id }, _mapper.Map<TransactionReadDto>(transaction));
        }

        [HttpGet]
        public IActionResult GetTransactions(DateTime? fromDate, DateTime? toDate, int? departmentId, string? category)
        {
            var query = _transactions.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(t => t.Date >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.Date <= toDate.Value);

            if (departmentId.HasValue)
                query = query.Where(t => t.DepartmentId == departmentId.Value);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            var result = query.Select(t => _mapper.Map<TransactionReadDto>(t)).ToList();

            return Ok(result);
        }
    }
}
