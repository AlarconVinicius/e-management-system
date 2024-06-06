using EMS.Core.Requests.Products;
using EMS.Core.Responses;
using EMS.Core.Responses.Products;
using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Mappings;

public static class ProductMappings
{
    public static ProductResponse MapProductToProductResponse(this Product product)
    {
        if (product == null)
        {
            return null;
        }

        return new ProductResponse(product.Id, product.CompanyId, product.Title, product.Description, product.UnitaryValue, product.IsActive, product.CreatedAt, product.UpdatedAt);
    }

    public static Product MapProductResponseToProduct(this ProductResponse productResponse)
    {
        if (productResponse == null)
        {
            return null;
        }
        
        return new Product(productResponse.Id, productResponse.CompanyId, productResponse.Title, productResponse.Description, productResponse.UnitaryValue, productResponse.IsActive);
    }

    public static PagedResponse<ProductResponse> MapPagedProductsToPagedResponseProducts(this PagedResult<Product> product)
    {
        if (product == null)
        {
            return null;
        }

        return new PagedResponse<ProductResponse>(product.List.Select(x => x.MapProductToProductResponse()).ToList(), product.TotalResults, product.PageIndex, product.PageSize);
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