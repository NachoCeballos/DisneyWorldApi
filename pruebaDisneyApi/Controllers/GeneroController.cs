using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pruebaDisneyApi.Models;
using pruebaDisneyApi.Models.Response;
using System;
using System.Linq;

namespace pruebaDisneyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class GeneroController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using(DisneyContext db = new DisneyContext())
                {
                    respuesta.Data = db.Generos.ToList();
                    respuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Genero generoFV)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    db.Generos.Add(new Genero()
                    {
                        Nombre = generoFV.Nombre,
                        Peliculas = generoFV.Peliculas
                    });
                    db.SaveChanges();
                    respuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpPut]
        public IActionResult Edit([FromBody] Genero generoFV)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    Genero generoAux = db.Generos.Find(generoFV.Id);
                    generoAux.Nombre = generoFV.Nombre;
                    generoAux.Peliculas = generoFV.Peliculas;
                    db.Entry(generoAux).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    respuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    Genero generoAux = db.Generos.Find(Id);
                    db.Remove(generoAux);
                    db.SaveChanges();
                    respuesta.Exito = 1;    
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
