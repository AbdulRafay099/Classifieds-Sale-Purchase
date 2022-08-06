using ApniShop.Data;
using ApniShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApniShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApniShopDbContext productDB;

        public ProductController(ApniShopDbContext db)
        {
            productDB = db;
        }
        //GET - HOME
        public IActionResult Home()
        {
            IEnumerable<Product> productList = productDB.Product;
            return View(productList);
        }

        //POST - CREATE
        [HttpPost]
        public IActionResult Home(IFormFile ImageData, int? id)
        {
            IEnumerable<Product> productList = productDB.Product;
            foreach (var obj in productList) {
                if (obj.Id == id) {
                    if (ImageData != null)

                    {
                        if (ImageData.Length > 0)

                        //Convert Image to byte and save to database

                        {

                            byte[] p1 = null;
                            using (var fs1 = ImageData.OpenReadStream())
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                p1 = ms1.ToArray();
                            }
                            obj.Image = p1;

                        }
                    }

                    productDB.Product.Update(obj);
                }
            }
            productDB.SaveChanges();

            return RedirectToAction("Home");
        }

        public ActionResult GetImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageFromDataBase(int id)
        {
            var q = from temp in productDB.Product where temp.Id == id select temp.Image;
            byte[] cover = q.First();
            return cover;
        }

        //GET - CREATE
        public IActionResult Create() {
            return View();
        }

        // POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                productDB.Product.Add(obj);
                productDB.SaveChanges();
                return RedirectToAction("Home");
            }
            return View(obj);
        }

        // GET - POST
        public IActionResult Edit(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = productDB.Product.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            TempData["Id"] = id;

            return View(obj);
        }

        // POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                var q = from temp in productDB.Product where temp.Id == (int)TempData.Peek("Id") select temp.Image;
                byte[] image = q.First();
                obj.Image = image;
                productDB.Product.Update(obj);
                productDB.SaveChanges();
                return RedirectToAction("Home");
            }
            return View(obj);
        }

        // GET - Delete
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = productDB.Product.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = productDB.Product.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            productDB.Product.Remove(obj);
            productDB.SaveChanges();
            return RedirectToAction("Home");
        }
    }
}
