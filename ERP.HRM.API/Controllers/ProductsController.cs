using ERP.HRM.Application.Common;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IUnitOfWork unitOfWork, ILogger<ProductsController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Create a new product for production salary calculation
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            try
            {
                _logger.LogInformation("Creating Product: {ProductCode}", request.ProductCode);

                var product = new Product
                {
                    ProductCode = request.ProductCode,
                    ProductName = request.ProductName,
                    Description = request.Description,
                    Unit = request.Unit ?? "cái",
                    Category = request.Category,
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.ProductRepository.AddAsync(product);

                _logger.LogInformation("Product created successfully. Id: {ProductId}", product.ProductId);

                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId },
                    new ApiResponse<ProductResponse>(true, "Sản phẩm được tạo thành công",
                        new ProductResponse
                        {
                            ProductId = product.ProductId,
                            ProductCode = product.ProductCode,
                            ProductName = product.ProductName,
                            Unit = product.Unit,
                            Category = product.Category
                        }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Product");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                    return NotFound(new ApiResponse<string>(false, "Sản phẩm không tìm thấy", null));

                return Ok(new ApiResponse<ProductResponse>(true, "Thành công",
                    new ProductResponse
                    {
                        ProductId = product.ProductId,
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        Description = product.Description,
                        Unit = product.Unit,
                        Category = product.Category,
                        Status = product.Status
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Product");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                var productResponses = products.Select(p => new ProductResponse
                {
                    ProductId = p.ProductId,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Unit = p.Unit,
                    Category = p.Category,
                    Status = p.Status
                });

                return Ok(new ApiResponse<IEnumerable<ProductResponse>>(true, "Thành công", productResponses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Products");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                    return NotFound(new ApiResponse<string>(false, "Sản phẩm không tìm thấy", null));

                product.ProductName = request.ProductName ?? product.ProductName;
                product.Description = request.Description ?? product.Description;
                product.Unit = request.Unit ?? product.Unit;
                product.Category = request.Category ?? product.Category;
                product.Status = request.Status ?? product.Status;
                product.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.ProductRepository.UpdateAsync(product);

                _logger.LogInformation("Product updated successfully. Id: {ProductId}", id);

                return Ok(new ApiResponse<ProductResponse>(true, "Cập nhật thành công",
                    new ProductResponse
                    {
                        ProductId = product.ProductId,
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        Unit = product.Unit,
                        Category = product.Category,
                        Status = product.Status
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Product");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Delete product (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                    return NotFound(new ApiResponse<string>(false, "Sản phẩm không tìm thấy", null));

                product.IsDeleted = true;
                product.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.ProductRepository.UpdateAsync(product);

                _logger.LogInformation("Product deleted successfully. Id: {ProductId}", id);

                return Ok(new ApiResponse<string>(true, "Xóa thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Product");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }

    public class CreateProductRequest
    {
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Unit { get; set; }
        public string? Category { get; set; }
    }

    public class UpdateProductRequest
    {
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Unit { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
    }

    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public string Unit { get; set; } = "cái";
        public string? Category { get; set; }
        public string Status { get; set; } = "Active";
    }
}
