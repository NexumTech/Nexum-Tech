using Nexum_Tech.Domain.Interfaces;
using Nexum_Tech.Infra.DAO.Interfaces;

namespace NexumTech.Domain.Services
{
    public class TestService : ITest
    {
        private readonly ITestDAO _testDAO;

        public TestService(ITestDAO testDAO)
        {
            _testDAO = testDAO;
        }

        public int Teste()
        {
            return _testDAO.Teste();
        }
    }
}
