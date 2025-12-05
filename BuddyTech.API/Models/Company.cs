using BuddyTech.API.Enums;

namespace BuddyTech.API.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
        public string Industry { get; set; }
        public RevenueRanges RevenueRange { get; set; }
        public Company() { }
    }
}
