using Nexum_Tech.DAO.Interfaces;
using Nexum_Tech.Domain.Interfaces;

namespace Nexum_Tech.Domain
{
    public class TestDomain : ITest
    {
        private readonly ITestDAO _testDAO;

        public TestDomain(ITestDAO testDAO)
        {
            _testDAO = testDAO;
        }

        public int Teste() 
        {
           return _testDAO.Teste();
        }
    }
}
