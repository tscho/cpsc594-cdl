using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MetricAnalyzer.Portal.Models;
using MetricAnalyzer.Portal.Infrastructure;
using MetricAnalyzer.Common.Models;

namespace MetricAnalyzer.Portal.Controllers
{
    public class MenuController : Controller
    {
        [DatabaseRequired]
        public ActionResult Index()
        {
            IndexModel model = new IndexModel();

            /*
            var plist = new List<Product>();
            plist.Add(new Product() { ProductID = -1, ProductName = "Select a Product" });
            plist.AddRange(DatabaseAccessor.GetProducts());
            model.Products = plist;
             */

            return View(model);
        }

        [HttpPost]
        [DatabaseRequired]
        public ActionResult Index(IndexModel model)
        {
            if (model.MetricIDs.Count() == 0)
                return View(model);

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
            iterationList.AddRange(DatabaseAccessor.GetIterations(new DateTime(DateTime.Now.Year, 1, 1)));
            model.Iterations = iterationList;
            model.StartIteration = model.EndIteration = -1;

            return View(model);
        }
    }
}
