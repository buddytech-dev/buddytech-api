using BuddyTech.API.DTOs.Mission;
using BuddyTech.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuddyTech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MissionController : ControllerBase
    {
        private readonly IMissionService _missionService;

        public MissionController(IMissionService missionService)
        {
            _missionService = missionService;
        }

        // GET /api/Mission/seller/{sellerId}
        /// <summary>
        /// Lista as missões de um vendedor e seu status de conclusão.
        /// </summary>
        [HttpGet("seller/{sellerId}")]
        public async Task<IActionResult> GetMissionsBySeller(Guid sellerId)
        {
            var sellerMissions = await _missionService.GetSellerMissionsAsync(sellerId);

            // Mapeia os Models para DTOs
            var response = sellerMissions.Select(sm => new MissionResponseDto
            {
                MissionId = sm.MissionId,
                Title = sm.Mission.Title.ToString(),
                Description = sm.Mission.Description.ToString(),
                PointsGiven = sm.Mission.PointsGiven,
                IsCompleted = sm.IsCompleted,
                CompletedDate = sm.CompletedDate
            }).ToList();

            return Ok(response);
        }

        // Em um projeto real, você precisaria de um endpoint para popular a tabela Missions
        // para que as missões existam antes que o CheckMissionCompletionAsync funcione.

        // Ex: POST /api/Mission/seed (endpoint temporário para criar as missões no DB)
    }
}