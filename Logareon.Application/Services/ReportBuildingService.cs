using Logareon.Application.Abstractions;
using Logareon.Application.Models;
using Logareon.Domain.Repository;

namespace Logareon.Application.Services
{
    public class ReportBuildingService : IReportBuildingService
    {
        private readonly IRequestIdentifierService _requestIdentifierService;
        private readonly IReportBuilder _reportBuilderRepository;
        private readonly IReporter _reporter;
        private Dictionary<int, CancellationTokenSource> _reportTasks = new Dictionary<int, CancellationTokenSource>();

        public ReportBuildingService(IRequestIdentifierService requestIdentifierService, IReportBuilder reportBuilderRepository, IReporter reporter)
        {
            _requestIdentifierService = requestIdentifierService;
            _reportBuilderRepository = reportBuilderRepository;
            _reporter = reporter;
        }

        public async Task<int> BuildReportAsync()
        {
            int requestId = _requestIdentifierService.GenerateRequestId();
            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            _reportTasks.Add(requestId, cancellationTokenSource);

            try
            {
                var reportTask = Task.Run(async () =>
                {
                    var report = await _reportBuilderRepository.Build();
                    cancellationToken.ThrowIfCancellationRequested();
                    return report;
                }, cancellationToken);

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
                    return -1; // Возвращаем отрицательное значение в случае ошибки
                }
                else
                {
                    _reporter.ReportTimeout(requestId);
                    return -2; // Возвращаем другое отрицательное значение в случае таймаута
                }
            }
            catch (Exception ex)
            {
                _reporter.ReportError(requestId);
                return -1; // Возвращаем отрицательное значение в случае ошибки
            }
        }

        public StopReportResult StopReport(int id)
        {
            if (_reportTasks.TryGetValue(id, out CancellationTokenSource cancellationTokenSource))
            {
                cancellationTokenSource.Cancel();
                _reportTasks.Remove(id);
                return new StopReportResult { IsSuccess = true, Message = $"Report build request with ID {id} stopped successfully." };
            }

            return new StopReportResult { IsSuccess = false, Message = $"Report build request with ID {id} not found." };
        }

    }
}
