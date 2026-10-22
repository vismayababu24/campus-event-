using System.ComponentModel.DataAnnotations;

namespace CampusEvent.Models;

public class Volunteer
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string FullName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [StringLength(50)]
    public string Role { get; set; } = "";

    [StringLength(100)]
    public string AssignedEvent { get; set; } = "";
}
