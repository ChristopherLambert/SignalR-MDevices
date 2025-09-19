using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NCTech.Opstech.SignalR.Core.Hub;

namespace NCTech.Opstech.SignalR.Core.Services.BizServices
{
    public class EquipmentService
    {
        private readonly IHubContext<DeviceHub> _hubContext;
        private readonly ILogger<EquipmentService> _logger;
    }
}
