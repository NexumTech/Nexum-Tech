using NexumTech.Domain.Interfaces;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.Domain.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly IDevicesDAO _devicesDAO;

        public DevicesService(IDevicesDAO devicesDAO) 
        { 
            _devicesDAO = devicesDAO;
        }

        public async Task<bool> CreateDevice(DevicesViewModel devicesViewModel)
        {
            return await _devicesDAO.CreateDevice(devicesViewModel);
        }

        public async Task<IEnumerable<DevicesViewModel>> GetDevices(int? companyId)
        {
            return await _devicesDAO.GetDevices(companyId);
        }

        public async Task<bool> RemoveDevice(DevicesViewModel devicesViewModel)
        {
            return await _devicesDAO.RemoveDevice(devicesViewModel);
        }
    }
}
