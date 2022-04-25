using pruebaDisneyApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pruebaDisneyApi.Services;
using pruebaDisneyApi.Tools;
using System;
using System.Linq;
using pruebaDisneyApi.Models.Request;
using pruebaDisneyApi.Models.Response;

namespace pruebaDisneyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest model)
        {
            Respuesta respuesta = new Respuesta();

            var usuarioResponse = _usuarioService.Auth(model);

            if (usuarioResponse == null)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = "Usuario o Contraseña incorrecta";
                return BadRequest(respuesta);
            }
            respuesta.Exito = 1;
            respuesta.Data = usuarioResponse;

            return Ok(respuesta);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Registrar([FromBody] AuthRequest model)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    var existe = db.Usuarios.Where(x => x.Email == model.Email).FirstOrDefault();
                    if (existe != null) respuesta.Mensaje = "Ese email ya esta registrado";
                    else
                    {
                        db.Usuarios.Add(new Usuario()
                        {
                            Email = model.Email,
                            Contraseña = Encrypt.GetSHA256(model.Contraseña)
                        });
                        db.SaveChanges();
                        respuesta.Mensaje = "Registro exitoso";
                        respuesta.Exito = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }
    }
}
