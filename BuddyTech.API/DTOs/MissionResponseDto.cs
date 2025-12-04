namespace BuddyTech.API.DTOs
{
    public class MissionResponseDto
    {
        public Guid MissionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PointsGiven { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}