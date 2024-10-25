using CieloExtremo.Data.ApplicationDbContext;
using CieloExtremo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CieloExtremo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productosDestacados = await _context.Producto
                .Where(td => td.destacar == true)
                .ToListAsync();
            return View(productosDestacados);
        }
        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                // Si el t�rmino de b�squeda est� vac�o, podr�as redirigir a la p�gina principal o mostrar un mensaje de error.
                ViewData["Mensaje"] = "No se encontraron productos";
                return View("~/Views/Productos/NoProductos.cshtml");
            }

            // L�gica para buscar productos por el t�rmino de b�squeda
            var productosEncontrados = await _context.Producto
                .Where(p => p.nombre.StartsWith(searchTerm))
                .ToListAsync();

            // Pasar el t�rmino de b�squeda a la vista
            ViewData["SearchTerm"] = searchTerm;

            if (productosEncontrados.Count == 0)
            {
                ViewData["Mensaje"] = "No se encontraron productos";
                return View("~/Views/Productos/NoProductos.cshtml");
            }

            return View("~/Views/Productos/VistaBusqueda.cshtml", productosEncontrados);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
