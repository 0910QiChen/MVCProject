using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        public ActionResult Signup(User user)
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

            userDAL.NewUser(user);
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