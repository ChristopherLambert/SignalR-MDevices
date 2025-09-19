using NCTech.Opstech.Core.PostgressCore.Domain;
namespace NCTech.Opstech.Oee.SignalR.Domains.Models
{
    public class Device : Entity<int>
    {
        public string IdDevice { get; set; }
        public string IdEquipment { get; set; }
        public string IdConnection { get; set; }
        public string IdMAC { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsConnected { get; set; }
    }
}

