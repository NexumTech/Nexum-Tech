using NexumTech.Infra.Models;

namespace NexumTech.Domain.Interfaces
{
    public interface IDevicesService
    {
        public Task<bool> CreateDevice(DevicesViewModel devicesViewModel);
        public Task<IEnumerable<DevicesViewModel>> GetDevices(int? companyId);
        public Task<bool> RemoveDevice(DevicesViewModel devicesViewModel);
    }
}
