using BuddyTech.API.Enums;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BuddyTech.API.Models
{
    public class Seller
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        Roles Role { get; set; }
        ICollection<Lead> Leads { get; set; }
        ICollection<SellerMission> Missions { get; set; }
    }
}
