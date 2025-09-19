using NCTech.Opstech.Core.PostgressCore.Domain;
namespace NCTech.Opstech.Oee.SignalR.Domains.Models
{
    public class DeviceMessage : Entity<int>
    {
        public string IdMessage { get; set; }
        public string IdDevice { get; set; }
        public string IdEquipment { get; set; }
        public string PayloadJson { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime SentAt { get; set; }
    }
}

