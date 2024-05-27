using NexumTech.Infra.Models;

namespace NexumTech.Infra.DAO.Interfaces
{
    public interface IDevicesDAO
    {
        public Task<bool> CreateDevice(DevicesViewModel devicesViewModel);
        public Task<IEnumerable<DevicesViewModel>> GetDevices(int? companyId);
        public Task<bool> RemoveDevice(DevicesViewModel devicesViewModel);
    }
}
