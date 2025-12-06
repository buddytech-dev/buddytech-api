using BuddyTech.API.Enums;
using BuddyTech.API.Infra;
using BuddyTech.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BuddyTech.API.Services
{  

    public class MissionService : IMissionService
    {
        private readonly ApplicationDbContext _context;

        public MissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Verifica se as condições de uma missão foram atingidas e a registra.
        /// </summary>
        public async Task CheckMissionCompletionAsync(Guid sellerId, MissionTitles triggerTitle)
        {
            // 1. Verifica se o vendedor já concluiu esta missão (para missões de 'uma vez')
            var existingMission = await _context.SellerMissions
                // O Include(sm => sm.Mission) é necessário para acessar o Title do Enum
                .Include(sm => sm.Mission)
                .AnyAsync(sm => sm.SellerId == sellerId && sm.Mission.Title == triggerTitle && sm.IsCompleted);

            if (existingMission) return; // Já concluída, sai

            // 2. Carrega a definição da missão (a missão deve estar seedada no banco)
            var mission = await _context.Missions
                .FirstOrDefaultAsync(m => m.Title == triggerTitle);

            if (mission == null) return; // Missão não definida no banco

            bool isCompleted = false;

            // 3. Lógica de Verificação da Missão
            if (triggerTitle == MissionTitles.FirstLeadCreated)
            {
                var createdLeads = await _context.Leads.CountAsync(l => l.SellerId == sellerId);
                if (createdLeads >= 1) isCompleted = true;
            }
            else if (triggerTitle == MissionTitles.FiveLeadsCreated)
            {
                var createdLeads = await _context.Leads.CountAsync(l => l.SellerId == sellerId);
                if (createdLeads >= 5) isCompleted = true;
            }
            // Exemplo de como implementar as missões de fechamento (Close)
            else if (triggerTitle == MissionTitles.FirstLeadClosed)
            {
                var closedLeads = await _context.Leads.CountAsync(l => l.SellerId == sellerId && l.Status == LeadStatus.Converted);
                if (closedLeads >= 1) isCompleted = true;
            }
            else if (triggerTitle == MissionTitles.FiveLeadsClosed)
            {
                var closedLeads = await _context.Leads.CountAsync(l => l.SellerId == sellerId && l.Status == LeadStatus.Converted);
                if (closedLeads >= 5) isCompleted = true;
            }

            // 4. Se a missão foi concluída, registra o progresso
            if (isCompleted)
            {
                var sellerMission = new SellerMission
                {
                    Id = Guid.NewGuid(),
                    SellerId = sellerId,
                    MissionId = mission.Id,
                    AssignedDate = DateTime.Now,
                    IsCompleted = true,
                    CompletedDate = DateTime.Now
                };

                _context.SellerMissions.Add(sellerMission);

                // Opcional: Adicionar a pontuação ao vendedor (requer um campo 'Points' em Seller)
                var seller = await _context.Sellers.FindAsync(sellerId);
                // if (seller != null) { seller.Points += mission.PointsGiven; }

                await _context.SaveChangesAsync();
                Console.WriteLine($"Missão '{mission.Title}' concluída por {sellerId}! Pontos: {mission.PointsGiven}");
            }
        }

        /// <summary>
        /// Obtém todas as missões e status de conclusão de um vendedor.
        /// </summary>
        public async Task<IEnumerable<SellerMission>> GetSellerMissionsAsync(Guid sellerId)
        {
            // Retorna as missões, incluindo os dados da Missão (Title, Description, PointsGiven)
            return await _context.SellerMissions
                .Include(sm => sm.Mission)
                .Where(sm => sm.SellerId == sellerId)
                .OrderBy(sm => sm.IsCompleted) // Mostra as não concluídas primeiro
                .ThenByDescending(sm => sm.CompletedDate)
                .ToListAsync();
        }
    }
}