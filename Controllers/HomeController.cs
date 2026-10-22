using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CampusEvent.Models;

namespace CampusEvent.Controllers;

public class HomeController : Controller
{
    // In-memory stores shared across requests via static lists
    public static List<Event> EventList { get; } = new()
    {
        new Event { Id = 1, EventName = "Tech Fest 2025", EventDate = new DateTime(2025, 11, 10), Venue = "Main Auditorium", Budget = 50000, Status = "Planned" },
        new Event { Id = 2, EventName = "Cultural Night", EventDate = new DateTime(2025, 12, 5), Venue = "Open Air Stage", Budget = 30000, Status = "Confirmed" }
    };

    public static List<Volunteer> VolunteerList { get; } = new()
    {
        new Volunteer { Id = 1, FullName = "Alice Johnson", Email = "alice@campus.edu", Role = "Coordinator", AssignedEvent = "Tech Fest 2025" },
        new Volunteer { Id = 2, FullName = "Bob Smith", Email = "bob@campus.edu", Role = "Logistics", AssignedEvent = "Cultural Night" }
    };

    public static List<BudgetRecord> BudgetRecords { get; } = new()
    {
        new BudgetRecord { Id = 1, Description = "Stage Setup", Amount = 15000, Category = "Infrastructure", ExpenseDate = new DateTime(2025, 10, 1) },
        new BudgetRecord { Id = 2, Description = "Catering", Amount = 8000, Category = "Food", ExpenseDate = new DateTime(2025, 10, 5) }
    };

    public IActionResult Dashboard()
    {
        var msg = HttpContext.Session.GetString("Message");
        if (msg != null)
        {
            ViewBag.Message = msg;
            HttpContext.Session.Remove("Message");
        }

        ViewBag.EventCount = EventList.Count;
        ViewBag.VolunteerCount = VolunteerList.Count;
        ViewBag.TotalBudget = EventList.Sum(e => e.Budget);
        ViewBag.TotalExpenses = BudgetRecords.Sum(b => b.Amount);
        ViewBag.RecentEvents = EventList.OrderByDescending(e => e.EventDate).Take(3).ToList();
        return View();
    }

    // ── Events ──────────────────────────────────────────────
    public IActionResult Events() => View(EventList);

    public IActionResult CreateEvent() => View(new Event());

    [HttpPost]
    public IActionResult CreateEvent(Event model)
    {
        if (!ModelState.IsValid) return View(model);
        model.Id = EventList.Count > 0 ? EventList.Max(e => e.Id) + 1 : 1;
        EventList.Add(model);
        HttpContext.Session.SetString("Message", $"Event '{model.EventName}' created successfully.");
        return RedirectToAction("Events");
    }

    public IActionResult EditEvent(int id)
    {
        var ev = EventList.FirstOrDefault(e => e.Id == id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    [HttpPost]
    public IActionResult EditEvent(Event model)
    {
        if (!ModelState.IsValid) return View(model);
        var ev = EventList.FirstOrDefault(e => e.Id == model.Id);
        if (ev == null) return NotFound();
        ev.EventName = model.EventName; ev.EventDate = model.EventDate;
        ev.Venue = model.Venue; ev.Budget = model.Budget; ev.Status = model.Status;
        HttpContext.Session.SetString("Message", $"Event '{model.EventName}' updated.");
        return RedirectToAction("Events");
    }

    [HttpPost]
    public IActionResult DeleteEvent(int id)
    {
        var ev = EventList.FirstOrDefault(e => e.Id == id);
        if (ev != null) { EventList.Remove(ev); HttpContext.Session.SetString("Message", "Event deleted."); }
        return RedirectToAction("Events");
    }

    // ── Volunteers ───────────────────────────────────────────
    public IActionResult Volunteers() => View(VolunteerList);

    public IActionResult CreateVolunteer() => View(new Volunteer());

    [HttpPost]
    public IActionResult CreateVolunteer(Volunteer model)
    {
        if (!ModelState.IsValid) return View(model);
        model.Id = VolunteerList.Count > 0 ? VolunteerList.Max(v => v.Id) + 1 : 1;
        VolunteerList.Add(model);
        HttpContext.Session.SetString("Message", $"Volunteer '{model.FullName}' added.");
        return RedirectToAction("Volunteers");
    }

    public IActionResult EditVolunteer(int id)
    {
        var vol = VolunteerList.FirstOrDefault(v => v.Id == id);
        if (vol == null) return NotFound();
        return View(vol);
    }

    [HttpPost]
    public IActionResult EditVolunteer(Volunteer model)
    {
        if (!ModelState.IsValid) return View(model);
        var vol = VolunteerList.FirstOrDefault(v => v.Id == model.Id);
        if (vol == null) return NotFound();
        vol.FullName = model.FullName; vol.Email = model.Email;
        vol.Role = model.Role; vol.AssignedEvent = model.AssignedEvent;
        HttpContext.Session.SetString("Message", $"Volunteer '{model.FullName}' updated.");
        return RedirectToAction("Volunteers");
    }

    [HttpPost]
    public IActionResult DeleteVolunteer(int id)
    {
        var vol = VolunteerList.FirstOrDefault(v => v.Id == id);
        if (vol != null) { VolunteerList.Remove(vol); HttpContext.Session.SetString("Message", "Volunteer removed."); }
        return RedirectToAction("Volunteers");
    }

    // ── Budget ───────────────────────────────────────────────
    public IActionResult Budget()
    {
        ViewBag.Total = BudgetRecords.Sum(b => b.Amount);
        return View(BudgetRecords);
    }

    public IActionResult CreateBudget() => View(new BudgetRecord());

    [HttpPost]
    public IActionResult CreateBudget(BudgetRecord model)
    {
        if (!ModelState.IsValid) return View(model);
        model.Id = BudgetRecords.Count > 0 ? BudgetRecords.Max(b => b.Id) + 1 : 1;
        BudgetRecords.Add(model);
        HttpContext.Session.SetString("Message", "Budget record added.");
        return RedirectToAction("Budget");
    }

    public IActionResult EditBudget(int id)
    {
        var rec = BudgetRecords.FirstOrDefault(b => b.Id == id);
        if (rec == null) return NotFound();
        return View(rec);
    }

    [HttpPost]
    public IActionResult EditBudget(BudgetRecord model)
    {
        if (!ModelState.IsValid) return View(model);
        var rec = BudgetRecords.FirstOrDefault(b => b.Id == model.Id);
        if (rec == null) return NotFound();
        rec.Description = model.Description; rec.Amount = model.Amount;
        rec.Category = model.Category; rec.ExpenseDate = model.ExpenseDate;
        HttpContext.Session.SetString("Message", "Budget record updated.");
        return RedirectToAction("Budget");
    }

    [HttpPost]
    public IActionResult DeleteBudget(int id)
    {
        var rec = BudgetRecords.FirstOrDefault(b => b.Id == id);
        if (rec != null) { BudgetRecords.Remove(rec); HttpContext.Session.SetString("Message", "Budget record deleted."); }
        return RedirectToAction("Budget");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
