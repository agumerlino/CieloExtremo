using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Threading.Tasks;
using CieloExtremo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CieloExtremo.Data.ApplicationDbContext;
using CieloExtremo.Models;
using Microsoft.AspNetCore.Authorization;

namespace CieloExtremo.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;

        public ProductosController(ApplicationDbContext context, IWebHostEnvironment env, IEmailService emailService)
        {
            _context = context;
            _env = env;
            _emailService = emailService;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Todos los productos";
            return View(await _context.Producto.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, telefonoVendedor, mailVendedor, nombre, descripcion, precio, categoria, subcategoria, destacar")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                string moneda = Request.Form["moneda"];
                producto.precio = $"{moneda} {producto.precio}";

                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var pathDestino = Path.Combine(_env.WebRootPath, "img\\Productos");

                    for (int i = 0; i < archivos.Count && i < 6; i++)
                    {
                        var archivoFoto = archivos[i];
                        if (archivoFoto.Length > 0)
                        {
                            var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                            // Procesar y comprimir la imagen antes de guardarla
                            using (var image = Image.Load(archivoFoto.OpenReadStream()))
                            {
                                // Redimensionar la imagen si es necesario, por ejemplo, a 1024px de ancho
                                image.Mutate(x => x.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = new Size(900, 0) // Solo cambia el ancho, la altura se ajusta automáticamente
                                }));

                                // Guardar la imagen comprimida en un formato JPEG con calidad de 75 (ajustable)
                                var opcionesJpeg = new JpegEncoder { Quality = 65 };

                                using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                                {
                                    image.Save(filestream, opcionesJpeg);
                                }
                            }

                            // Asignar la ruta de la imagen a la propiedad correspondiente
                            switch (i)
                            {
                                case 0:
                                    producto.foto1 = archivoDestino;
                                    break;
                                case 1:
                                    producto.foto2 = archivoDestino;
                                    break;
                                case 2:
                                    producto.foto3 = archivoDestino;
                                    break;
                                case 3:
                                    producto.foto4 = archivoDestino;
                                    break;
                                case 4:
                                    producto.foto5 = archivoDestino;
                                    break;
                                case 5:
                                    producto.foto6 = archivoDestino;
                                    break;
                            }
                        }
                    }
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }
        // GET: Productos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var producto = await _context.Producto.FindAsync(id);
            producto.precio = producto.precio.Replace("AR$ ","");
            producto.precio = producto.precio.Replace("U$D ", "");
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,telefonoVendedor,mailVendedor,nombre,descripcion,precio,categoria,subcategoria,destacar,foto1,foto2,foto3,foto4,foto5,foto6")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string fotoActual1 = Request.Form["foto1Current"];
                string fotoActual2 = Request.Form["foto2Current"];
                string fotoActual3 = Request.Form["foto3Current"];
                string fotoActual4 = Request.Form["foto4Current"];
                string fotoActual5 = Request.Form["foto5Current"];
                string fotoActual6 = Request.Form["foto6Current"];
                string moneda = Request.Form["moneda"];
                producto.precio = $"{moneda} {producto.precio}";

                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var pathDestino = Path.Combine(_env.WebRootPath, "img\\Productos");

                    for (int i = 0; i < archivos.Count && i < 6; i++)
                    {
                        var archivoFoto = archivos[i];
                        if (archivoFoto.Length > 0)
                        {
                            var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);
                            string numeroFoto = archivoFoto.Name.Replace("foto","");
                            // Eliminar la foto existente si hay una

                            switch (Int32.Parse(numeroFoto))
                            {
                                case 1:
                                    if (!string.IsNullOrEmpty(fotoActual1))
                                    {
                                        string fotoAnterior1 = Path.Combine(pathDestino, fotoActual1);
                                        if (System.IO.File.Exists(fotoAnterior1))
                                            System.IO.File.Delete(fotoAnterior1);
                                    }
                                    break;
                                case 2:
                                    if (!string.IsNullOrEmpty(fotoActual2))
                                    {
                                        string fotoAnterior2 = Path.Combine(pathDestino, fotoActual2);
                                        if (System.IO.File.Exists(fotoAnterior2))
                                            System.IO.File.Delete(fotoAnterior2);
                                    }
                                    break;
                                case 3:
                                    if (!string.IsNullOrEmpty(fotoActual3))
                                    {
                                        string fotoAnterior3 = Path.Combine(pathDestino, fotoActual3);
                                        if (System.IO.File.Exists(fotoAnterior3))
                                            System.IO.File.Delete(fotoAnterior3);
                                    }
                                    break;
                                case 4:
                                    if (!string.IsNullOrEmpty(fotoActual4))
                                    {
                                        string fotoAnterior4 = Path.Combine(pathDestino, fotoActual4);
                                        if (System.IO.File.Exists(fotoAnterior4))
                                            System.IO.File.Delete(fotoAnterior4);
                                    }
                                    break;
                                case 5:
                                    if (!string.IsNullOrEmpty(fotoActual5))
                                    {
                                        string fotoAnterior5 = Path.Combine(pathDestino, fotoActual5);
                                        if (System.IO.File.Exists(fotoAnterior5))
                                            System.IO.File.Delete(fotoAnterior5);
                                    }
                                    break;
                                case 6:
                                    if (!string.IsNullOrEmpty(fotoActual6))
                                    {
                                        string fotoAnterior6 = Path.Combine(pathDestino, fotoActual6);
                                        if (System.IO.File.Exists(fotoAnterior6))
                                            System.IO.File.Delete(fotoAnterior6);
                                    }
                                    break;
                            }                             

                            // Procesar y comprimir la imagen antes de guardarla
                            using (var image = Image.Load(archivoFoto.OpenReadStream()))
                            {
                                // Redimensionar la imagen si es necesario, por ejemplo, a 1024px de ancho
                                image.Mutate(x => x.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = new Size(900, 0) // Solo cambia el ancho, la altura se ajusta automáticamente
                                }));

                                // Guardar la imagen comprimida en un formato JPEG con calidad de 75 (ajustable)
                                var opcionesJpeg = new JpegEncoder { Quality = 65 };

                                using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                                {
                                    image.Save(filestream, opcionesJpeg);
                                }
                            }
                            
                            // Asignar la nueva ruta de la imagen a la propiedad correspondiente
                            switch (Int32.Parse(numeroFoto))
                            {
                                case 1:
                                    producto.foto1 = archivoDestino;
                                    break;
                                case 2:
                                    producto.foto2 = archivoDestino;
                                    break;
                                case 3:
                                    producto.foto3 = archivoDestino;
                                    break;
                                case 4:
                                    producto.foto4 = archivoDestino;
                                    break;
                                case 5:
                                    producto.foto5 = archivoDestino;
                                    break;
                                case 6:
                                    producto.foto6 = archivoDestino;
                                    break;
                            }
                        }
                    }
                }
                if (archivos.Count < 6)
                {
                    if (string.IsNullOrEmpty(producto.foto1))
                        producto.foto1 = fotoActual1;
                    if (string.IsNullOrEmpty(producto.foto2))
                        producto.foto2 = fotoActual2;
                    if (string.IsNullOrEmpty(producto.foto3))
                        producto.foto3 = fotoActual3;
                    if (string.IsNullOrEmpty(producto.foto4))
                        producto.foto4 = fotoActual4;
                    if (string.IsNullOrEmpty(producto.foto5))
                        producto.foto5 = fotoActual5;
                    if (string.IsNullOrEmpty(producto.foto6))
                        producto.foto6 = fotoActual6;
                }
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Producto
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto != null)
            {
                // Lista de propiedades de fotos
                var fotos = new List<string> { producto.foto1, producto.foto2, producto.foto3, producto.foto4, producto.foto5, producto.foto6 };

                // Eliminar archivos de fotos del sistema de archivos
                foreach (var foto in fotos)
                {
                    if (!string.IsNullOrEmpty(foto))
                    {
                        var filePath = Path.Combine(_env.WebRootPath, "img\\Productos", foto);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                // Eliminar el producto de la base de datos
                _context.Producto.Remove(producto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Cat(string categoria)
        {
            ViewData["Title"] = categoria;
            if (string.IsNullOrEmpty(categoria))
            {
                return NotFound();
            }

            // Filtrar productos por la categoría especificada
            var productos = await _context.Producto
                .Where(p => p.categoria == categoria)
                .ToListAsync();

            if (productos == null || productos.Count == 0)
            {
                ViewData["Mensaje"] = "No se encontraron productos";
                return View("NoProductos");
            }

            return View("Index", productos); // Mostrar la vista "Index" con los productos filtrados
        }
        public async Task<IActionResult> sCat(string subcategoria, string categoria)
        {
            ViewData["Title"] = categoria + " > " + subcategoria;
            if (string.IsNullOrEmpty(subcategoria) || string.IsNullOrEmpty(categoria))
            {
                return NotFound();
            }

            // Filtrar productos por la categoría y subcategoría especificadas
            var productos = await _context.Producto
                .Where(p => p.categoria == categoria && p.subcategoria == subcategoria)
                .ToListAsync();

            if (productos == null || productos.Count == 0)
            {
                ViewData["Mensaje"] = $"No se encontraron productos";
                return View("NoProductos");
            }

            return View("Index", productos); // Mostrar la vista "Index" con los productos filtrados
        }
        public async Task<IActionResult> QuieroVender()
        {
            return View("QuieroVender"); // Mostrar la vista "Index" con los productos filtrados
        }

        // GET: Productos/ContactSeller/5
        public async Task<IActionResult> ContactSeller(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto); // Retorna la vista con el producto, donde se incluirá el formulario de contacto
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToSeller(int id, string nombre, string email, string telefono, string mensaje)
        {
            var producto = await _context.Producto.FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null || string.IsNullOrEmpty(producto.mailVendedor))
            {
                return NotFound("Producto no encontrado o el vendedor no tiene un correo registrado.");
            }

            if (ModelState.IsValid)
            {
                // Configurar el asunto y el cuerpo del mensaje
                var subject = $"Nuevo mensaje sobre {producto.nombre} de {nombre}";
                var body = $"Nombre: {nombre}<br>" +
                           $"Email: {email}<br>" +
                           $"Teléfono: {telefono}<br>" +
                           $"Mensaje: {mensaje}<br>" +
                           $"Producto: {producto.nombre}<br>" +
                           $"PRECIO PUBLICADO: {producto.precio}";

                // Enviar correo al vendedor
                await _emailService.SendEmailToSellerAsync(producto.mailVendedor, subject, body);

                // Redirige a una vista de éxito o muestra un mensaje de confirmación
                return RedirectToAction("Success");
            }

            return View("ContactSeller", producto); // Retorna a la vista original con errores de validación
        }
        public IActionResult Success()
        {
            return View(); // Asegúrate de que haya una vista Success.cshtml en la carpeta correcta
        }

    }
}
