using System.ComponentModel.DataAnnotations;
using BuddyTech.API.Enums;

namespace BuddyTech.API.DTOs
{
    public class SellerUpdateRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}