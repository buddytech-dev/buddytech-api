namespace BuddyTech.API.DTOs
    {
        public class SellerResponseDto
        {
            public Guid SellerId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public string PhoneNumber { get; set; }
            public int TotalPoints { get; set; }
            public int MissionsCompletedCount { get; set; }
            public int TotalLeadsAssigned { get; set; }
            public int LeadsWonCount { get; set; }
            public double ClosingRate { get; set; }
        }
    }