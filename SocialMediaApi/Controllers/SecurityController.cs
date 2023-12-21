using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using SocialMedia.Core.Interfaces;
using SocialMediaApi.Responses;

namespace SocialMediaApi.Controllers
{
    [Authorize(Roles = nameof(RoleType.Administrator))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController(ISecurityService security, IMapper mapper) : ControllerBase
    {
        private readonly ISecurityService _securityService = security;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<IActionResult> Post(SecurityDTO securityDTO)
        {
            var security = _mapper.Map<Security>(securityDTO);

            await _securityService.RegisterSecurity(security);

            securityDTO = _mapper.Map<SecurityDTO>(security);

            var response = new ApiResponse<SecurityDTO>(securityDTO);

            return Ok(response);
        }
    }
}
