using Dapper;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.Infra.DAO
{
    public class DevicesDAO : IDevicesDAO
    {
        private readonly BaseDatabaseService _baseDatabaseService;
        public DevicesDAO(BaseDatabaseService baseDatabaseService) 
        { 
            _baseDatabaseService = baseDatabaseService;
        }

        public async Task<IEnumerable<DevicesViewModel>> GetDevices(int? companyId)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "SELECT * FROM tb_device WHERE CompanyId = @CompanyId";

                    var devices = await connection.QueryAsync<DevicesViewModel>(sql, new
                    {
                        @CompanyId = companyId,
                    });

                    return devices;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateDevice(DevicesViewModel devicesViewModel)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = @"INSERT INTO tb_device
                                        ([Name]
                                        ,[CompanyId])
                                    VALUES
                                        (@Name
                                        ,@CompanyId)";

                    await connection.QueryAsync(sql, new
                    {
                        @Name = devicesViewModel.Name,
                        @CompanyId = devicesViewModel.CompanyId,
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveDevice(DevicesViewModel devicesViewModel)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "DELETE FROM tb_device WHERE Id = @Id";

                    var employees = await connection.QueryAsync<int>(sql, new
                    {
                        @Id = devicesViewModel.Id,
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
