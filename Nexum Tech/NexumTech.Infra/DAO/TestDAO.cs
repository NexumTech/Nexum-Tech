using Nexum_Tech.Infra.DAO.Interfaces;

namespace Nexum_Tech.Infra.DAO
{
    public class TestDAO : ITestDAO
    {
        private readonly BaseDatabaseService _baseDatabaseService;

        public TestDAO(BaseDatabaseService baseDatabaseService) 
        { 
            _baseDatabaseService = baseDatabaseService;
        }

        public int Teste()
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    return 1;
                }
            } catch (Exception ex) 
            { 
                throw new Exception(ex.Message);
            }
        }
    }
}
