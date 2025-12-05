using BuddyTech.API.DTOs;
using BuddyTech.API.Infra;
using BuddyTech.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BuddyTech.API.Services
{
    public class SellerService : ISellerService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SellerService> _logger;

        public SellerService(ApplicationDbContext context, ILogger<SellerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Seller> CreateSellerAsync(Seller newSeller)
        {
            // TO-DO: fazer hash da senha
            _context.Sellers.Add(newSeller);
            await _context.SaveChangesAsync();
            return newSeller;
        }

        public async Task<IEnumerable<Seller>> GetAllSellersAsync()
        {
            return await _context.Sellers
                .Include(s => s.Leads)
                .Include(s => s.Missions)
                .ToListAsync();
        }

        public async Task<Seller?> GetSellerByIdAsync(Guid id)
        {
            return await _context.Sellers
                .Include(s => s.Leads)
                .Include(s => s.Missions)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Seller?> UpdateSellerAsync(Guid id, SellerUpdateRequestDto dto)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null) return null;

            seller.Name = dto.Name ?? seller.Name;
            seller.Email = dto.Email ?? seller.Email;

            await _context.SaveChangesAsync();
            return seller;
        }

        public async Task<bool> DeleteSellerAsync(Guid id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null) return false;

            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateSellerPointsAsync(Guid sellerId, int pointsChange)
        {
            var seller = await _context.Sellers.FindAsync(sellerId);
            if (seller == null)
            {
                _logger.LogWarning("Vendedor {SellerId} não encontrado para atualização de pontos.", sellerId);
                return;
            }

            seller.CurrentPoints += pointsChange;
            await _context.SaveChangesAsync();
        }
    }
}