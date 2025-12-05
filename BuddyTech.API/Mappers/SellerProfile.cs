using AutoMapper;
using BuddyTech.API.DTOs;
using BuddyTech.API.Models;

namespace BuddyTech.API.Mappers
{
    public class SellerProfile : Profile
    {
        public SellerProfile()
        {
            CreateMap<SellerCreateRequestDto, Seller>();

            CreateMap<Seller, SellerResponseDto>()
                .ForMember(dest => dest.Role,
                           opt => opt.MapFrom(src => src.Role.ToString()))

                // Exemplo de mapeamento customizado para dados agregados:
                .ForMember(dest => dest.MissionsCompletedCount,
                           opt => opt.MapFrom(src => src.Missions.Count(sm => sm.IsCompleted)));
            // Nota: Isso requer que a coleção Missions esteja carregada (Include)
        }
    }
}