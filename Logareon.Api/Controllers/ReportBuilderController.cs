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
        private int _currentRequestId = 0;
        private Dictionary<int, CancellationTokenSource> _reportTasks = new Dictionary<int, CancellationTokenSource>();

        public ReportBuilderController(IReportBuilder reportBuilderRepository, IReporter reporter)
        {
            _reportBuilderRepository = reportBuilderRepository;
            _reporter = reporter;
            
        }

        [HttpGet("BUILD")]
        public async Task<ActionResult<int>> Build()
        {
            
                int requestId = _currentRequestId++;
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                _reportTasks.Add(requestId, cancellationTokenSource);

                try
                {
                    var reportTask = Task.Run(() => _reportBuilderRepository.Build(), cancellationTokenSource.Token);
                    await Task.WhenAny(reportTask, Task.Delay(60000)); 

                    if (reportTask.IsCompletedSuccessfully)
                    {
                        var report = await reportTask;
                        _reporter.ReportSuccess(report, requestId);
                        return requestId;
                    }
                    else if (reportTask.IsFaulted)
                    {
                        _reporter.ReportError(requestId);
                        return StatusCode(500, "An error occurred while building the report.");
                    }
                    else
                    {
                        _reporter.ReportTimeout(requestId);
                        return StatusCode(500, "Report build timed out.");
                    }
                }
                catch (Exception ex)
                {
                    _reporter.ReportError(requestId);
                    return StatusCode(500, $"An error occurred while building the report: {ex.Message}");
                }
            
        }

        [HttpPost("STOP")]
        public IActionResult Stop([FromBody] int id)
        {
            if (_reportTasks.TryGetValue(id, out CancellationTokenSource cancellationTokenSource))
            {
                cancellationTokenSource.Cancel();
                _reportTasks.Remove(id);
                return Ok($"Report build request with ID {id} stopped successfully.");
            }

            return NotFound($"Report build request with ID {id} not found.");
        }
    }
}
