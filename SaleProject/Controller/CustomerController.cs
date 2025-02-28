using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SaleProject.Models;
using SaleProject.Models.Request;
using SaleProject.Models.Response;

namespace SaleProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly SaleProjectContext _context;
        public CustomerController(SaleProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                List<Customer>? customers = await _context.Customers.ToListAsync();
                if (customers == null)
                    return NotFound();
                response.Success = true;
                response.Data = customers;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerRequest customer)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Customer customerEntity = new Customer();
                customerEntity.Name = customer.Name;
                EntityEntry customerCreated = await _context.AddAsync(customerEntity);
                await _context.SaveChangesAsync();
                response.Data = customerCreated.Entity;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return Ok(response);
        }
        
        [HttpGet("{id}")]

        public async Task<IActionResult> Get(int id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customerEntity == null)
                {
                    response.Success = false;
                    response.Message = "Customer not found";
                    return NotFound(response);
                }
                response.Data = customerEntity;
                response.Success = true;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPut]

        public async Task<IActionResult> Edit([FromBody] CustomerRequest customer)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
                if (customerEntity == null)
                {
                    response.Success = false;
                    response.Message = "Customer not found";
                    return NotFound(response);
                }
                customerEntity.Name = customer.Name;
                _context.Customers.Update(customerEntity);
                int res = await _context.SaveChangesAsync();
                response.Success = true;
                response.Data = customerEntity;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Remove(int id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customerEntity == null)
                {
                    response.Success = false;
                    response.Message = "Customer not found";
                    return NotFound(response);
                }
                _context.Customers.Remove(customerEntity);
                await _context.SaveChangesAsync();
                response.Message = "Customer Deleted";
                response.Success = true;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }

            return Ok(response);
        }
    }
}
