using NCTech.Opstech.Core.PostgressCore.Infrastructure.Persistence;
using NCTech.Opstech.Oee.SignalR.Domains.Models;

namespace NCTech.Opstech.Oee.Core.Services.EntityServices.Departments.Interface;

public interface IDeviceEntityService : IEntityService<Device, int>
{
    Task<Device?> GetByName(string name);
    Task<Device?> GetByTag(string tag);
    Task<Device?> GetByIdConnection(string connectionId);
}