using FinancialApi.DTOs;
using FinancialApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("cashflow")]
        public IActionResult GetCashFlowReport([FromQuery] int month, [FromQuery] int? departmentId)
        {
            var report = _reportService.GetCashFlowReport(month, departmentId);
            return Ok(report);
        }

        [HttpGet("budget-variance")]
        public IActionResult GetBudgetVariance()
        {
            var variance = _reportService.GetBudgetVariance();
            return Ok(variance);
        }
    }
}
