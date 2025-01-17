﻿using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MVCProject.DAL;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class UserController : Controller
    {
        UserDAL userDAL = new UserDAL();

        public ActionResult Signup()
        {
            return View("Signup");
        }

        [HttpPost]
        public ActionResult Signup(User user, HttpPostedFileBase profilePic)
        {
            if (userDAL.IsEmailExists(user.email) == true)
            {
                ModelState.AddModelError("email", "Email address already exists!");
            }
            else if (userDAL.IsUsernameExists(user.username) == true)
            {
                ModelState.AddModelError("username", "Username already exists!");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (profilePic != null && profilePic.ContentLength > 0)
            {
                string filename = Path.GetFileName(profilePic.FileName);
                string uploadDir = Server.MapPath("~/Images/");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                string path = Path.Combine(uploadDir, filename);
                profilePic.SaveAs(path);
                user.profilePicPath = path;
            }
            userDAL.NewUser(user);
            TempData["SuccessMessage"] = "User Registered Successfully!";
            return RedirectToAction("UserList");
        }

        [HttpGet]
        public ActionResult UserList()
        {
            List<User> user = userDAL.GetUsers();
            return View(user);
        }
    }
}