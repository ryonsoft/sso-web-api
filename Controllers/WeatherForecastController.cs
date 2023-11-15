using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace jwt_auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                return Ok("Authorization header is missing.");
            }
            int spaceIndex = token.IndexOf(' ');
            string part2 = token.Substring(spaceIndex + 1);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(part2);
            var claims = jwtToken.Claims;
            var jsonClaims = new Dictionary<string, string>();
            foreach (var claim in claims)
            {
                jsonClaims[claim.Type] = claim.Value;
            }
            var json = JsonSerializer.Serialize(jsonClaims);
            return Ok(json);
        }
    }
}
