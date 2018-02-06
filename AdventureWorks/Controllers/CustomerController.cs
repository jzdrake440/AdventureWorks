using AdventureWorks.Models.Dto;
using AdventureWorks.Models.Entity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AdventureWorks.Controllers
{
    public class CustomerController : ApiController
    {
        private IMapper _mapper;
        private IAdventureWorks2017Entities _context;
        
        public CustomerController(IMapper mapper, IAdventureWorks2017Entities context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET api/customer
        public IEnumerable<CustomerDto> GetCustomer()
        {
            return _mapper.Map<List<CustomerDto>>(_context.Customers.ToList());
        }

        // GET api/customer/5
        public string GetCustomer(int id)
        {
            return "value";
        }

        // POST api/customer
        [HttpPost]
        public void AddCustomer([FromBody]string value)
        {
        }

        // PUT api/customer/5
        [HttpPut]
        public void UpdateCustomer(int id, [FromBody]string value)
        {
        }

        // DELETE api/customer/5
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
        }
    }
}