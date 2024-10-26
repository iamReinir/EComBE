using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using EComBusiness.Entity;
using EComBusiness.HelperModel;
using System.Threading.Tasks;
using System;
using System.Text;
using ECom;
using Microsoft.AspNetCore.Authorization;
using ECom.Service;
using ECom.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;

namespace EComBusiness.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EComContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(EComContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                return BadRequest(new { Message = "Email is already taken." });
            var hashedPassword = PasswordHelper.HashPassword(model.Password);

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                PasswordHash = hashedPassword,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Role = UserRole.Customer
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
                return Unauthorized(new { Message = "Invalid credentials." });

            var token = GenerateJwtToken(user);

            return Ok(new { Token = token, User = user});
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            //    var key = new RsaSecurityKey(KeyHelper.GetPrivateKey());
            //    var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Trang",
                audience: "Jade",
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
