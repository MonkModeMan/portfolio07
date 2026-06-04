using InquiryDesk.Models;
using InquiryDesk.Services;
using Microsoft.AspNetCore.Mvc;

namespace InquiryDesk.Controllers;

public sealed class TicketsController(IInquiryRepository repository) : Controller
{
    public IActionResult Index([FromQuery] TicketFilter filter)
    {
        var viewModel = new TicketDashboardViewModel
        {
            Tickets = repository.Search(filter),
            Agents = repository.GetAgents(),
            Filter = filter,
            StatusCounts = repository.GetStatusCounts()
        };

        return View(viewModel);
    }

    public IActionResult Details(int id)
    {
        var ticket = repository.Find(id);
        if (ticket is null)
        {
            return NotFound();
        }

        return View(new TicketFormViewModel
        {
            Ticket = ticket,
            Agents = repository.GetAgents()
        });
    }

    public IActionResult Create()
    {
        return View("Form", new TicketFormViewModel
        {
            Ticket = new InquiryTicket(),
            Agents = repository.GetAgents()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind(Prefix = "Ticket")] InquiryTicket ticket)
    {
        if (!ModelState.IsValid)
        {
            return View("Form", new TicketFormViewModel
            {
                Ticket = ticket,
                Agents = repository.GetAgents()
            });
        }

        var created = repository.Add(ticket);
        TempData["Message"] = "問い合わせチケットを登録しました。";
        return RedirectToAction(nameof(Details), new { id = created.Id });
    }

    public IActionResult Edit(int id)
    {
        var ticket = repository.Find(id);
        if (ticket is null)
        {
            return NotFound();
        }

        return View("Form", new TicketFormViewModel
        {
            Ticket = ticket,
            Agents = repository.GetAgents()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind(Prefix = "Ticket")] InquiryTicket ticket)
    {
        if (id != ticket.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View("Form", new TicketFormViewModel
            {
                Ticket = ticket,
                Agents = repository.GetAgents()
            });
        }

        if (!repository.Update(ticket))
        {
            return NotFound();
        }

        TempData["Message"] = "問い合わせチケットを更新しました。";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        if (!repository.Delete(id))
        {
            return NotFound();
        }

        TempData["Message"] = "問い合わせチケットを削除しました。";
        return RedirectToAction(nameof(Index));
    }
}
