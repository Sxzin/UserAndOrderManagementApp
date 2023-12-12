using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserAndOrderManagement.Models.Dto;
using UserAndOrderManagement.Models;
using UserAndOrderManagement.Repository.IRepository;
using UserAndOrderManagementApp.Repository.IRepository;
using UserAndOrderManagementApp.Models.Dto;
using UserAndOrderManagement.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UserAndOrderManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IOrderUpdateRep _dbOrder;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db, IOrderUpdateRep dbOrder, IMapper mapper)
        {
            _db = db;
            _dbOrder = dbOrder;
            _mapper = mapper;
            _response = new();
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllOrder()
        {
            try
            {

                var OrderList = await _dbOrder.GetOrdersAsync();
                _response.Result = _mapper.Map<List<OrderDto>>(OrderList);
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
        [HttpGet("{id:int}", Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var order = await _dbOrder.GetAsync(u => u.OrderId == id);
                if (order == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<OrderDto>(order);
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
        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] CreateOrderDto createDTO)
        {
            try
            {

                if (createDTO == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Invalid input data (createDTO is null).");
                    return BadRequest(ModelState);
                }
                var existingUsername = await _db.ApplicationUsers.FindAsync(createDTO.Id);
                if (existingUsername == null)
                {
                    return BadRequest("Invalid UsernameId");
                }
                var existingProduct = await _db.Products.FindAsync(createDTO.ProductId);
                if (existingProduct == null)
                {
                    return BadRequest("Invalid ProductId");
                }

                Order order = new Order
                {
                    ProductName = existingProduct.ProductName,
                    Price = existingProduct.Price,
                    UserName = existingUsername.UserName
                };

                _mapper.Map(createDTO, order);

                await _dbOrder.CreateOrderAsync(order);
                _response.Result = _mapper.Map<OrderDto>(order);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetOrder", new { id = order.OrderId }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteOrder")]
        public async Task<ActionResult<APIResponse>> DeleteOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var product = await _dbOrder.GetAsync(u => u.OrderId == id);
                if (product == null)
                {
                    return NotFound();
                }
                await _dbOrder.DeleteOrderAsync(product);
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
        [HttpPut("{id:int}", Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateOrder(int id, [FromBody] UpdateOrderDto updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.OrderId)
                {
                    return BadRequest();
                }

                var existingUsername = await _db.ApplicationUsers.FindAsync(updateDTO.Id);
                if (existingUsername == null)
                {
                    return BadRequest("Invalid UsernameId");
                }
                var existingProduct = await _db.Products.FindAsync(updateDTO.ProductId);
                if (existingProduct == null)
                {
                    return BadRequest("Invalid ProductId");
                }

                Order order = new Order
                {
                    ProductName = existingProduct.ProductName,
                    Price = existingProduct.Price,
                    UserName = existingUsername.UserName
                };

                _mapper.Map(updateDTO, order);

                await _dbOrder.UpdateAsync(order);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }

}
