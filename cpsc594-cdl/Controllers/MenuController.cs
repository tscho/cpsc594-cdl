using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cpsc594_cdl.Models;
using cpsc594_cdl.Infrastructure;
using cpsc594_cdl.Common.Models;

namespace cpsc594_cdl.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Home/
        public MenuController()
        {
        }

        [DatabaseRequired]
        public ActionResult Index()
        {
            IndexModel model = new IndexModel();

            List<Product> plist = new List<Product>();
            plist.Add(new Product() { ProductID = -1, ProductName = "Select a Product" });
            plist.AddRange(DatabaseAccessor.GetProducts());
            model.Products = plist;

            return View(model);
        }

        [HttpPost]
        [DatabaseRequired]
        public ActionResult Index(IndexModel model)
        {
            List<Product> plist = new List<Product>();
            plist.Add(new Product() { ProductID = -1, ProductName = "Select a Product" });
            plist.AddRange(DatabaseAccessor.GetProducts());
            model.Products = plist;
            if (model.ProductID == -1) return View(model);

            List<Component> clist = new List<Component>();
            clist.Add(new Component() { ComponentID = -1, ProductID = -1, ComponentName = "Select All" });
            clist.AddRange(DatabaseAccessor.GetComponents(Convert.ToInt32(model.ProductID)));
            model.Components = clist;

            model.MetricIDs = new[] { 0, 1, 2, 3, 4, 5 };

            return View(model);
        }
    }
}
