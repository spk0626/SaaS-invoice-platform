using System.ComponentModel.DataAnnotations;

namespace Invoice.Application.Common.Settings;

public sealed class EmailSettings
{
    public const string SectionName = "Email"; 

    [Required]
    public string SmtpHost { get; init; } = string.Empty; // The hostname or IP address of the SMTP server used to send emails.
    
    public int SmtpPort { get; init; } = 587; // The port number used to connect to the SMTP server, typically 587 for secure connections.

    [Required]
    public string Username { get; init; } = string.Empty; // The username for authenticating with the SMTP server.

    [Required]
    public string Password { get; init; } = string.Empty; // The password for authenticating with the SMTP server.
    
    [Required]
    public string FromAddress { get; init; } = string.Empty; // The email address that will appear as the sender of the emails.
    
    public string FromName { get; set; } = "Invoice System"; // The name that will appear as the sender of the emails.
}