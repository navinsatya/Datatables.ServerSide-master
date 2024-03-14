using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }
        public List<Customer> Customers { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        public string FirstNameSort { get; set; } = "firstname_asc";
        public string LastNameSort { get; set; } = "lastname_asc";
        public string ContactSort { get; set; } = "contact_asc";
        public string EmailSort { get; set; } = "email_asc";
        public string DateOfBirthSort { get; set; } = "dateofbirth_asc";
        public string ReadSort { get; set; } = "read_asc";
        public string WriteSort { get; set; } = "write_asc";
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1; // Default to first page

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10; // Default page size

        public int TotalItems { get; set; }
        public async Task OnGetAsync(string firstNameSort, string lastNameSort, string contactSort, string emailSort, string dateOfBirthSort, string readSort, string writeSort)
        {
            ViewDataSetup(firstNameSort, lastNameSort, contactSort, emailSort, dateOfBirthSort, readSort, writeSort);

            IQueryable<Customer> items = _context.Customers;
            if (!string.IsNullOrEmpty(SearchString))
            {
                items = items.Where(i => i.FirstName.Contains(SearchString) || i.LastName.Contains(SearchString));
            }

            switch (firstNameSort)
            {
                case "firstname_asc":
                    items = items.OrderBy(i => i.FirstName);
                    break;
                case "firstname_desc":
                    items = items.OrderByDescending(i => i.FirstName);
                    break;
            }
            
            switch (lastNameSort)
            {
                case "lastname_asc":
                    items = items.OrderBy(i => i.LastName);
                    break;
                case "lastname_desc":
                    items = items.OrderByDescending(i => i.LastName);
                    break;
            }
            switch (contactSort)
            {
                case "contact_asc":
                    items = items.OrderBy(i => i.Contact);
                    break;
                case "contact_desc":
                    items = items.OrderByDescending(i => i.Contact);
                    break;
            }
            switch (emailSort)
            {
                case "email_asc":
                    items = items.OrderBy(i => i.Email);
                    break;
                case "email_desc":
                    items = items.OrderByDescending(i => i.Email);
                    break;
            }
            switch (dateOfBirthSort)
            {
                case "dateofbirth_asc":
                    items = items.OrderBy(i => i.LastName);
                    break;
                case "dateofbirth_desc":
                    items = items.OrderByDescending(i => i.LastName);
                    break;
            }
            switch (readSort)
            {
                case "read_asc":
                    items = items.OrderBy(i => i.Read);
                    break;
                case "read_desc":
                    items = items.OrderByDescending(i => i.Read);
                    break;
            }
            switch (writeSort)
            {
                case "write_asc":
                    items = items.OrderBy(i => i.Write);
                    break;
                case "write_desc":
                    items = items.OrderByDescending(i => i.Write);
                    break;
            }
            var query = _context.Customers.AsQueryable(); // Your customer query

            // Calculate total items
            TotalItems = await items.CountAsync();

            // Retrieve paged data
            Customers = await items.Skip((PageNumber - 1) * PageSize)
                                   .Take(PageSize)
                                   .ToListAsync();
            //Customers = await items.AsNoTracking().ToListAsync();
        }

        private void ViewDataSetup(string firstNameSort, string lastNameSort, string contactSort, string emailSort, string dateOfBirthSort, string readSort, string writeSort)
        {
            ViewData["FirstNameSort"] = string.IsNullOrEmpty(firstNameSort) ? "firstname_asc" : (firstNameSort == "firstname_asc" ? "firstname_desc" : "firstname_asc");
            ViewData["LastNameSort"] = string.IsNullOrEmpty(lastNameSort) ? "lastname_asc" : (lastNameSort == "lastname_asc" ? "lastname_desc" : "lastname_asc");
            ViewData["ContactSort"] = string.IsNullOrEmpty(contactSort) ? "contact_asc" : (contactSort == "contact_asc" ? "contact_desc" : "contact_asc");
            ViewData["EmailSort"] = string.IsNullOrEmpty(emailSort) ? "email_asc" : (emailSort == "email_asc" ? "email_desc" : "email_desc");
            ViewData["DateOfBirthSort"] = string.IsNullOrEmpty(dateOfBirthSort) ? "dateofbirth_asc" : (dateOfBirthSort == "dateofbirth_asc" ? "dateofbirth_desc" : "dateofbirth_desc");
            ViewData["ReadSort"] = string.IsNullOrEmpty(readSort) ? "read_asc" : (readSort == "read_asc" ? "read_desc" : "read_desc");
            ViewData["WriteSort"] = string.IsNullOrEmpty(writeSort) ? "write_asc" : (writeSort == "write_asc" ? "write_desc" : "write_desc");
        }
    }
}
