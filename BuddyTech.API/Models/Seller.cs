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
        public Roles Role { get; set; }
        public ICollection<Lead> Leads { get; set; }
        public ICollection<SellerMission> Missions { get; set; }
        public Seller() { }
    }
}
