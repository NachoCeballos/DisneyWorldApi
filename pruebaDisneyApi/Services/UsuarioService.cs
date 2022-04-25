using pruebaDisneyApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using pruebaDisneyApi.Tools;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using pruebaDisneyApi.Models.Common;
using pruebaDisneyApi.Models.Request;
using pruebaDisneyApi.Models.Response;

namespace pruebaDisneyApi.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppSettings _appSettings;

        public UsuarioService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public UsuarioResponse Auth(AuthRequest model)
        {
            UsuarioResponse usuarioResponse = new UsuarioResponse();

            using (DisneyContext db = new DisneyContext())
            {
                string sContraseña = Encrypt.GetSHA256(model.Contraseña);

                var usuario = db.Usuarios.Where(x => x.Contraseña == sContraseña &&
                                                x.Email == model.Email).FirstOrDefault();

                if (usuario == null) return null;

                usuarioResponse.Email = usuario.Email;
                usuarioResponse.Token = GetToken(usuario);
            }

            return (usuarioResponse);
        }

        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Email, usuario.Email.ToString())
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
