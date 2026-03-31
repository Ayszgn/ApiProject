namespace ApiProject.Controllers
{
    using ApiProject.Data;
    using ApiProject.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // REGISTER
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto model)
        {
            if (_context.Users.Any(u => u.UserName == model.UserName))
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcı zaten mevcut!"
                });

            var user = new User
            {
                UserName = model.UserName,
                Password = model.Password, // İstersen hash ekleyebilirsin
                Role = model.Role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = null,
                Message = "Kullanıcı oluşturuldu"
            });
        }

        // LOGIN
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto model)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

            if (user == null)
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcı adı veya şifre hatalı!"
                });

            // JWT token oluştur
            var keyString = _configuration["Jwt:Key"]
                            ?? throw new Exception("JWT Key configuration bulunamadı!");

            var key = Encoding.UTF8.GetBytes(keyString);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"], // opsiyonel
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = jwtToken,
                Message = "Giriş başarılı"
            });
        }
    }
}