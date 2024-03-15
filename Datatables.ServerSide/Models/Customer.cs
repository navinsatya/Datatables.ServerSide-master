using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class Customer
    {
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Contact { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public DateTime DateOfBirth { get; set; }
        [BindProperty]
        public bool Read { get; set; }
        [BindProperty]
        public bool Write { get; set; }
    }
}
