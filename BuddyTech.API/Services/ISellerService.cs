using BuddyTech.API.DTOs;
using BuddyTech.API.Models;

namespace BuddyTech.API.Services
{
    public interface ISellerService
    {
        Task<Seller> CreateSellerAsync(Seller newSeller);
        Task<IEnumerable<Seller>> GetAllSellersAsync();
        Task<Seller?> GetSellerByIdAsync(Guid id);
        Task<Seller?> UpdateSellerAsync(Guid id, SellerUpdateRequestDto dto);
        Task<bool> DeleteSellerAsync(Guid id);
        Task UpdateSellerPointsAsync(Guid sellerId, int pointsChange);
    }
}