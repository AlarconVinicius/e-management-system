using EMS.Core.Configuration;
using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Products;
using EMS.WebApi.API.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApi.API.Controllers;

[Authorize]
[Route("api/v1/products/")]
public class ProductsController : ApiController
{
    public readonly IProductHandler _productHandler;
    public ProductsController(INotifier notifier, IProductHandler productHandler) : base(notifier)
    {
        _productHandler = productHandler;
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        GetProductByIdRequest request = new(id);
        var result = await _productHandler.GetByIdAsync(request);

        return IsOperationValid() ? ResponseOk(result) : ResponseBadRequest();

    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int ps = ConfigurationDefault.DefaultPageSize, [FromQuery] int page = ConfigurationDefault.DefaultPageNumber, [FromQuery] string q = null)
    {
        GetAllProductsRequest request = new()
        {
            PageSize = ps,
            PageNumber = page,
            Query = q
        };
        var result = await _productHandler.GetAllAsync(request);

        return IsOperationValid() ? ResponseOk(result) : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Post(CreateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ResponseBadRequest(ModelState);
        }
        await _productHandler.CreateAsync(request);

        return IsOperationValid() ? ResponseCreated() : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequest request)
    {
        if (id != request.Id) return ResponseBadRequest("Os IDs não correspondem.");
        if (!ModelState.IsValid)
        {
            return ResponseBadRequest(ModelState);
        }

        await _productHandler.UpdateAsync(request);

        return IsOperationValid() ? ResponseOk() : ResponseBadRequest();
    }

    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CustomResult), StatusCodes.Status400BadRequest)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productHandler.DeleteAsync(new DeleteProductRequest(id));

        return IsOperationValid() ? ResponseOk() : ResponseBadRequest();
    }
}
