using EMS.WebApp.Business.Models;
using EMS.WebApp.Data.Context;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Controllers;

public class CompaniesController : Controller
{
    private readonly EMSDbContext _context;

    public CompaniesController(EMSDbContext context)
    {
        _context = context;
    }

    // GET: Companies
    public async Task<IActionResult> Index()
    {
        var eMSDbContext = _context.Companies.Include(c => c.Plan);
        return View(await eMSDbContext.ToListAsync());
    }

    // GET: Companies/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Companies == null)
        {
            return NotFound();
        }

        var company = await _context.Companies
            .Include(c => c.Plan)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    // GET: Companies/Create
    public IActionResult Create()
    {
        ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Benefits");
        return View();
    }

    // POST: Companies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PlanId,Name,IsActive,Id,CreatedAt,UpdatedAt")] Company company)
    {
        if (ModelState.IsValid)
        {
            _context.Add(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Benefits", company.PlanId);
        return View(company);
    }

    // GET: Companies/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Companies == null)
        {
            return NotFound();
        }

        var company = await _context.Companies.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }
        ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Benefits", company.PlanId);
        return View(company);
    }

    // POST: Companies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("PlanId,Name,IsActive,Id,CreatedAt,UpdatedAt")] Company company)
    {
        if (id != company.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(company);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(company.Id))
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
        ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Benefits", company.PlanId);
        return View(company);
    }

    // GET: Companies/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Companies == null)
        {
            return NotFound();
        }

        var company = await _context.Companies
            .Include(c => c.Plan)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    // POST: Companies/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Companies == null)
        {
            return Problem("Entity set 'EMSDbContext.Companies'  is null.");
        }
        var company = await _context.Companies.FindAsync(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CompanyExists(Guid id)
    {
      return _context.Companies.Any(e => e.Id == id);
    }
}
