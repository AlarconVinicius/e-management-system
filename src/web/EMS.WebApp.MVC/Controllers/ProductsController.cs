using EMS.WebApp.MVC.Business.Interfaces.Repository;
using EMS.WebApp.MVC.Business.Models;
using EMS.WebApp.MVC.Business.Models.ViewModels;
using EMS.WebApp.MVC.Business.Utils.User;
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
    private readonly IUserRepository _userRepository;

    public ProductsController(IProductRepository productRepository, IAspNetUser appUser, IUserRepository userRepository)
    {
        _productRepository = productRepository;
        _appUser = appUser;
        _userRepository = userRepository;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 8;
        var productDb = await _productRepository.GetAllProducts(ps, page, q);
        var mappedProducts = new PagedViewModel<ProductViewModel>
        {
            List = productDb.List.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                UnitaryValue = p.UnitaryValue,
                Image = p.Image,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }),
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
        var productDb = await _productRepository.GetById(id);
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
            Image = productDb.Image,
            IsActive = productDb.IsActive,
            CreatedAt = productDb.CreatedAt,
            UpdatedAt = productDb.UpdatedAt

        };
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
        var userDb = await _userRepository.GetById(userId);
        var tenantId = _appUser.GetTenantId();
        if (ModelState.IsValid)
        {
            var mappedProduct = new Product(userDb.CompanyId, tenantId, product.Title, product.Description, product.UnitaryValue, product.Image, product.IsActive);
            _productRepository.AddProduct(mappedProduct);
            await _productRepository.UnitOfWork.Commit();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    [HttpGet("editar/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var productDb = await _productRepository.GetById(id);
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
            Image = productDb.Image,
            IsActive = productDb.IsActive,
            CreatedAt = productDb.CreatedAt,
            UpdatedAt = productDb.UpdatedAt

        };
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
                var productDb = await _productRepository.GetById(id);
                productDb.SetTitle(product.Title);    
                productDb.SetDescription(product.Description);    
                productDb.SetImage(product.Image);    
                productDb.SetUnitaryValue(product.UnitaryValue);    
                productDb.SetIsActive(product.IsActive);    

                _productRepository.UpdateProduct(productDb);
                await _productRepository.UnitOfWork.Commit();
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
        var productDb = await _productRepository.GetById(id);
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
            Image = productDb.Image,
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
        var productDb = await _productRepository.GetById(id);
        if (productDb is null)
        {
            return NotFound();
        }

        await _productRepository.DeleteProduct(productDb);

        await _productRepository.UnitOfWork.Commit();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ProductExists(Guid id)
    {
      return await _productRepository.GetById(id) is not null;
    }
}
