using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pruebaDisneyApi.Models;
using pruebaDisneyApi.Models.Response;
using pruebaDisneyApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pruebaDisneyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CharactersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    var lstPersonajes = db.Personajes.ToList();
                    List<PersonajeVM> personajeVMs = new List<PersonajeVM>();

                    for (int i = 0; i <lstPersonajes.Count; i++)
                    {
                        personajeVMs.Add(new PersonajeVM()
                        {
                            Nombre = lstPersonajes[i].Nombre,
                            UrlImg = lstPersonajes[i].UrlImg
                        });    
                    }
                    respuesta.Data = personajeVMs;
                    respuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        #region CRUD
        [HttpPost]
        public IActionResult Add(Personaje personajeFV)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    db.Personajes.Add(new Personaje()
                    {
                        Nombre = personajeFV.Nombre,
                        Edad = personajeFV.Edad,
                        Peso = personajeFV.Peso,
                        Historia = personajeFV.Historia,
                        PeliculaPersonajes = personajeFV.PeliculaPersonajes,
                        UrlImg = personajeFV.UrlImg
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
        public IActionResult Edit(Personaje personajeFV)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    Personaje personajeAux = db.Personajes.Find(personajeFV.Id);
                    personajeAux.Nombre = personajeFV.Nombre;
                    personajeAux.Edad = personajeFV.Edad;
                    personajeAux.Peso = personajeFV.Peso;
                    personajeAux.Historia = personajeFV.Historia;
                    personajeAux.PeliculaPersonajes = personajeFV.PeliculaPersonajes;
                    personajeAux.UrlImg = personajeFV.UrlImg;
                    db.Entry(personajeAux).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                    Personaje personajeAux = db.Personajes.Find(Id);
                    db.Remove(personajeAux);
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
        #endregion

        [HttpGet]
        [Route("name={Nombre}")]
        public IActionResult GetByNombre(string Nombre)
        {
            return Ok(GetBy(Nombre));
        }

        [HttpGet]
        [Route("edad={Edad}")]
        public IActionResult GetByEdad(int Edad)
        {
            return Ok(GetBy(Edad.ToString()));
        }

        [HttpGet]
        [Route("peso={Peso}")]
        public IActionResult GetByPeso(int Peso)
        {
            return Ok(GetBy(Peso.ToString()));
        }

        [HttpGet]
        [Route("movies={IdMovie}")]
        public IActionResult GetByIdMovie(int IdMovie)
        {
            return Ok(GetBy(IdMovie.ToString()));
        }

        public Respuesta GetBy(string obj)
        {
            Respuesta respuesta = new Respuesta();

            using (DisneyContext db = new DisneyContext())
            {
                List<PersonajeDetails> ListPersonajeDetails = new List<PersonajeDetails>();
                Personaje personajeAux;
                List<Personaje> ListPersonajeAux = new List<Personaje>();
                var ListaPersonajes = db.Personajes.ToList();

                if (db.Personajes.Where(x => x.Nombre == obj).FirstOrDefault() != null)
                    ListPersonajeAux = db.Personajes.Where(x => x.Nombre == obj).ToList();
                else if (db.Personajes.Where(x => x.Edad.ToString() == obj).FirstOrDefault() != null)
                    ListPersonajeAux = db.Personajes.Where(x => x.Edad.ToString() == obj).ToList();
                else if (db.Personajes.Where(x => x.Peso.ToString() == obj).FirstOrDefault() != null)
                    ListPersonajeAux = db.Personajes.Where(x => x.Peso.ToString() == obj).ToList();

                else if (db.PeliculaPersonaje.Where(x=>x.PeliculaId.ToString() == obj).FirstOrDefault() != null)
                {
                    var pelicula = db.Peliculas.Where(x => x.Id.ToString() == obj).FirstOrDefault();
                    var PeliculasPersonajes = db.PeliculaPersonaje.Where(x => x.PeliculaId == pelicula.Id).ToList();

                    foreach (var item in PeliculasPersonajes)
                    {
                        personajeAux = db.Personajes.Find(item.PersonajeId);
                        ListPersonajeAux.Add(personajeAux);
                    }

                }

                foreach (var personaje in ListPersonajeAux)
                {
                    List<PeliculaDetailsVM> LstPeliculaDetailsVM = new List<PeliculaDetailsVM>();

                    var IdPeli = db.PeliculaPersonaje.ToList();
                    personajeAux = db.Personajes.Find(personaje.Id);

                    foreach (var peliculaPersonaje in IdPeli)
                    {
                        if (personajeAux.Id == peliculaPersonaje.PersonajeId)
                        {
                            var i = peliculaPersonaje.PeliculaId;

                            LstPeliculaDetailsVM.Add(new PeliculaDetailsVM()
                            {
                                Id = db.Peliculas.Find(i).Id,
                                Titulo = db.Peliculas.Find(i).Titulo,
                                FechaCreacion = db.Peliculas.Find(i).FechaCreacion,
                                Clasificacion = db.Peliculas.Find(i).Clasificacion,
                                Genero = db.Generos.Find(db.Peliculas.Find(i).GeneroId).Nombre,
                                UrlImg = db.Peliculas.Find(i).UrlImg
                            });
                        }
                    }

                    ListPersonajeDetails.Add(new PersonajeDetails()
                    {
                        Id = personaje.Id,
                        Nombre = personaje.Nombre,
                        Edad = personaje.Edad,
                        Peso = personaje.Peso,
                        Historia = personaje.Historia,
                        UrlImg = personaje.UrlImg,
                        PeliculasRelacionadas = LstPeliculaDetailsVM
                    });
                }
                respuesta.Data = ListPersonajeDetails;
                respuesta.Exito = 1;
            }
            return respuesta;
        }
    }
}
