using EAppointment.Application.Services;
using EAppointment.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EAppointment.Infrastructure.Services
{
    internal sealed class JwtProvider : IJwtProvider
    {
        public string CreateToken(AppUser user)
        {
            List<Claim> claims = new()
            {
                 new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                 new Claim(ClaimTypes.Name,user.FullName),
                 new Claim(ClaimTypes.Email,user.Email?? string.Empty),
                 new Claim("UserName",user.UserName??string.Empty),
            };
            DateTime expires=DateTime.Now.AddDays(1);
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(string.Join("-", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);
            
            JwtSecurityToken SecurityToken = new(
               issuer: "Ali Can Yücel", // kimin tarfınan olusturulduı
               audience: "eAppointment", // kim kullancak
               claims: claims,    // body olaraktan birden fazla değr ekleyebiliyoruz user adı mail adresi 
               notBefore: DateTime.Now,// tokeninne zamandan sonra kullanılacak 
               expires: expires, //geçerlilik süresi
               signingCredentials:signingCredentials // şireleme turu anhtar
                ); 
            JwtSecurityTokenHandler handler = new();
            string token = handler.WriteToken(SecurityToken);
            return token;
        }
    }
}
