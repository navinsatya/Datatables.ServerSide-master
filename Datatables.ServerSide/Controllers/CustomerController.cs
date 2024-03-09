using UserManagement.Data;
using UserManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public CustomerController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        [Route("Create")]

        public IActionResult GetCustomers()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = (from tempcustomer in context.Customers select tempcustomer);
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.FirstName.Contains(searchValue) 
                                                || m.LastName.Contains(searchValue) 
                                                || m.Contact.Contains(searchValue) 
                                                || m.Email.Contains(searchValue) );
                }
                recordsTotal = customerData.Count();
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Create([FromBody]Customer customer)
        {
            context.Add(customer);
            context.SaveChanges();
            return Ok("Customer created successfully.");
        }

        [HttpPost]
        [Route("SaveCustomers")]
        public IActionResult SaveCustomers([FromBody]List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                var existingCustomer = context.Customers.FirstOrDefault(c => c.Id == customer.Id);

                // If the customer exists, update its properties
                if (existingCustomer != null)
                {
                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LastName = customer.LastName;
                    existingCustomer.Contact = customer.Contact;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.DateOfBirth = customer.DateOfBirth;
                    existingCustomer.Read = customer.Read;
                    existingCustomer.Write = customer.Write;
                }
            }
            context.SaveChanges();
            return Ok("Customer created successfully.");
        }
    }
}