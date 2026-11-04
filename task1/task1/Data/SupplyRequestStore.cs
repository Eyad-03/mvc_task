using task1.Models;

namespace task1.Data
{
    // In-memory List that stores all confirmed supply requests for the lifetime of the application
    public static class SupplyRequestStore
    {
        private static readonly object _lock = new object();
        private static int _nextId = 1;

        public static List<SupplyRequest> ConfirmedRequests { get; } = new List<SupplyRequest>();

        public static SupplyRequest Add(SupplyRequest request)
        {
            lock (_lock)
            {
                request.RequestId = _nextId++;
                ConfirmedRequests.Add(request);
                return request;
            }
        }

        public static List<SupplyRequest> GetAll()
        {
            lock (_lock)
            {
                return ConfirmedRequests.ToList();
            }
        }
    }
}
