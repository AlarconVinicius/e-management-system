using EMS.Core.Requests.Products;
using EMS.Core.Responses.Products;
using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Mappings;

public static class ProductMappings
{
    public static ProductResponse MapProductToProductResponse(this Product product)
    {
        if (product == null)
        {
            return null;
        }

        return new ProductResponse
        {
            Id = product.Id,
            CompanyId = product.CompanyId,
            Title = product.Title,
            Description = product.Description,
            UnitaryValue = product.UnitaryValue,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public static Product MapProductResponseToProduct(this ProductResponse productResponse)
    {
        if (productResponse == null)
        {
            return null;
        }
        
        return new Product(productResponse.Id, productResponse.CompanyId, productResponse.Title, productResponse.Description, productResponse.UnitaryValue, productResponse.IsActive);
    }

    public static Product MapCreateProductRequestToProduct(this CreateProductRequest productRequest)
    {
        if (productRequest == null)
        {
            return null;
        }

        return new Product(productRequest.CompanyId, productRequest.Title, productRequest.Description, productRequest.UnitaryValue, productRequest.IsActive);
    }
}