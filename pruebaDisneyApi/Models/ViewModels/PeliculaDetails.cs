using System;
using System.Collections.Generic;

namespace pruebaDisneyApi.Models.ViewModels
{
    public class PeliculaDetails
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Clasificacion { get; set; }
        public string Genero { get; set; }
        public string UrlImg { get; set; }
        public List<PersonajeDetailsVM> PersonajesRelacionados { get; set; }
    }
}
