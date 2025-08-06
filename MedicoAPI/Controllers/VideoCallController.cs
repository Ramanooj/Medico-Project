using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicoAPI.Controllers
{
    [Authorize(Roles = "patient, doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class VideoCallController : ControllerBase
    {
        private readonly AgoraTokenService _tokenService;

        public VideoCallController(AgoraTokenService agoraTokenService)
        {
            _tokenService = agoraTokenService;
        }

        [HttpPost("generate-agora-token")]
        public IActionResult GenerateAgoraToken([FromQuery] string appointmentId)
        {
            var channelName = appointmentId; 
            uint uid = 0; 
            int expirationTimeInSeconds = 3600; 

            var token = _tokenService.GenerateToken(channelName, uid, expirationTimeInSeconds);

            return Ok(new
            {
                token,
                channelName,
                uid
            });
        }

    }
}
