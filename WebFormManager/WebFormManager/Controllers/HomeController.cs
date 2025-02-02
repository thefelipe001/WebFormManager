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

        public async Task<IActionResult> Data(string pelicula, string genero, string plataformasSeleccionadas, string actor, string comentario, string datetime)
        {
            try
            {
                FormularioPeliculas peliculaData = new FormularioPeliculas();
                peliculaData.PeliculaFavorita = pelicula;
                peliculaData.GenerosFavoritos = genero; // Se asume que genero ya es una cadena separada por comas
                peliculaData.PlataformasStreaming = plataformasSeleccionadas; // Recibe los valores de checkboxes separados por comas
                peliculaData.ActorFavorito = actor;
                peliculaData.EscenaFavorita = comentario;
                peliculaData.FechaHoraUltimaPelicula = !string.IsNullOrEmpty(datetime) ? DateTime.Parse(datetime) : (DateTime?)null;

                // Opcional: Procesar plataformas seleccionadas como lista
                if (!string.IsNullOrEmpty(plataformasSeleccionadas))
                {
                    var plataformasLista = plataformasSeleccionadas.Split(',').ToList(); // Convierte en una lista de strings si necesitas trabajar con ellas
                }

                await _applicationDBContext.FormularioPeliculas.AddAsync(peliculaData);
                await _applicationDBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Aquí puedes registrar el error si tienes un sistema de logs
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
