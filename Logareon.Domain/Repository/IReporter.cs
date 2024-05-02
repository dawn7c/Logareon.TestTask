namespace Logareon.Domain.Repository
{
    public interface IReporter
    {
        public void ReportSuccess(byte[] Data, int Id);
        public void ReportError(int Id);
        public void ReportTimeout(int Id);
    }
}
