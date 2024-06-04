using EMS.Core.Requests.Products;
using EMS.Core.Responses;
using EMS.Core.Responses.Products;

namespace EMS.Core.Handlers;
public interface IProductHandler
{
    Task<Response<ProductResponse>> CreateAsync(CreateProductRequest request);
    Task<Response<ProductResponse>> UpdateAsync(UpdateProductRequest request);
    Task<Response<ProductResponse>> DeleteAsync(DeleteProductRequest request);
    Task<Response<ProductResponse>> GetByIdAsync(GetProductByIdRequest request);
    Task<PagedResponse<List<ProductResponse>>> GetAllAsync(GetAllProductsRequest request);
}