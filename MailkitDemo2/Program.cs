using MailkitDemo2.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("===== DEBUG CONFIG =====");
Console.WriteLine("SMTP: " + builder.Configuration["EmailSettings:SmtpServer"]);
Console.WriteLine("PORT: " + builder.Configuration["EmailSettings:Port"]);
Console.WriteLine("========================");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ContactController>();

// Configurar EmailSettings desde appsettings.json
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Registrar EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

public class ContactController : Controller
{
    private readonly IEmailService _emailService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IEmailService emailService, ILogger<ContactController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    // Your action methods here
}
