using NCTech.Opstech.Core.PostgressCore.Context;
using NCTech.Opstech.Core.PostgressCore.Infrastructure.Persistence;
using NCTech.Opstech.Oee.Core.Services.EntityServices.Departments.Interface;
using NCTech.Opstech.Oee.SignalR.Domains.Models;

namespace NCTech.Opstech.Oee.Core.Services.EntityServices.Departments;

public class DeviceEntityService : EntityService<Device, int>, IDeviceEntityService
{
    public DeviceEntityService(NCApplicationContext context) : base(context)
    {
    }
}