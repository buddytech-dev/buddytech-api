namespace BuddyTech.API.Models
{
    public class SellerMission
    {
        public Guid Id { get; set; }

        public Guid SellerId { get; set; }
        public Seller Seller { get; set; }

        public Guid MissionId { get; set; }
        public Mission Mission { get; set; }

        public DateTime AssignedDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public SellerMission() { }
    }
}
