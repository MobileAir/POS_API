using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using MVC.Filters;
using MVC.Common;
using MVC.DTOs;

namespace MVC.Controllers
{
    
    [RoutePrefix("categories")]
    public class CategoriesController : Controller
    {

        // TODO: Make method callable from _Sidebar with AJAX
        [Route("get-all")]
        public PartialViewResult GetAll()
        {
            List<CategoryDTO> categories = null;

            // why i do not get data back??? 500 internal server error....
            var r = new TokenAuthCrudClient().Get<List<CategoryDTO>>("v1/Category/all", Session["Token"].ToString(), Request.UserAgent);
            categories = r?.Data;
            if (categories == null)
            {
                // ReSharper disable once Mvc.PartialViewNotResolved
                return PartialView("_Sidebar", new List<CategoryDTO>());
            }
            return PartialView("_Sidebar", categories);
        }

        // GET: Categories
        [Route("all")]
        public ActionResult Index(string data = null)
        {
            List<CategoryDTO> categories = null;

            if (data != null && TempData["Model"] != null)
            {
                var p = (CategoryDTO)TempData["Model"];
                ViewBag.Category = $"Success data change to {p.Name}";
            }
            var r = new TokenAuthCrudClient().Get<List<CategoryDTO>>("v1/Categories/all", Session["Token"].ToString(), Request.UserAgent);
            categories = r?.Data;
            if (categories == null)
            {
                // Get - Debug basic error  info - basic handliing better should be done
                if (r?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new HttpUnauthorizedResult(r?.ReasonPhrase);
                }
                return HttpNotFound(r?.Exception ?? "Response was null");
            }
            return View(categories);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sessionToken = Session["Token"];
            var resp = new TokenAuthCrudClient().Get<CategoryDTO>($"v1/Categories/get/{id}", sessionToken.ToString(), Request.UserAgent);
            CategoryDTO category = resp?.Data;
            if (category == null)
            {
                // Get - Debug basic error  info
                return HttpNotFound(resp?.Exception ?? "Response was null");
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                var sessionToken = Session["Token"];
                var response = new TokenAuthCrudClient().
                    Post<CategoryDTO>($"v1/Categories/add", sessionToken.ToString(), Request.UserAgent, category);
                if (response.Success && response.Data != null)
                {
                    TempData["Model"] = response.Data;
                    return RedirectToAction("Index", new { data = "Success" });
                }
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sessionToken = Session["Token"];
            CategoryDTO category = new TokenAuthCrudClient().Get<CategoryDTO>($"v1/Categories/get/{id}", sessionToken.ToString(), Request.UserAgent)?.Data;
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                var sessionToken = Session["Token"];
                var response = new TokenAuthCrudClient().
                    Put<CategoryDTO>($"v1/Categories/update/{category.ID}", sessionToken.ToString(), Request.UserAgent, category);
                if (response.Success)
                {
                    TempData["Model"] = category;
                    return RedirectToAction("Index", new { data = "Success" });
                }
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sessionToken = Session["Token"];
            CategoryDTO category = new TokenAuthCrudClient().Get<CategoryDTO>($"v1/Categories/get/{id}", sessionToken.ToString(), Request.UserAgent)?.Data;
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sessionToken = Session["Token"];
            var response = new TokenAuthCrudClient().Delete<CategoryDTO>($"v1/Categories/remove/{id}", sessionToken.ToString(), Request.UserAgent);
            if (response.Success)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id });
        }
    }
}
