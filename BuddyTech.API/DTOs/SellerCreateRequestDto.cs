using BuddyTech.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace BuddyTech.API.DTOs
{
    public class SellerCreateRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
    }
}
