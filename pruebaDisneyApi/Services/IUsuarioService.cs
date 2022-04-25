using pruebaDisneyApi.Models.Request;
using pruebaDisneyApi.Models.Response;

namespace pruebaDisneyApi.Services
{
    public interface IUsuarioService
    {
        UsuarioResponse Auth(AuthRequest model);
    }
}
