using Microsoft.AspNetCore.Authorization;
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
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta respuesta = new Respuesta();

            using (DisneyContext db = new DisneyContext())
            {
                var lstPeliculas = db.Peliculas.ToList();
                List<PeliculaVM> peliculaVMs = new List<PeliculaVM>();
                
                for(int i = 0; i < lstPeliculas.Count; i++)
                {
                    peliculaVMs.Add(new PeliculaVM()
                    {
                        Titulo = lstPeliculas[i].Titulo,
                        FechaCreacion = lstPeliculas[i].FechaCreacion,
                        UrlImg = lstPeliculas[i].UrlImg
                    });
                }
                respuesta.Data = peliculaVMs;
                respuesta.Exito = 1;
            }
            return Ok(respuesta);
        }

        #region CRUD
        [HttpPost]
        public IActionResult Add([FromBody] Pelicula peliculaFV)
        {
            Respuesta respuesta = new Respuesta();
            
            if(peliculaFV.Clasificacion > 5) return Ok(respuesta.Mensaje = "La clasificación debe encontrarse entre 1 y 5");
            
            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    db.Peliculas.Add(new Pelicula()
                    {
                        Titulo = peliculaFV.Titulo,
                        FechaCreacion = peliculaFV.FechaCreacion,
                        Clasificacion = peliculaFV.Clasificacion,
                        GeneroId = peliculaFV.GeneroId,
                        Genero = db.Generos.Find(peliculaFV.GeneroId),
                        UrlImg = peliculaFV.UrlImg,
                        PeliculaPersonajes = peliculaFV.PeliculaPersonajes,
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
        public IActionResult Edit([FromBody] Pelicula peliculaFV)
        {
            Respuesta respuesta = new Respuesta();

            if (peliculaFV.Clasificacion > 5) return Ok(respuesta.Mensaje = "La clasificación debe encontrarse entre 1 y 5");

            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    Pelicula peliculaAux = db.Peliculas.Find(peliculaFV.Id);
                    peliculaAux.Titulo = peliculaFV.Titulo;
                    peliculaAux.FechaCreacion = peliculaFV.FechaCreacion;
                    peliculaAux.Clasificacion = peliculaFV.Clasificacion;
                    peliculaAux.PeliculaPersonajes = peliculaFV.PeliculaPersonajes;
                    peliculaAux.GeneroId = peliculaFV.GeneroId;
                    peliculaAux.UrlImg = peliculaFV.UrlImg;
                    db.Entry(peliculaAux).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                    Pelicula peliculaAux = db.Peliculas.Find(Id);
                    db.Remove(peliculaAux);
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
        [Route("genre={IdGenero}")]
        public IActionResult GetByGenero(int IdGenero)
        {
            return Ok(GetBy(IdGenero.ToString()));
        }

        [HttpGet]
        [Route("order=ASC")]
        public IActionResult GetByASC()
        {
            return Ok(GetByOrder("ASC"));
        }

        [HttpGet]
        [Route("order=DESC")]
        public IActionResult GetByDESC()
        {
            return Ok(GetByOrder("DESC"));
        }

        public Respuesta GetByOrder(string order)
        {
            Respuesta respuesta = new Respuesta();
            
            using (DisneyContext db = new DisneyContext())
            {
                List<PeliculaDetails> ListPeliculaDetails = new List<PeliculaDetails>();
                Pelicula peliculaAux;
                var ListaPeliculas = db.Peliculas.ToList();

                foreach (var peliculas in ListaPeliculas)
                {
                    List<PersonajeDetailsVM> LstPersonajeDetailsVM = new List<PersonajeDetailsVM>();

                    var IdPeli = db.PeliculaPersonaje.ToList();
                    peliculaAux = db.Peliculas.Find(peliculas.Id);

                    foreach (var peliculaPersonaje in IdPeli)
                    {
                        if (peliculaAux.Id == peliculaPersonaje.PeliculaId)
                        {
                            var i = peliculaPersonaje.PersonajeId;

                            LstPersonajeDetailsVM.Add(new PersonajeDetailsVM()
                            {
                                Id = db.Personajes.Find(i).Id,
                                Nombre = db.Personajes.Find(i).Nombre,
                                Edad = db.Personajes.Find(i).Edad,
                                Peso = db.Personajes.Find(i).Peso,
                                Historia = db.Personajes.Find(i).Historia,
                                UrlImg = db.Personajes.Find(i).UrlImg
                            });
                        }
                    }

                    ListPeliculaDetails.Add(new PeliculaDetails()
                    {
                        Id = peliculas.Id,
                        Titulo = peliculas.Titulo,
                        FechaCreacion = peliculas.FechaCreacion,
                        Clasificacion = peliculas.Clasificacion,
                        UrlImg = peliculas.UrlImg,
                        Genero = db.Generos.Find(peliculaAux.GeneroId).Nombre,
                        PersonajesRelacionados = LstPersonajeDetailsVM
                    });
                }

                if(order == "DESC") respuesta.Data = ListPeliculaDetails.OrderByDescending(x => x.FechaCreacion);
                if(order == "ASC") respuesta.Data = ListPeliculaDetails.OrderBy(x => x.FechaCreacion);
                respuesta.Exito = 1;
            }
            return respuesta;
        }

        public Respuesta GetBy(string obj)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (DisneyContext db = new DisneyContext())
                {
                    PeliculaDetails peliculaDetails = new PeliculaDetails();
                    peliculaDetails.PersonajesRelacionados = new List<PersonajeDetailsVM>();
                    Pelicula peliculaAux;
                    if (db.Peliculas.Where(x => x.Titulo == obj).FirstOrDefault() != null)
                        peliculaAux = db.Peliculas.Where(x => x.Titulo == obj).FirstOrDefault();
                    else if (db.Peliculas.Where(x => x.GeneroId.ToString() == obj).FirstOrDefault() != null)
                        peliculaAux = db.Peliculas.Where(x => x.GeneroId.ToString() == obj).FirstOrDefault();
                    else
                    {
                        respuesta.Mensaje = "No se encontro ninguna película";
                        return respuesta;
                    }

                    peliculaDetails.Id = peliculaAux.Id;
                    peliculaDetails.Titulo = peliculaAux.Titulo;
                    peliculaDetails.FechaCreacion = peliculaAux.FechaCreacion;
                    peliculaDetails.Clasificacion = peliculaAux.Clasificacion;
                    peliculaDetails.Genero = db.Generos.Find(peliculaAux.GeneroId).Nombre;
                    peliculaDetails.UrlImg = peliculaAux.UrlImg;

                    var IdPeli = db.PeliculaPersonaje.ToList();

                    var personajeDetailsVM = new PersonajeDetailsVM();

                    foreach (var peliculaPersonaje in IdPeli)
                    {
                        if (peliculaAux.Id == peliculaPersonaje.PeliculaId)
                        {
                            var i = peliculaPersonaje.PersonajeId;

                            peliculaDetails.PersonajesRelacionados.Add(new PersonajeDetailsVM()
                            {
                                Id = db.Personajes.Find(i).Id,
                                Nombre = db.Personajes.Find(i).Nombre,
                                Edad = db.Personajes.Find(i).Edad,
                                Peso = db.Personajes.Find(i).Peso,
                                Historia = db.Personajes.Find(i).Historia,
                                UrlImg = db.Personajes.Find(i).UrlImg
                            });
                        }
                    }
                    respuesta.Data = peliculaDetails;
                    respuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
