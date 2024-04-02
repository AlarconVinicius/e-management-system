using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
using EMS.WebApp.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
[Route("dashboard/produtos")]
public class ProductsController : Controller
{
    private readonly EMSDbContext _context;
    private readonly IAspNetUser _appUser;
    private readonly IUserRepository _userRepository;

    public ProductsController(EMSDbContext context, IAspNetUser appUser, IUserRepository userRepository)
    {
        _context = context;
        _appUser = appUser;
        _userRepository = userRepository;
    }

    public async Task<IActionResult> Index()
    {
        var eMSDbContext = _context.Products.Include(p => p.Company);
        var mappedProducts = (await eMSDbContext).Select(p => new ProductViewModel
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            UnitaryValue = p.UnitaryValue,
            Image = p.Image,
            IsActive = p.IsActive

        });
        return View(mappedProducts);
    }

    [HttpGet("detalhes/{id}")]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Company)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpGet("adicionar")]
    public IActionResult Create()
    {
        ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
        return View();
    }

    [HttpPost("adicionar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel product)
    {
        var userId = _appUser.GetUserId();
        var userDb = await _userRepository.GetById(userId);
        if (ModelState.IsValid)
        {
            var mappedProduct = new Product(userDb.CompanyId, product.Title, product.Description, product.UnitaryValue, product.Image, product.IsActive);
            _context.Add(mappedProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    [HttpGet("editar/{id}")]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        var mappedProduct = new ProductViewModel
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            UnitaryValue = product.UnitaryValue,
            Image = product.Image,
            IsActive = product.IsActive
        };
        if (product == null)
        {
            return NotFound();
        }
        //ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", product.CompanyId);
        return View(mappedProduct);
    }

    [HttpPost("editar/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProductViewModel product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
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
        //ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", product.CompanyId);
        return View(product);
    }

    [HttpGet("deletar/{id}")]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Company)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost("deletar/{id}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Products == null)
        {
            return Problem("Entity set 'EMSDbContext.Products'  is null.");
        }
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(Guid id)
    {
      return _context.Products.Any(e => e.Id == id);
    }
}
