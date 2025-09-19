using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NCTech.Opstech.Oee.Core.Services.EntityServices.Departments.Interface;
using NCTech.Opstech.Oee.SignalR.Domains.Models;

namespace NCTech.Opstech.SignalR.Core.Hub
{
    public class DeviceHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IDeviceEntityService _deviceEntityService;
        private readonly ILogger<DeviceHub> _logger;

        public DeviceHub(IDeviceEntityService deviceEntityService, ILogger<DeviceHub> logger)
        {
            _deviceEntityService = deviceEntityService;
            _logger = logger;
        }

        public async Task RegisterDevice(string tagId)
        {
            try
            {
                var existingEquipment = await _deviceEntityService.GetByTag(tagId)
                    .FirstOrDefaultAsync(e => e.TagId == tagId);

                if (existingEquipment != null)
                {
                    // Atualiza equipment existente
                    existingEquipment.ConnectionId = Context.ConnectionId;
                    existingEquipment.IsConnected = true;
                    existingEquipment.LastActivity = DateTime.UtcNow;
                }
                else
                {
                    // Cria novo Device
                    var device = new Device
                    {
                        TagId = tagId,
                        ConnectionId = Context.ConnectionId,
                        ConnectedAt = DateTime.UtcNow,
                        LastActivity = DateTime.UtcNow,
                        IsConnected = true
                    };
                    _deviceEntityService.Insert(device);
                }

                await _deviceEntityService.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, $"Equipment_{tagId}");
                await Groups.AddToGroupAsync(Context.ConnectionId, "Equipments");
                
                _logger.LogInformation($"Equipment {tagId} registered with connection {Context.ConnectionId}");
                
                // Notifica apenas os servidores/monitores sobre o novo equipamento conectado
                await Clients.Group("Servers").SendAsync("EquipmentConnected", tagId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error registering equipment {tagId}");
                throw;
            }
        }

        //public async Task RegisterServer(string serverId)
        //{
        //    try
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, "Servers");
        //        _logger.LogInformation($"Server {serverId} registered with connection {Context.ConnectionId}");
                
        //        // Envia lista de equipamentos conectados para o servidor
        //        var connectedEquipments = await _context.Equipments
        //            .Where(e => e.IsConnected)
        //            .Select(e => e.TagId)
        //            .ToListAsync();
                
        //        await Clients.Caller.SendAsync("ConnectedEquipments", connectedEquipments);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error registering server {serverId}");
        //        throw;
        //    }
        //}

        //public async Task SendMessageToServer(string message)
        //{
        //    try
        //    {
        //        var equipment = await _context.Equipments
        //            .FirstOrDefaultAsync(e => e.ConnectionId == Context.ConnectionId);

        //        if (equipment != null)
        //        {
        //            var messageEntity = new Message
        //            {
        //                TagId = equipment.TagId,
        //                Content = message,
        //                Type = MessageType.FromEquipment,
        //                Timestamp = DateTime.UtcNow
        //            };

        //            _context.Messages.Add(messageEntity);
        //            equipment.LastActivity = DateTime.UtcNow;
        //            await _context.SaveChangesAsync();

        //            _logger.LogInformation($"Message received from equipment {equipment.TagId}: {message}");

        //            // Envia mensagem apenas para servidores/monitores, não para outros equipamentos
        //            await Clients.Group("Servers").SendAsync("MessageFromEquipment", equipment.TagId, message, DateTime.UtcNow);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error processing message from equipment");
        //        throw;
        //    }
        //}

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
