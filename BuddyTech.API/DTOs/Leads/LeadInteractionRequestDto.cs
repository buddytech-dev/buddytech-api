using System.ComponentModel.DataAnnotations;
using BuddyTech.API.Enums;

namespace BuddyTech.API.DTOs.Leads
{
    public class LeadInteractionRequestDto
    {
        [Required]
        public TypesOfContact TypeOfContact { get; set; }

        [Required(ErrorMessage = "O conteúdo da interação é obrigatório.")]
        [StringLength(500, ErrorMessage = "O conteúdo deve ter no máximo 500 caracteres.")]
        public string InteractionContent { get; set; }
    }
}