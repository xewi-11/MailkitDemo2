using MailkitDemo2.Models;
using MailkitDemo2.Services; // ← Cambiar de MailkitDemo.Interfaces a MailkitDemo.Services
using Microsoft.AspNetCore.Mvc;

namespace MailkitDemo2.Controllers;

public class ContactController : Controller
{
    private readonly IEmailService _emailService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IEmailService emailService, ILogger<ContactController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var htmlBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; }}
                        .container {{ padding: 20px; background-color: #f5f5f5; }}
                        .message-box {{ background-color: white; padding: 20px; border-radius: 5px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='message-box'>
                            <h2>Nuevo mensaje de contacto</h2>
                            <p><strong>Nombre:</strong> {model.Name}</p>
                            <p><strong>Email:</strong> {model.Email}</p>
                            <p><strong>Mensaje:</strong></p>
                            <p>{model.Message}</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await _emailService.SendEmailAsync(
                "javi11coimbra@gmail.com",
                $"Contacto desde la web: {model.Name}",
                htmlBody,
                isHtml: true
            );

            TempData["Message"] = "✅ ¡Correo enviado exitosamente! Revisa tu bandeja de entrada.";
            TempData["IsSuccess"] = true;

            _logger.LogInformation("Email de contacto enviado desde {Email}", model.Email);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Message = $"❌ Error al enviar el correo: {ex.Message} ";
            ViewBag.IsSuccess = false;


            _logger.LogError(ex, "Error al enviar email de contacto desde {Email}", model.Email);

            return View(model);
        }
    }
}