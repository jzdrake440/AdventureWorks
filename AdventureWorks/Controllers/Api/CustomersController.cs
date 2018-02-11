using AdventureWorks.Models.DataTables;
using AdventureWorks.Models.Dto;
using AdventureWorks.Models.Entity;
using AdventureWorks.Utility;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static AdventureWorks.Models.DataTables.DataTableServerSideRequest;

namespace AdventureWorks.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private IMapper _mapper;
        private AdventureWorks2017Entities _context;
        
        public CustomersController(IMapper mapper, AdventureWorks2017Entities context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET api/customer
        [HttpGet]
        public IHttpActionResult GetCustomers([FromUri]DataTableServerSideRequest request)
        {
            if (request == null) //called for all customers
                return Ok(_mapper.Map<List<CustomerDto>>(_context.Customers.ToList()));

            //called specifically from DataTables api
            DataTableServerSideResponse<CustomerDto> response = new DataTableServerSideResponse<CustomerDto>
            {
                Draw = request.Draw, //used by DataTables api to prevent xss
                RecordsTotal = _context.Customers.Count() //total number of records before filtering
            };

            //Search Filtering
            var data = DataTableUtility.FilterData(request, _context.Customers);
            response.RecordsFiltered = data.Count(); //total number of records after filtering

            //Ordering
            DataTableUtility.OrderData(request, data);

            //Pagination
            if (data.Count < request.Start + request.Length) //i.e. start = 51, length = 10, count = 55; 55 < 51 + 10
                response.Data = _mapper.Map<List<CustomerDto>>(data.GetRange(request.Start, data.Count - request.Start));// i.e. cont GetRange(51, 55-51)
            else
                response.Data = _mapper.Map<List<CustomerDto>>(data.GetRange(request.Start, request.Length));
            
            return Ok(response);
        }

        // GET api/customer/5
        [HttpGet]
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerID == id);

            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerDetailDto>(customer));
        }

        // POST api/customer
        [HttpPost]
        public IHttpActionResult AddCustomer(CustomerDetailDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _mapper.Map<Customer>(customerDto);

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return Created(
                new Uri(Request.RequestUri + customer.CustomerID.ToString()),
                _mapper.Map<CustomerDetailDto>(customer));
        }

        // PUT api/customer/5
        [HttpPut]
        public IHttpActionResult UpdateCustomer(int id, CustomerDetailDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = _context.Customers.SingleOrDefault(c => c.CustomerID == customerDto.CustomerID);

            if (customer == null)
                return NotFound();

            _mapper.Map<CustomerDetailDto, Customer>(customerDto, customer);

            _context.SaveChanges();

            return Ok(_mapper.Map<CustomerDetailDto>(customer)); //remap customer to dto in case of backend updates
        }

        // DELETE api/customer/5
        [HttpDelete]
        public IHttpActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerID == id);

            if (customer == null)
                return BadRequest();

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return Ok();
        }
    }
}