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
    public readonly ICompanyRepository _companyRepository;

    public ProductHandler(INotifier notifier, IAspNetUser appUser, IProductRepository productRepository, ICompanyRepository companyRepository) : base(notifier, appUser)
    {
        _productRepository = productRepository;
        _companyRepository = companyRepository;
    }

    public async Task<ProductResponse> GetByIdAsync(GetProductByIdRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            var product = await _productRepository.GetByIdAsync(request.Id, TenantId);

            if (product is null)
            {
                Notify("Produto não encontrado.");
                return null;
            }
            return product.MapProductToProductResponse();
        }
        catch
        {
            Notify("Não foi possível recuperar o produto.");
            return null;
        }
    }

    public async Task<PagedResponse<ProductResponse>> GetAllAsync(GetAllProductsRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            return (await _productRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, TenantId, request.Query)).MapPagedProductsToPagedResponseProducts();
        }
        catch
        {
            Notify("Não foi possível consultar os produtos.");
            return null;
        }
    }

    public async Task CreateAsync(CreateProductRequest request)
    {
        //if (!ExecuteValidation(new ProductValidation(), product)) return;
        if (TenantIsEmpty()) return;
        if (!CompanyExists(TenantId)) return;

        request.CompanyId = TenantId;
        var productMapped = request.MapCreateProductRequestToProduct();
        try
        {
            await _productRepository.AddAsync(productMapped);
            return;
        }
        catch
        {
            Notify("Não foi possível criar o produto.");
            return;
        }
    }

    public async Task UpdateAsync(UpdateProductRequest request)
    {
        //if (!ExecuteValidation(new ProductValidation(), product)) return;
        if (TenantIsEmpty()) return;
        if (!ProductExists(request.Id, TenantId)) return;

        var productDb = await _productRepository.GetByIdAsync(request.Id, TenantId);

        try
        {
            productDb.SetTitle(request.Title);
            productDb.SetDescription(request.Description);
            productDb.SetUnitaryValue(request.UnitaryValue);
            productDb.SetIsActive(request.IsActive);

            await _productRepository.UpdateAsync(productDb);

            return;
        }
        catch
        {
            Notify("Não foi possível atualizar o produto.");
            return;
        }
    }

    public async Task DeleteAsync(DeleteProductRequest request)
    {
        if (TenantIsEmpty()) return;
        try
        {
            if (!ProductExists(request.Id, TenantId)) return;

            await _productRepository.DeleteAsync(request.Id);

            return;
        }
        catch
        {
            Notify("Não foi possível deletar o produto.");
            return;
        }
    }

    private bool ProductExists(Guid id, Guid companyId)
    {
        if (_productRepository.SearchAsync(f => f.Id == id && f.CompanyId == companyId).Result.Any())
        {
            return true;
        };

        Notify("Produto não encontrado.");
        return false;
    }

    private bool CompanyExists(Guid companyId)
    {
        if (_companyRepository.GetByIdAsync(companyId).Result is not null)
        {
            return true;
        };

        Notify("TenantId não encontrado.");
        return false;
    }
}
