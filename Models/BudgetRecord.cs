using System.ComponentModel.DataAnnotations;

namespace CampusEvent.Models;

public class BudgetRecord
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Description { get; set; } = "";

    [Range(0.01, 10000000)]
    public decimal Amount { get; set; }

    [Required, StringLength(30)]
    public string Category { get; set; } = "";

    public DateTime ExpenseDate { get; set; } = DateTime.Today;
}
