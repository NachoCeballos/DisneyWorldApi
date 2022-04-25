using System.ComponentModel.DataAnnotations;

namespace pruebaDisneyApi.Models.Request
{
    public class AuthRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Contraseña { get; set; }
    }
}
