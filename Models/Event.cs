using System.ComponentModel.DataAnnotations;

namespace CampusEvent.Models;

public class Event
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string EventName { get; set; } = "";

    [Required]
    public DateTime EventDate { get; set; } = DateTime.Today;

    [Required, StringLength(100)]
    public string Venue { get; set; } = "";

    [Range(0, 10000000)]
    public decimal Budget { get; set; }

    [StringLength(30)]
    public string Status { get; set; } = "Planned";
}
