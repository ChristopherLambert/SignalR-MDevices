using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NCTech.Opstech.Oee.Core.Services.EntityServices.Departments;
using NCTech.Opstech.Oee.Core.Services.EntityServices.Departments.Interface;
using NCTech.Opstech.Oee.SignalR.Domains.Models;

namespace NCTech.Opstech.SignalR.Core.Hub
{
    public class DeviceHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IDeviceEntityService _deviceEntityService;
        private readonly IDeviceMessageEntityService _deviceMessageEntityService;
        private readonly ILogger<DeviceHub> _logger;

        public DeviceHub(IDeviceEntityService deviceEntityService,
            IDeviceMessageEntityService deviceMessageEntityService, ILogger<DeviceHub> logger)
        {
            _deviceEntityService = deviceEntityService;
            _deviceMessageEntityService = deviceMessageEntityService;
            _logger = logger;
        }

        public async Task RegisterDevice(string tagId)
        {
            try
            {
                var existingDevice = await _deviceEntityService.GetByTag(tagId);
                if (existingDevice != null)
                {
                    // Atualiza device existente
                    existingDevice.IdConnection = Context.ConnectionId;
                    existingDevice.IsConnected = true;
                    //existingDevice.LastActivity = DateTime.UtcNow;
                }
                else
                {
                    // Cria novo Device
                    var device = new Device
                    {
                        Tag = tagId,
                        IdConnection = Context.ConnectionId,
                        //ConnectedAt = DateTime.UtcNow,
                        //LastActivity = DateTime.UtcNow,
                        IsConnected = true
                    };
                    _deviceEntityService.Insert(device);
                }

                await _deviceEntityService.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, $"Device with Equipment_{tagId}");
                await Groups.AddToGroupAsync(Context.ConnectionId, "Devices");
                
                _logger.LogInformation($"Device with Equipment {tagId} registered with connection {Context.ConnectionId}");
                
                // Notifica apenas os servidores/monitores sobre o novo device conectado
                await Clients.Group("Servers").SendAsync("Device with EquipmentConnected", tagId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error registering Device {tagId}");
                throw;
            }
        }

       public async Task RegisterServer(string serverId)
       {
           try
           {
               await Groups.AddToGroupAsync(Context.ConnectionId, "Servers");
               _logger.LogInformation($"Server {serverId} registered with connection {Context.ConnectionId}");
             
               // Envia lista de devices conectados para o servidor
               var connectedDevices = await _deviceEntityService.GetAll()
                   .Where(e => e.IsConnected)
                   .Select(e => e.Tag)
                   .ToListAsync();
             
               await Clients.Caller.SendAsync("ConnectedDevices", connectedDevices);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, $"Error registering server {serverId}");
               throw;
           }
       }

        public async Task SendMessageToServer(string message)
        {
            try
            {
                var device = await _deviceEntityService.GetByIdConnection(Context.ConnectionId);
                if (device != null)
                {
                    var messageEntity = new DeviceMessage
                    {
                        // TagId = device.Tag,
                        PayloadJson = message,
                        // Type = MessageType.FromEquipment,
                        // Timestamp = DateTime.UtcNow
                    };

                    _deviceMessageEntityService.Insert(messageEntity);
                    // device.LastActivity = DateTime.UtcNow;
                    await _deviceMessageEntityService.SaveChangesAsync();

                    _logger.LogInformation($"Message received from " +
                        $"device to equipment {device.Tag}: {message}");

                    // Envia mensagem apenas para servidores/monitores, não para outros devices
                    await Clients.Group("Servers").SendAsync("MessageFromDevice", device.Tag, message, DateTime.UtcNow);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from equipment");
                throw;
            }
        }

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    try
        //    {
        //        var equipment = await _context.Equipments
        //            .FirstOrDefaultAsync(e => e.ConnectionId == Context.ConnectionId);

        //        if (equipment != null)
        //        {
        //            equipment.IsConnected = false;
        //            equipment.LastActivity = DateTime.UtcNow;
        //            await _context.SaveChangesAsync();

        //            _logger.LogInformation($"Equipment {equipment.TagId} disconnected");

        //            // Notifica apenas os servidores/monitores sobre a desconexão
        //            await Clients.Group("Servers").SendAsync("EquipmentDisconnected", equipment.TagId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error handling disconnection");
        //    }

        //    await base.OnDisconnectedAsync(exception);
        //}
    }
}
