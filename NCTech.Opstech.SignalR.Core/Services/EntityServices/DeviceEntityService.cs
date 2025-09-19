using Microsoft.EntityFrameworkCore;
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
    public async Task<Device?> GetByName(string name)
    {
        IQueryable<Device> query = GetAll()
            .Where(a => a.Name == name);

        var result = await query.FirstOrDefaultAsync();

        return result;
    }

    public async Task<Device?> GetByTag(string tag)
    {
        IQueryable<Device> query = GetAll()
            .Where(a => a.Tag == tag);

        var result = await query.FirstOrDefaultAsync();

        return result;
    }

    public async Task<Device?> GetByIdConnection(string connectionId)
    {
        IQueryable<Device> query = GetAll()
            .Where(a => a.IdConnection == connectionId);

        var result = await query.FirstOrDefaultAsync();

        return result;
    }
}