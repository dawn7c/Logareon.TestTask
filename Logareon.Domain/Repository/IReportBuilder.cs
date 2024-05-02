
namespace Logareon.Domain.Repository
{
    public interface IReportBuilder
    {
        Task<byte[]> Build(); 
    }
}
