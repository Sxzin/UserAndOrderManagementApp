using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using UserAndOrderManagement.Models;
using UserAndOrderManagement.Models.Dto;
using UserAndOrderManagement.Repository;
using UserAndOrderManagement.Repository.IRepository;

namespace UserAndOrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IProductRepository _dbProduct;
        private readonly IMapper _mapper;
        public ProductController(IProductRepository dbProduct, IMapper mapper)
        {
            _dbProduct = dbProduct;
            _mapper = mapper;
            _response = new();
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllProduct()
        {
            try
            {

               var ProductList = await _dbProduct.GetAllAsync();
                _response.Result = _mapper.Map<List<ProductDto>>(ProductList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var product = await _dbProduct.GetAsync(u => u.ProductId == id);
                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] CreateProductDto createDTO)
        {
            try
            {

                if (await _dbProduct.GetAsync(u => u.ProductName == createDTO.ProductName) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Product already Exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Invalid input data (createDTO is null).");
                    return BadRequest(ModelState);
                }
                Product product = _mapper.Map<Product>(createDTO);

                await _dbProduct.CreateAsync(product);
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProduct", new { id = product.ProductId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var product = await _dbProduct.GetAsync(u => u.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }
                await _dbProduct.RemoveAsync(product);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize]
        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int id, [FromBody] UpdateProductDto updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.ProductId)
                {
                    return BadRequest();
                }

                Product model = _mapper.Map<Product>(updateDTO);

                await _dbProduct.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize]
        [HttpPatch("{id:int}", Name = "UpdatePartialProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialProduct(int id, JsonPatchDocument<UpdateProductDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var product = await _dbProduct.GetAsync(u => u.ProductId == id, tracked: false);

            UpdateProductDto ProductDto = _mapper.Map<UpdateProductDto>(product);


            if (product == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(ProductDto, (Microsoft.AspNetCore.JsonPatch.JsonPatchError e) => ModelState.AddModelError("JsonPatchError", e.ErrorMessage));
            Product model = _mapper.Map<Product>(ProductDto);

            await _dbProduct.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
