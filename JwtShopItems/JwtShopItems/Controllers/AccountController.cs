using Microsoft.AspNetCore.Mvc;
using JwtShopItems.Data;
using JwtShopItems.Helpers;
using JwtShopItems.Models.Authentication;
using JwtShopItems.Models;

namespace JwtShopItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettings jwtSettings;
        private readonly IEnumerable<User> users;
        protected readonly object userNotFound = new
        {
            statusCode = 404,
            message = "User not found"
        };

        public AccountController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
            users = Users.GetUsers();
        }

        [HttpPost]
        public IActionResult GetToken(UserLogin userLogins)
        {
            var isValid = users.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
            if (isValid)
            {
                var user = users.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                return Ok(JwtHelper.GenerateTokenKey(new UserToken()
                {
                    EmailId = user!.Email,
                    GuidId = Guid.NewGuid(),
                    UserName = user.UserName,
                    Id = user.Id,
                }, jwtSettings));
            }
            else
            {
                NotFound(userNotFound);
            }

            return BadRequest($"Wrong password");
        }
    }
}
