using AutoMapper;
using BuddyTech.API.DTOs;
using BuddyTech.API.Enums;
using BuddyTech.API.Models;
using BuddyTech.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuddyTech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;
        private readonly IMapper _mapper;

        public SellerController(ISellerService sellerService, IMapper mapper)
        {
            _sellerService = sellerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSellers()
        {
            var sellers = await _sellerService.GetAllSellersAsync();

            var response = _mapper.Map<List<SellerResponseDto>>(sellers);

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetSeller(Guid id)
        {
            var seller = await _sellerService.GetSellerByIdAsync(id);

            if (seller == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<SellerResponseDto>(seller);
            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSeller([FromBody] SellerCreateRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.IsDefined(typeof(Roles), dto.Role) &&
                !Enum.TryParse<Roles>(dto.Role, true, out _))
            {
                ModelState.AddModelError(nameof(dto.Role), "A função (Role) fornecida é inválida.");
                return BadRequest(ModelState);
            }

            var newSeller = _mapper.Map<Seller>(dto);
            newSeller.Id = Guid.NewGuid();

            var createdSeller = await _sellerService.CreateSellerAsync(newSeller);
            var response = _mapper.Map<SellerResponseDto>(createdSeller);

            return CreatedAtAction(nameof(GetSeller), new { id = response.SellerId }, response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSeller(Guid id, [FromBody] SellerUpdateRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedSeller = await _sellerService.UpdateSellerAsync(id, dto);

            if (updatedSeller == null)
            {
                return NotFound($"Vendedor com ID {id} não encontrado.");
            }

            var response = _mapper.Map<SellerResponseDto>(updatedSeller);

            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSeller(Guid id)
        {
            var success = await _sellerService.DeleteSellerAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}