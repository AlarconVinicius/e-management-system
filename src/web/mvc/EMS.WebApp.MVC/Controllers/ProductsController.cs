using AutoMapper;
using EMS.WebApp.Business.Interfaces.Repositories;
using EMS.WebApp.Business.Models;
using EMS.WebApp.Business.Utils;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
//[Route("dashboard/produtos")]
public class ProductsController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IAspNetUser _appUser;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository productRepository, IAspNetUser appUser, IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _appUser = appUser;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 8;
        var productDb = await _productRepository.GetAllPagedAsync(ps, page, q);
        var mappedProducts = new PagedViewModel<ProductViewModel>
        {
            List = _mapper.Map<List<ProductViewModel>>(productDb.List),
            PageIndex = productDb.PageIndex,
            PageSize = productDb.PageSize,
            Query = productDb.Query,
            TotalResults = productDb.TotalResults
        };
        ViewBag.Search = q;
        mappedProducts.ReferenceAction = "Index";
        //if (!string.IsNullOrEmpty(search))
        //{
        //    mappedProducts = mappedProducts.Where(p => p.Title.Contains(search) || p.Description.Contains(search));
        //}

        return View(mappedProducts);
    }

    [HttpGet("detalhes/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var productDb = await _productRepository.GetByIdAsync(id);
        if (productDb is null)
        {
            return NotFound();
        }
        var mappedProduct = _mapper.Map<ProductViewModel>(productDb);
        return View(mappedProduct);
    }

    [HttpGet("adicionar")]
    public IActionResult Create()
    {
        //ViewData["CompanyId"] = new SelectList(_productRepository.Companies, "Id", "Name");
        return View();
    }

    [HttpPost("adicionar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel product)
    {
        var userId = _appUser.GetUserId();
        var userDb = await _employeeRepository.GetByIdAsync(userId);
        var tenantId = _appUser.GetTenantId();
        if (ModelState.IsValid)
        {
            var mappedProduct = _mapper.Map<Product>(product);
            await _productRepository.AddAsync(mappedProduct);
            TempData["Success"] = "Produto adicionado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    [HttpGet("editar/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var productDb = await _productRepository.GetByIdAsync(id);
        if (productDb is null)
        {
            return NotFound();
        }

        var mappedProduct = _mapper.Map<ProductViewModel>(productDb);
        //ViewData["CompanyId"] = new SelectList(_productRepository.Companies, "Id", "Name", product.CompanyId);
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
                var productDb = await _productRepository.GetByIdAsync(id);
                productDb.SetTitle(product.Title);    
                productDb.SetDescription(product.Description);  
                productDb.SetUnitaryValue(product.UnitaryValue);    
                productDb.SetIsActive(product.IsActive);    

                await _productRepository.UpdateAsync(productDb);
                TempData["Success"] = "Produto atualizado com sucesso!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await ProductExists(product.Id))
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
        //ViewData["CompanyId"] = new SelectList(_productRepository.Companies, "Id", "Name", product.CompanyId);
        return View(product);
    }

    [HttpGet("deletar/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var productDb = await _productRepository.GetByIdAsync(id);
        if (productDb is null)
        {
            return NotFound();
        }
        var mappedProduct = new ProductViewModel
        {
            Id = productDb.Id,
            Title = productDb.Title,
            Description = productDb.Description,
            UnitaryValue = productDb.UnitaryValue,
            IsActive = productDb.IsActive,
            CreatedAt = productDb.CreatedAt,
            UpdatedAt = productDb.UpdatedAt
        };

        return View(mappedProduct);
    }

    [HttpPost("deletar/{id}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var productDb = await _productRepository.GetByIdAsync(id);
        if (productDb is null)
        {
            return NotFound();
        }

        await _productRepository.DeleteAsync(id);

        TempData["Success"] = "Produto deletado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ProductExists(Guid id)
    {
      return await _productRepository.GetByIdAsync(id) is not null;
    }
}
