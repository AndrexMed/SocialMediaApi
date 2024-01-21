using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Interfaces;
using SocialMediaApi.Responses;

namespace SocialMediaApi.Controllers
{
    //[Authorize(Roles = nameof(RoleType.Administrator))]
    public class SecurityController(ISecurityService security,
                                        IMapper mapper,
                                            IPasswordService passwordService) : ApiController
    {
        private readonly ISecurityService _securityService = security;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordService _passwordService = passwordService;

        [HttpPost]
        public async Task<IActionResult> Post(SecurityAndUserDTO securityDTO)
        {
            var security = _mapper.Map<Security>(securityDTO);
            var user = _mapper.Map<User>(securityDTO);

            security.Password = _passwordService.Hash(security.Password);

            await _securityService.RegisterUserAndSecurity(security, user);

            //securityDTO = _mapper.Map<SecurityAndUserDTO>(security);

            //var response = new ApiResponse<SecurityAndUserDTO>(securityDTO);
            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }
    }
}