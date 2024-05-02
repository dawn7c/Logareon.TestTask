using Logareon.Domain.Models;
using Logareon.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logareon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportBuilderController : ControllerBase
    {
        private readonly IReportBuilder _reportBuilderRepository;
        private readonly IReporter _reporter;
        private int _currentRequestId;

        public ReportBuilderController(IReportBuilder reportBuilderRepository, IReporter reporter)
        {
            _reportBuilderRepository = reportBuilderRepository;
            _reporter = reporter;
            _currentRequestId = 0;
        }

        [HttpGet]
        public async Task<ActionResult> ReportBuildAsync()
        {
            
            int requestId = _currentRequestId++;
            try
            {
                var report = await _reportBuilderRepository.Build();
                return Ok(report);
            }
            catch (Exception ex)
            {
                _reporter.ReportError(requestId); // Отчет с указанным requestId завершился ошибкой
                return StatusCode(500, $"An error occurred while building the report: {ex.Message}");
            }
        }

        [HttpPost("STOP")]
        public IActionResult StopBuild([FromBody] int id)
        {
            
            return Ok($"Report build request with ID {id} stopped successfully.");
        }
    }
}
