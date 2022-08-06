using ApniShop.Data;
using ApniShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient.Memcached;
using Microsoft.EntityFrameworkCore;

namespace ApniShop.Controllers
{
    public class UserController : Controller 
    {


        private readonly ApniShopDbContext userDB;

        public UserController(ApniShopDbContext db) {
            userDB = db;
        }

        //GET - Register
        public IActionResult Register()
        {
            return View();
        }

        //POST - Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User obj) {

            if (ModelState.IsValid)
            {
                userDB.User.Add(obj);
                userDB.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }

        //GET - Login
        public IActionResult Login()
        {
            return View();
        }

        // POST - Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User obj) { 
                
           var objUser = userDB.User.Where(a => a.Email.Equals(obj.Email) && a.Password.Equals(obj.Password)).FirstOrDefault();
            if (objUser != null)
            {
                HttpContext.Session.SetString("Name", objUser.Name.ToString());
                HttpContext.Session.SetString("MobileNo", objUser.MobileNo.ToString());
                HttpContext.Session.SetString("Email", objUser.Email.ToString());
                TempData["Id"] = objUser.Id;
                return View("Profile", objUser);
            }
            else
            {
                ViewBag.ErrorMessage = "Email or Password is not correct.";
                return View();
            }  
        }
        public IActionResult Profile(User objUser) {
            IEnumerable<User> objList = userDB.User;
            User userObj = new User();

            foreach (var obj in objList) {
                if (obj.Id == (int)TempData.Peek("Id")) {
                    userObj = obj;
                    TempData["Id"] = userObj.Id;
                    return View(userObj);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Profile(IFormFile ImageData)
        {
            IEnumerable<User> objList = userDB.User;
            User userObj = new User();

            foreach (var obj in objList)
            {
                if (obj.Id == (int)TempData.Peek("Id"))
                {
                    userObj = obj;
                    TempData["Id"] = userObj.Id;
                }
            }
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
                    userObj.Image = p1;

                }
            }

            userDB.User.Update(userObj);
            userDB.SaveChanges();


            return View("Profile",userObj);
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
            var q = from temp in userDB.User where temp.Id == id select temp.Image;
            byte[] cover = q.First();
            return cover;
        }

        public ActionResult DeletePicture(int? id) {
            IEnumerable<User> objList = userDB.User;
            User userObj = new User();

            foreach (var obj in objList)
            {
                if (obj.Id == id)
                {
                    userObj = obj;
                    TempData["Id"] = userObj.Id;
                }
            }

            userObj.Image = null;
            userDB.User.Update(userObj);
            userDB.SaveChanges();

            return View("Profile", userObj);
        }

        // GET - UPDATE
        public IActionResult Update(int? id) {

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = userDB.User.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(User obj) {

            if (ModelState.IsValid)
            {
                var q = from temp in userDB.User where temp.Id == (int)TempData.Peek("Id") select temp.Image;
                byte[] image = q.First();
                obj.Image = image;
                userDB.User.Update(obj);
                userDB.SaveChanges();
                return View("Profile", obj);
            }
            return View(obj);
        }

    }
}
