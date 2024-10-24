using Microsoft.AspNetCore.Mvc;
using CieloExtremo.Models;
using CieloExtremo.Services;

namespace CieloExtremo.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Send(FormModel model)
        {
            if (ModelState.IsValid)
            {
                var subject = $"Nuevo mensaje de {model.NombreApellido}";
                var body = $"Nombre y Apellido: {model.NombreApellido}<br>" +
                           $"Email: {model.Email}<br>" +
                           $"Teléfono: {model.Telefono}<br>" +
                           $"Cómo nos conociste: {model.Conociste}<br>" +
                           $"Mensaje: {model.Mensaje}";

                await _emailService.SendEmailAsync(subject, body);
                return RedirectToAction("Success"); // Redirige a una vista de éxito
            }

            return View(model); // Retorna a la vista original con errores de validación
        }       

        public IActionResult Success()
        {
            return View(); // Asegúrate de tener una vista "Success.cshtml"
        }
    }
}