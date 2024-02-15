using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Interfaces;

namespace SocialMediaApi.Controllers
{
    public class RecoverPasswordController(ISecurityService securityService) : ApiController
    {
        private readonly ISecurityService _securityService = securityService;

        [HttpGet]
        public async Task<IActionResult> RecoverPassword([FromQuery]RecoverPassDTO recoverPassDTO)
        {
            var recover = await _securityService.RecoverPassword(recoverPassDTO);
            return Ok(recover);
        }
    }
}