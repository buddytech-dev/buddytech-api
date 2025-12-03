using BuddyTech.API.Enums;

namespace BuddyTech.API.Models
{
    public class Mission
    {
        public Guid Id { get; set; }
        public MissionTitles Title { get; set; }
        public MissionDescriptions Description { get; set; }
        public int PointsGiven { get; set; }
        ICollection<SellerMission> Sellers { get; set; }
        public Mission() { }
    }
}
