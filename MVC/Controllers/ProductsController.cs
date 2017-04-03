using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using MVC.Common;
using MVC.Models;

namespace MVC.Controllers
{
    public class ProductsController : BaseController
    {
        // GET: Products
        public ActionResult Index(string data = null)
        {
            List<ProductDto> products = null;

            if (data != null && TempData["Model"] != null)
            {
                var p = (ProductDto) TempData["Model"];
                ViewBag.Product = $"Success data change to {p.ProductName}";
            }

            var resp = new WebApiClient().Get<List<ProductDto>>("v1/Products/all");
            products = resp?.Data;
            if (products == null)
            {
                // Get - Debug basic error  info
                return HttpNotFound(resp?.Exception ?? "Response was null");
            }
            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var resp = new WebApiClient().Get<ProductDto>($"v1/Products/get/{id}");
            ProductDto product = resp?.Data;
            if (product == null)
            {
                // Get - Debug basic error  info
                return HttpNotFound(resp?.Exception ?? "Response was null");
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName")] ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var response = new WebApiClient().Post<ProductDto>($"v1/Products/add", product);
                if (response.Success && response.Data != null)
                {
                    TempData["Model"] = response.Data;
                    return RedirectToAction("Index", new {data = "Success"});
                }
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDto product = new WebApiClient().Get<ProductDto>($"v1/Products/get/{id}")?.Data;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName")] ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var response = new WebApiClient().Put<ProductDto>($"v1/Products/update/{product.ProductId}", product);
                if (response.Success)
                {
                    TempData["Model"] = product;
                    return RedirectToAction("Index", new { data = "Success" });
                }
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDto product = new WebApiClient().Get<ProductDto>($"v1/Products/get/{id}")?.Data;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var response = new WebApiClient().Delete<ProductDto>($"v1/Products/remove/{id}");
            if (response.Success)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id} );
        }
    }
}
