using System.Collections.Generic;

namespace pruebaDisneyApi.Models.ViewModels
{
    public class PersonajeDetails
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Peso { get; set; }
        public string Historia { get; set; }
        public string UrlImg { get; set; }
        public List<PeliculaDetailsVM> PeliculasRelacionadas { get; set; }
    }
}
