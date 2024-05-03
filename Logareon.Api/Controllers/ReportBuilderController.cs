using Logareon.Application.Abstractions;
using Logareon.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Logareon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportBuilderController : ControllerBase
    {
        private readonly IReportBuildingService _reportBuildingService;

        public ReportBuilderController(IReportBuildingService reportBuilderService)
        {
            _reportBuildingService = reportBuilderService;
        }

        [HttpGet("BUILD")]
        public async Task<ActionResult<int>> Build()
        {
            return await _reportBuildingService.BuildReportAsync();
        }

        [HttpPost("STOP")]
        public ActionResult<StopReportResult> Stop([FromBody] int id)
        {
            var stopResult = _reportBuildingService.StopReport(id);
            return stopResult;
        }
    }
}
