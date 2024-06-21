using EMS.Core.Notifications;
using Microsoft.AspNetCore.Authorization;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
public class ProductsController : MainController
{
    //private readonly IProductHandler _productHandler;
    //private readonly IAspNetUser _appUser;

    public ProductsController(INotifier notifier) : base(notifier)
    {
    }
    //public ProductsController(INotifier notifier, IProductHandler productHandler, IAspNetUser appUser) : base(notifier)
    //{
    //    _productHandler = productHandler;
    //    _appUser = appUser;
    //}

    //public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    //{
    //    var ps = 10;
    //    var request = new GetAllProductsRequest { PageNumber = page, PageSize = ps, Query = q };
    //    var response = await _productHandler.GetAllAsync(request);

    //    //var mappedProducts = new PagedViewModel<ProductResponse>
    //    //{
    //    //    List = response.Data,
    //    //    PageIndex = request.PageNumber,
    //    //    PageSize = request.PageSize,
    //    //    Query = request.Query,
    //    //    TotalResults = response.TotalCount
    //    //};
    //    //ViewBag.Search = q;
    //    //mappedProducts.ReferenceAction = "Index";

    //    //return View(mappedProducts);

    //    return View();
    //}

    //public async Task<IActionResult> Details(Guid id)
    //{
    //    var response = await GetById(id);
    //    if (response is null)
    //    {
    //        return NotFound();
    //    }
    //    return View(response);
    //}

    //public IActionResult Create()
    //{
    //    return View();
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create(CreateProductRequest request)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return View(request);
    //    }
    //    request.CompanyId = GetTenant();
    //    //var result = await _productHandler.CreateAsync(request);

    //    //if (result != null && !result.IsSuccess)
    //    //{
    //    //    Notify(result.Message);
    //    //    TempData["Failure"] = "Falha ao adicionar produto: " + string.Join("; ", await GetNotificationErrors());
    //    //    return View(request);
    //    //}
    //    //if (!IsValidOperation())
    //    //{
    //    //    TempData["Failure"] = "Falha ao adicionar produto: " + string.Join("; ", await GetNotificationErrors());
    //    //    return View(request);
    //    //}
    //    TempData["Success"] = "Produto adicionado com sucesso!";
    //    return RedirectToAction(nameof(Index));
    //}

    //public async Task<IActionResult> Edit(Guid id)
    //{
    //    var response = await GetById(id);
    //    if (response is null)
    //    {
    //        return NotFound();
    //    }
    //    //var request = new UpdateProductRequest(id, response.Data.CompanyId, response.Data.Title, response.Data.Description, response.Data.UnitaryValue, response.Data.IsActive);

    //    //return View(request);

    //    return View();
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(Guid id, UpdateProductRequest request)
    //{
    //    if (id != request.Id)
    //    {
    //        return NotFound();
    //    }
    //    if (!ModelState.IsValid)
    //    {
    //        return View(request);
    //    }

    //    request.CompanyId = GetTenant();
    //    //var result = await _productHandler.UpdateAsync(request);

    //    //if (result != null && !result.IsSuccess)
    //    //{
    //    //    Notify(result.Message);
    //    //    TempData["Failure"] = "Falha ao atualizar produto: " + string.Join("; ", await GetNotificationErrors());
    //    //    return View(request);
    //    //}
    //    //if (!IsValidOperation())
    //    //{
    //    //    TempData["Failure"] = "Falha ao atualizar produto: " + string.Join("; ", await GetNotificationErrors());
    //    //    return View(request);
    //    //}
    //    TempData["Success"] = "Produto atualizado com sucesso!";
    //    return RedirectToAction(nameof(Index));
    //}

    //public async Task<IActionResult> Delete(Guid id)
    //{
    //    var response = await GetById(id);
    //    if (response is null)
    //    {
    //        return NotFound();
    //    }

    //    return View(response);
    //}

    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(Guid id)
    //{
    //    var response = await GetById(id);
    //    if (response is null)
    //    {
    //        return NotFound();
    //    }

    //    await _productHandler.DeleteAsync(new DeleteProductRequest { Id = id });

    //    if (!IsValidOperation())
    //    {
    //        TempData["Failure"] = "Falha ao deletar produto: " + string.Join("; ", await GetNotificationErrors());
    //        return View(response);
    //    }

    //    TempData["Success"] = "Produto deletado com sucesso!";
    //    return RedirectToAction(nameof(Index));
    //}

    //private Guid GetTenant()
    //{
    //    return _appUser.GetTenantId();
    //}
    //private async Task<ProductResponse> GetById(Guid id)
    //{
    //    return await _productHandler.GetByIdAsync(new GetProductByIdRequest { Id = id });
    //}
}
