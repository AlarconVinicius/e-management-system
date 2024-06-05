using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Products;
using EMS.Core.Responses;
using EMS.Core.Responses.Products;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Mappings;
using EMS.WebApi.Business.Services;
using EMS.WebApi.Business.Utils;

namespace EMS.WebApi.Business.Handlers;
public class ProductHandler : MainService, IProductHandler
{
    public readonly IProductRepository _productRepository;

    public ProductHandler(INotifier notifier, IAspNetUser appUser, IProductRepository productRepository) : base(notifier, appUser)
    {
        _productRepository = productRepository;
    }

    public async Task<Response<ProductResponse>> GetByIdAsync(GetProductByIdRequest request)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            return product is null
                ? new Response<ProductResponse>(null, 200, "Produto não encontrado.")
                : new Response<ProductResponse>(product.MapProductToProductResponse());
        }
        catch
        {
            return new Response<ProductResponse>(null, 500, "Não foi possível recuperar o produto.");
        }
    }

    public async Task<PagedResponse<List<ProductResponse>>> GetAllAsync(GetAllProductsRequest request)
    {
        try
        {
            var products = await _productRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, request.Query);

            return new PagedResponse<List<ProductResponse>>(
                products.List.Select(x => x.MapProductToProductResponse()).ToList(),
                products.TotalResults,
                products.PageIndex,
                products.PageSize);
        }
        catch
        {
            return new PagedResponse<List<ProductResponse>>(null, 500, "Não foi possível consultar os produtos.");
        }
    }

    public async Task<Response<ProductResponse>> CreateAsync(CreateProductRequest request)
    {
        //if (!ExecuteValidation(new ProductValidation(), product)) return;
        var productMapped = request.MapCreateProductRequestToProduct();
        try
        {
            await _productRepository.AddAsync(productMapped);
            return new Response<ProductResponse>(productMapped.MapProductToProductResponse(), 201, "Produto criado com sucesso!");
        }
        catch
        {
            return new Response<ProductResponse>(null, 500, "Não foi possível criar o produto.");
        }
    }

    public async Task<Response<ProductResponse>> UpdateAsync(UpdateProductRequest request)
    {
        //if (!ExecuteValidation(new ProductValidation(), product)) return;
        if (!await ProductExists(request.Id)) return null;
        var productDb = await _productRepository.GetByIdAsync(request.Id);

        try
        {
            productDb.SetTitle(request.Title);
            productDb.SetDescription(request.Description);
            productDb.SetUnitaryValue(request.UnitaryValue);
            productDb.SetIsActive(request.IsActive);

            await _productRepository.UpdateAsync(productDb);

            return new Response<ProductResponse>(null, 204, "Produto atualizado com sucesso!");
        }
        catch
        {
            return new Response<ProductResponse>(null, 500, "Não foi possível atualizar o produto.");
        }
    }

    public async Task<Response<ProductResponse>> DeleteAsync(DeleteProductRequest request)
    {
        try
        {
            if (!await ProductExists(request.Id)) return null;

            await _productRepository.DeleteAsync(request.Id);

            return new Response<ProductResponse>(null, 204, "Produto deletado com sucesso!");
        }
        catch
        {
            return new Response<ProductResponse>(null, 500, "Não foi possível deletar o produto.");
        }
    }

    private async Task<bool> ProductExists(Guid id)
    {
        var productExist = await _productRepository.GetByIdAsync(id);

        if (productExist != null)
        {
            return true;
        };

        Notify("Produto não encontrado.");
        return false;
    }
}
