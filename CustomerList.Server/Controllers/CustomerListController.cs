using BT.Model;
using BT.Model.Customer;
using Microsoft.AspNetCore.Mvc;

namespace CustomerList.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerListController : ControllerBase
    {
        private readonly ICustomerDataRepository _customerDataRepository;

        public CustomerListController(ICustomerDataRepository customerDataRepository)
        {
            _customerDataRepository = customerDataRepository ?? throw new ArgumentNullException(nameof(customerDataRepository));
        }

        [HttpPost]
        public async Task<ActionResult<ICustomerRecord>> Create([FromBody] CustomerDto customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCustomer = await Task.Run(() => _customerDataRepository.CreateCustomer(customer));
                return CreatedAtAction(nameof(Get), new { customerID = createdCustomer.CustomerID }, createdCustomer); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("{customerId}")]
        public async Task<ActionResult<ICustomerRecord>> Get(int customerId)
        {
            try
            {
                var customer = await Task.Run(() => _customerDataRepository.GetCustomerById(customerId));
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ICustomerRecord>>> GetAll()
        {
            var customers = await Task.Run(() => _customerDataRepository.GetAllCustomers());
            return Ok(customers);
        }
        
        [HttpPut]
        public async Task<ActionResult<ICustomerRecord>> Update([FromBody] CustomerDto customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCustomer = await Task.Run(() => _customerDataRepository.UpdateCustomer(customer));
                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpDelete("{customerId}")]
        public ActionResult<bool> Delete(int customerId)
        {
            try
            {
                bool customerDeleted = _customerDataRepository.DeleteCustomer(customerId);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}