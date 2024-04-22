using Dapper;
using Nexum_Tech.DAO.Interfaces;
using System;

namespace Nexum_Tech.DAO
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
