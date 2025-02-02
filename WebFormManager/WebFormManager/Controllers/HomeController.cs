using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebFormManager.Models;

namespace WebFormManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDBContext;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public IActionResult Index()
        {
            return View();
        }

     public async Task<IActionResult> Data(
    string pelicula, 
    string genero, 
    string plataformasSeleccionadas, 
    string actor, 
    string comentario, 
    string datetime)
{
    try
    {
        // Validaci�n en el backend para asegurarse de que los datos requeridos est�n presentes
        if (string.IsNullOrWhiteSpace(pelicula) || 
            string.IsNullOrWhiteSpace(genero) || 
            string.IsNullOrWhiteSpace(plataformasSeleccionadas) || 
            string.IsNullOrWhiteSpace(actor) || 
            string.IsNullOrWhiteSpace(comentario) || 
            string.IsNullOrWhiteSpace(datetime))
        {
            TempData["Error"] = "Todos los campos son obligatorios.";
            return RedirectToAction("Formulario"); // Redirige al formulario si falta informaci�n
        }

        FormularioPeliculas peliculaData = new FormularioPeliculas
        {
            PeliculaFavorita = pelicula,
            GenerosFavoritos = genero, // Se asume que genero ya es una cadena separada por comas
            PlataformasStreaming = plataformasSeleccionadas, // Recibe los valores de checkboxes separados por comas
            ActorFavorito = actor,
            EscenaFavorita = comentario,
            FechaHoraUltimaPelicula = DateTime.TryParse(datetime, out DateTime fecha) ? fecha : (DateTime?)null
        };

        await _applicationDBContext.FormularioPeliculas.AddAsync(peliculaData);
        await _applicationDBContext.SaveChangesAsync();

        TempData["Success"] = "Pel�cula guardada correctamente.";
    }
    catch (Exception ex)
    {
        TempData["Error"] = "Ocurri� un error al guardar la pel�cula.";
        // Aqu� puedes registrar el error en logs si es necesario
    }

    return RedirectToAction("Inicio", "Home");
}











        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var pelicula = await _applicationDBContext.FormularioPeliculas.FindAsync(id);
                if (pelicula != null)
                {
                    _applicationDBContext.FormularioPeliculas.Remove(pelicula);
                    await _applicationDBContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Aqu� puedes registrar el error si tienes un sistema de logs
                throw;
            }

            return RedirectToAction("Inicio", "Home");
        }



        // GET: Clients
        public async Task<IActionResult> Inicio()
        {
            return View(await _applicationDBContext.FormularioPeliculas.ToListAsync());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
