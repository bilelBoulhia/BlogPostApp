using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtcilesServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HelpersController  : ControllerBase
    {
        [Authorize]
        [HttpGet("CheckAuth")]
        public async Task<IActionResult> CheckAuth()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(new
            {

                Message = "Auth working!",
                Username = User.Identity.Name,
                Claims = claims
            });
        }

    }
}
