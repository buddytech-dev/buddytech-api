using BuddyTech.API.Enums;
using BuddyTech.API.Models;

namespace BuddyTech.API.Services
{
    public interface IMissionService
    {
        Task CheckMissionCompletionAsync(Guid sellerId, MissionTitles triggerTitle);
        Task<IEnumerable<SellerMission>> GetSellerMissionsAsync(Guid sellerId);
    }
}