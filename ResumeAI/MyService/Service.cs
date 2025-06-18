using ResumeAI.Interfaces;

namespace ResumeAI.MyService
{
    public class Service : IMyService
    {
        Guid _operationId;
        public Service()
        {
            _operationId = Guid.NewGuid();
        }
        public Guid GetOperationId()
        {
            return _operationId;
        }
    }
}
