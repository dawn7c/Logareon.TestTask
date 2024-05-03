using Logareon.Application.Models;

namespace Logareon.Application.Abstractions
{
    public interface IReportBuildingService
    {
        Task<int> BuildReportAsync();
        StopReportResult StopReport(int id);
    }
}
