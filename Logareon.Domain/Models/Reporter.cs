
using Logareon.Domain.Repository;

namespace Logareon.Domain.Models
{
    public class Reporter : IReporter
    {
        
        public void ReportError(int Id)
        {
            string fileName = $"Error_{Id}.txt";
            try
            {
                string errorMessage = "Report error";
                File.WriteAllText(fileName, errorMessage); // Запись текста в файл
                Console.WriteLine($"Error report successfully written to {fileName}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error writing error report to {fileName}: {e.Message}");
            }
        }

        public void ReportSuccess(byte[] Data, int Id)
        {
            string fileName = $"Report_{Id}.txt";
            try
            {
                File.WriteAllBytes(fileName, Data); // Запись массива байтов в файл
                Console.WriteLine($"Report successfully written to {fileName}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error writing report to {fileName}: {e.Message}");
            }
        }

        public void ReportTimeout(int Id)
        {
            string fileName = $"Timeout_{Id}.txt";
            try
            {
                string timeoutMessage = "Report timeout";
                File.WriteAllText(fileName, timeoutMessage); // Запись текста в файл
                Console.WriteLine($"Timeout report successfully written to {fileName}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error writing timeout report to {fileName}: {e.Message}");
            }
        }
    }
}
