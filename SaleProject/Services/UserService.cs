using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SaleProject.Common;
using SaleProject.Models;
using SaleProject.Models.Request;
using SaleProject.Models.Response;
using SaleProject.Tools;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace SaleProject.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public async Task<UserResponse> Auth(AuthRequest model)
        {
            UserResponse userResponse = new UserResponse();
            try
            {
                using (var db = new SaleProjectContext())
                {
                    string spassword = Encrypt.GetSHA256(model.Password);
                    var user = await db.Users.Where(d => d.Email == model.Email &&
                                                  d.Password == spassword).FirstOrDefaultAsync();
                    if (user == null)
                        return null;

                    userResponse.Email = user.Email;
                    userResponse.Token = GetToken(user);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return userResponse;
        }
        private string GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
