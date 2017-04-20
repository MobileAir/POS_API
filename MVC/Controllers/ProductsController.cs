using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using MVC.Attributes;
using MVC.Common;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [TokenAuthCheck]
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string data = null)
        {
            List<ProductVm> products = null;

            if (data != null && TempData["Model"] != null)
            {
                var p = (ProductVm)TempData["Model"];
                ViewBag.Product = $"Success data change to {p.name}";
            }
            var r = new TokenAuthCrudClient().Get<List<ProductVm>>("v1/Products/all", Session["Token"].ToString(), Request.UserAgent);
            products = r?.Data;
            if (products == null)
            {
                // Get - Debug basic error  info - basic handliing better should be done
                if (r?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new HttpUnauthorizedResult(r?.ReasonPhrase);
                }
                return HttpNotFound(r?.Exception ?? "Response was null");
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
            var sessionToken = Session["Token"];
            var resp = new TokenAuthCrudClient().Get<ProductVm>($"v1/Products/get/{id}", sessionToken.ToString(), Request.UserAgent);
            ProductVm product = resp?.Data;
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
                var sessionToken = Session["Token"];
                var response = new TokenAuthCrudClient().
                    Post<ProductDto>($"v1/Products/add", sessionToken.ToString(), Request.UserAgent, product);
                if (response.Success && response.Data != null)
                {
                    TempData["Model"] = response.Data;
                    return RedirectToAction("Index", new { data = "Success" });
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
            var sessionToken = Session["Token"];
            ProductDto product = new TokenAuthCrudClient().Get<ProductDto>($"v1/Products/get/{id}", sessionToken.ToString(), Request.UserAgent)?.Data;
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
                var sessionToken = Session["Token"];
                var response = new TokenAuthCrudClient().
                    Put<ProductDto>($"v1/Products/update/{product.ProductId}", sessionToken.ToString(), Request.UserAgent, product);
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
            var sessionToken = Session["Token"];
            ProductDto product = new TokenAuthCrudClient().Get<ProductDto>($"v1/Products/get/{id}", sessionToken.ToString(), Request.UserAgent)?.Data;
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
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sessionToken = Session["Token"];
            var response = new TokenAuthCrudClient().Delete<ProductDto>($"v1/Products/remove/{id}", sessionToken.ToString(), Request.UserAgent);
            if (response.Success)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id });
        }
    }
}
