
using Logareon.Domain.Repository;
using System.Text;

namespace Logareon.Domain.Models
{
    public class ReportBuilder : IReportBuilder
    {
       
        public async Task<byte[]> Build()
        {
            Random random = new Random();
            int secondsToWait = random.Next(5, 46);
            int totalSeconds = 0;

            try
            {
                for (int i = 0; i < secondsToWait; i++)
                {
                    totalSeconds++;
                    await Task.Delay(1000); // Подождать 1 секунду
                }

                if (random.Next(1, 6) == 1) // Вероятность 20%
                {
                    throw new Exception("Report failed");
                }

                string reportMessage = $"Report built in {totalSeconds} s.";
                return Encoding.UTF8.GetBytes(reportMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error building report: {ex.Message}";
                return Encoding.UTF8.GetBytes(errorMessage);
            }
        }
    }
}
