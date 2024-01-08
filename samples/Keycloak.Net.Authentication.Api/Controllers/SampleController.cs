using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Keycloak.Net.Authentication.Api.Controllers
{

    [Route("api/")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SampleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("authenticate")]
        [Authorize]
        public IActionResult CheckAuthorization()
        {
            return Ok($"{HttpStatusCode.OK} authenticated");
        }
        [HttpGet("authorize")]
        [Authorize(Roles = "user_role")]
        public IActionResult GetAuthorization() => 
              Ok($"{HttpStatusCode.OK} authorized");
    }
}
