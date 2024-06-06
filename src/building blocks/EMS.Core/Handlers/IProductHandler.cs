using EMS.Core.Requests.Products;
using EMS.Core.Responses;
using EMS.Core.Responses.Products;

namespace EMS.Core.Handlers;

public interface IProductHandler
{
    Task CreateAsync(CreateProductRequest request);
    Task UpdateAsync(UpdateProductRequest request);
    Task DeleteAsync(DeleteProductRequest request);
    Task<ProductResponse> GetByIdAsync(GetProductByIdRequest request);
    Task<PagedResponse<ProductResponse>> GetAllAsync(GetAllProductsRequest request);
}