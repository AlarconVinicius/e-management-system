using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMS.WebApp.MVC.Business.Models.Users;
using EMS.WebApp.MVC.Data;

namespace EMS.WebApp.MVC.Controllers;

public class SubscribersController : Controller
{
    private readonly EMSDbContext _context;

    public SubscribersController(EMSDbContext context)
    {
        _context = context;
    }

    // GET: Subscribers
    public async Task<IActionResult> Index()
    {
          return View(await _context.Subscribers.ToListAsync());
    }

    // GET: Subscribers/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Subscribers == null)
        {
            return NotFound();
        }

        var subscriber = await _context.Subscribers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (subscriber == null)
        {
            return NotFound();
        }

        return View(subscriber);
    }

    // GET: Subscribers/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Subscribers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Deleted,Id")] Subscriber subscriber)
    {
        if (ModelState.IsValid)
        {
            subscriber.Id = Guid.NewGuid();
            _context.Add(subscriber);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(subscriber);
    }

    // GET: Subscribers/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Subscribers == null)
        {
            return NotFound();
        }

        var subscriber = await _context.Subscribers.FindAsync(id);
        if (subscriber == null)
        {
            return NotFound();
        }
        return View(subscriber);
    }

    // POST: Subscribers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Name,Deleted,Id")] Subscriber subscriber)
    {
        if (id != subscriber.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(subscriber);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriberExists(subscriber.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(subscriber);
    }

    // GET: Subscribers/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Subscribers == null)
        {
            return NotFound();
        }

        var subscriber = await _context.Subscribers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (subscriber == null)
        {
            return NotFound();
        }

        return View(subscriber);
    }

    // POST: Subscribers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Subscribers == null)
        {
            return Problem("Entity set 'EMSDbContext.Subscribers'  is null.");
        }
        var subscriber = await _context.Subscribers.FindAsync(id);
        if (subscriber != null)
        {
            _context.Subscribers.Remove(subscriber);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SubscriberExists(Guid id)
    {
      return _context.Subscribers.Any(e => e.Id == id);
    }
}
