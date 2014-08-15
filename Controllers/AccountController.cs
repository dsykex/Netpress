using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;
using System.Data.Common;
using System.Configuration;
using System.Security.Authentication;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;
using MySql.Data.Entity;
using MySql.Data.Types;
using Netpress.Models;

namespace Netpress.Controllers
{
    public class AccountController : Controller
    {
		public bool IsAdmin { get { return Session["IsAdmin"] != null && (bool)Session["IsAdmin"]; } }
		public bool IsMember { get { return Session["IsMember"] != null && (bool)Session["IsMember"]; } }

        public ActionResult Index()
        {
            return View ();
        }

		public ActionResult Login()
		{
			AccountSetup ();
			ViewBag.InvalidAcc = TempData ["InvalidAccount"];
			return View ();
		}

		public ActionResult Signup()
		{
			AccountSetup ();
			return View ();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult _Signup(int id, string username, string password, string displayName, string email, string rank, string profilePic)
		{
			NPDB db = new NPDB ();
			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = "SELECT * FROM accounts";
			MySqlDataReader reader = fetch.ExecuteReader ();

			while (reader.Read()) {
				Account _user = Account.Init(
								(int)reader["id"],
								(string)reader["username"],
								(string)reader["pword"],
								(string)reader["dname"],
								(string)reader["email"],
								(string)reader["rank"],
								(string)reader["profilepic"]);
				if (username == _user.username) {
					return RedirectToAction ("Signup", "Account");
				}
			}
			reader.Close ();
			id = Account.Max () + 1;
			MySqlCommand cmd = db.connection.CreateCommand ();

			cmd.CommandText = string.Format("INSERT INTO accounts (id, username, pword, dname, email, rank, profilepic) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}');",
			                                id,username,
			                                password,displayName,
			                                email,rank,
			                                profilePic);
			cmd.BeginExecuteNonQuery ();
			if (rank == "a")
			{
				Session["IsAdmin"] = true;
				Session["IsMember"] = true;
				FormsAuthentication.SetAuthCookie(username, true);
				return RedirectToAction("Index", "Home");
			}

			if (rank == "member")
			{
				Session["IsMember"] = true;
				Session["IsAdmin"] = false;
				TempData["Name"] = username;
				FormsAuthentication.SetAuthCookie(username, true);
				return RedirectToAction("Index", "Home");
			}

			return RedirectToAction ("Index", "Home");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Auth(string username, string password)
		{
			if (ModelState.IsValid)
			{
				Account userAuth = Account.GetUser(username,password);
				if (userAuth == null)
				{
					TempData["InvalidAccount"] = "Account is invalid.";
					return RedirectToAction("Login", "Account");
				}
				else
				{
					if (userAuth.rank == "a")
					{
						Session["IsAdmin"] = true;
						Session["IsMember"] = true;
						FormsAuthentication.SetAuthCookie(userAuth.username, true);
						return RedirectToAction("Index", "Home");
					}

					if (userAuth.rank == "member")
					{
						Session["IsMember"] = true;
						Session["IsAdmin"] = false;
						TempData["Name"] = userAuth.username;
						FormsAuthentication.SetAuthCookie(userAuth.username, true);
						return RedirectToAction("Index", "Home");
					}
				}
			}

			return View("Login");
		}

		public ActionResult DeAuth(string url)
		{
			Session["IsAdmin"] = null;
			Session["IsMember"] = null;
			FormsAuthentication.SignOut();
			//return RedirectToAction("Index", "Home");
			return Redirect (url);
		}

		public void AccountSetup()
		{
			if (Request.IsAuthenticated)
			{
				Account uAccount = Account.GetUser(User.Identity.Name);
				if (IsMember)
				{
					ViewBag.Username = uAccount.username;
					ViewBag.ProfilePic = uAccount.profilePic ?? "http://www.tenettech.com/Themes/Tenet/Content/images/default-profile-pic.jpg";
					ViewBag.UserRank = uAccount.rank;
					ViewBag.UserId = uAccount.id;
					ViewBag.IsMember = IsMember;
					if (IsAdmin)
					{
						ViewBag.IsAdmin = IsAdmin;
					}
				}
				else if (!IsMember)
				{
					Session["IsMember"] = false;
					Session["IsAdmin"] = false;
					ViewBag.IsMember = IsMember;
					ViewBag.IsAdmin = IsAdmin;
				}
			}
			else if (!Request.IsAuthenticated)
			{
				ViewBag.Username = "Guest";
				FormsAuthentication.SignOut();
				Session["IsMember"] = null;
				Session["IsAdmin"] = null;
			}
		}
    }
}
