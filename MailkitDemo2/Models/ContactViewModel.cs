using System.ComponentModel.DataAnnotations;

namespace MailkitDemo2.Models;

public class ContactViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Email no válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El mensaje es obligatorio")]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;
}