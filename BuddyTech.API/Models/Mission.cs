using BuddyTech.API.Enums;

namespace BuddyTech.API.Models
{
    public class Mission
    {
        public Guid Id { get; set; }
        MissionTitles Title { get; set; }
        MissionDescriptions Description { get; set; }
        public int PointsGiven { get; set; }
        ICollection<SellerMission> Sellers { get; set; }
    }
}
