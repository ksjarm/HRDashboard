using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRDashboardApplication.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BackEnd.Model;


namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private IConfiguration _config;

        public UsersController(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet] public IActionResult Get() => Ok(_context.UserList);
        
        [HttpGet("{id}")] public IActionResult GetDetails(int? id) {
        var user = _context.UserList!.Find(id);
        if (user == null) return NotFound();
        return Ok(user);
        }
    
        [HttpPost("login")]
        public IActionResult Login([FromBody] User login){
        var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == login.Username);

        if (dbUser == null) return NotFound();

        if (dbUser.Password != HashPassword(login.Password)) return Unauthorized();

        var token = GenerateJSONWebToken(dbUser);

        return Ok(new{
        Token = token,
        Permissions = dbUser.Permissions // Include permissions in the response
        });
        }


        private string HashPassword(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[0],
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
             var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("permissions", user.Permissions ?? "")
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPut("{id}")]
public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
{
    var existingUser = _context.UserList.FirstOrDefault(user => user.Id == id);

    if (existingUser == null)
    {
        return NotFound();
    }

    // Update properties of the existingUser with the values from updatedUser
    existingUser.Name = updatedUser.Name;
    existingUser.Surname = updatedUser.Surname;
    existingUser.Permissions = updatedUser.Permissions;
    // Add other properties as needed

    _context.SaveChanges();

    return Ok(existingUser);
}


    }
}