using System;

namespace pruebaDisneyApi.Models.ViewModels
{
    public class PeliculaDetailsVM
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Clasificacion { get; set; }
        public string Genero { get; set; }
        public string UrlImg { get; set; }
    }
}
