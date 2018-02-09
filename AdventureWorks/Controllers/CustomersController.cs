using AdventureWorks.Models.DataTables;
using AdventureWorks.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AdventureWorks.Models.DataTables.DataTableServerSideRequest;

namespace AdventureWorks.Controllers
{
    public class CustomersController : Controller
    {
        private AdventureWorks2017Entities _context;

        public CustomersController(AdventureWorks2017Entities context)
        {
            _context = context;
        }
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }
    }
}