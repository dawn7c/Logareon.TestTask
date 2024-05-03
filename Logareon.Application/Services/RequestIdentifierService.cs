using Logareon.Application.Abstractions;

namespace Logareon.Application.Services
{
    public class RequestIdentifierService : IRequestIdentifierService
    {
        private  int currentRequestId = -1;
        private readonly object lockObject = new object();

        public int GenerateRequestId()
        {
            lock (lockObject)
            {
                return ++currentRequestId;
            }
        }
    }
}
