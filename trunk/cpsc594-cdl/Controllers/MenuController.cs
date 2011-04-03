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
        [DatabaseRequired]
        public ActionResult Index()
        {
            IndexModel model = new IndexModel();

            var plist = new List<Product>();
            plist.Add(new Product() { ProductID = -1, ProductName = "Select a Product" });
            plist.AddRange(DatabaseAccessor.GetProducts());
            model.Products = plist;

            return View(model);
        }

        [HttpPost]
        [DatabaseRequired]
        public ActionResult Index(IndexModel model)
        {
            if (model.MetricIDs.Contains(-1))
                return View(model);

            var metricIds = Enum.GetValues(typeof(MetricType));
            model.MetricIDs = Enumerable.Range(0, metricIds.Length);

            var productList = new List<Product>();
            productList.Add(new Product() { ProductID = -1, ProductName = "Select a Product" });
            productList.AddRange(DatabaseAccessor.GetProducts());
            model.Products = productList;

            var componentList = new List<Component>();
            componentList.Add(new Component() { ComponentID = -1, ProductID = -1, ComponentName = "Select All" });
            componentList.AddRange(DatabaseAccessor.GetComponents(Convert.ToInt32(model.ProductID)));
            model.Components = componentList;

            var iterationList = new List<Iteration>();
            iterationList.Add(new Iteration() { IterationID = -1, IterationLabel = "Select" });
            iterationList.AddRange(DatabaseAccessor.GetIterations(26));
            model.Iterations = iterationList;

            return View(model);
        }
    }
}
