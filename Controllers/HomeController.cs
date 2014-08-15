using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;
using System.Data.Common;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;
using MySql.Data.Entity;
using MySql.Data.Types;
using Netpress.Models;

namespace Netpress.Controllers
{
	public class HomeController : Controller
	{
		//private MySqlConnection con;
		//private string cString;

		public NPDB db;
		public int postsPerPage;
		public NPDBEntity entity;
		//public List<Netpress.Models.Post> posts;
		public bool IsAdmin { get { return Session["IsAdmin"] != null && (bool)Session["IsAdmin"]; } }
		public bool IsMember { get { return Session["IsMember"] != null && (bool)Session["IsMember"]; } }
		string _time = DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss");
		public ActionResult Index (/*int id*/)
		{
			db = new NPDB ();
			Netpress.Models.Post[] posts = new Netpress.Models.Post[1024];
			MySqlCommand com = db.connection.CreateCommand ();
			com.CommandText = "SELECT * FROM posts";
			MySqlDataReader reader = com.ExecuteReader ();

			int i = 0;
			while (reader.Read()) {
				i++;
				Models.Post p = new Netpress.Models.Post();
				p.id = (int)reader ["id"];
				p.title = (string)reader ["title"];
				p.author = (string)reader ["author"];
				p.tags = (string)reader ["tags"];
				p.thumbnail = (string)reader ["thumbnail"];
				p.body = (string)reader ["body"];
				p.time = (DateTime)reader ["post_time"];
				posts [i] = p;
				ViewBag.Count = i;
			}
		
			reader.Close (); 
			//ExecuteCommand (db.connection);
			db.connection.Close ();
			ViewBag.PostDeleted = TempData ["PostDeleted"];
			AccountSetup ();
			return View (posts);
		}

		public ActionResult p(int id)
		{
			Post post = Post.GetPost (id);
			AccountSetup ();
			ViewBag.CommentSucc = TempData ["CommentSuccess"];
			ViewBag.ComDelete = TempData ["DeleteMessage"];
			return View (post);
		}

		public ActionResult edit_post(int? id)
		{
			int _id = (id == null) ? 0 : id.Value;

			Post post = Post.GetPost (_id);

			NPDB db = new NPDB ();
			MySqlCommand cmd = db.connection.CreateCommand ();

			if (Post.isNull (post)) {
				post = new Post (0, "Title..", "Tags..", "Image Url..","Author..", DateTime.Parse(_time), "Body..");
			} else {
				cmd.CommandText = string.Format("SELECT * FROM posts WHERE id={0}",_id);
				MySqlDataReader reader = cmd.ExecuteReader ();
				while (reader.Read()) {
					post = Post.Init (
						(int)reader ["id"],
						(string)reader ["title"],
						(string)reader ["tags"],
						(string)reader ["thumbnail"],
						(string)reader ["author"],
						(DateTime)reader ["post_time"],
						(string)reader ["body"]
					);
				}

				reader.Close ();
			}
		
			db.connection.Close ();
			AccountSetup ();
			return View (post);
		}

		public ActionResult UpdatePost(int? id, string title, string tags, string thumbnail, string body, string author, DateTime time)
		{
			Post post = new Post ();
			post.id = id.HasValue ? id.Value : 0;
			post.title = title;
			post.tags = tags;
			post.thumbnail = thumbnail;
			post.body = body;
			post.author = author;
			post.time = time;

			NPDB db = new NPDB ();
			MySqlCommand com = db.connection.CreateCommand ();

			string[] _tags = tags.Split (new char[] { ' ' });

			int returnId = 0;
			if (!id.HasValue || id == 0) {
				string _time = DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss");
				post.id = Post.Max()+1;
				foreach (string t in _tags) {
					Tag.AddToDb (Tag.Max () + 1, t);
				}

				com.CommandText = string.Format (
					"INSERT INTO posts (id, title, tags, thumbnail, author, body, post_time) " +
					"VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", 
					post.id, 
					post.title,
					post.tags, 
					post.thumbnail, 
					post.author, 
					post.body,
					_time
				);

				com.ExecuteNonQuery ();
			} else {
				string _time = time.ToString ("yyyy-MM-dd hh:mm:ss");
				com.CommandText = string.Format (
					"UPDATE posts " +
					"SET id='{0}', title='{1}', post_time='{2}', author='{3}', tags='{4}', thumbnail='{5}', body='{6}' " +
					"WHERE id={7}",
					post.id,
					post.title,
					_time,
					post.author,
					post.tags,
					post.thumbnail,
					post.body,
					post.id
				);
				com.ExecuteNonQuery ();

			}

			db.connection.Close ();
			returnId += post.id;
			return RedirectToAction("p", "Home", new { id = returnId });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeletePost(int id)
		{
			int counter = 0;
			Post post = Post.GetPost (id);
			NPDB db = new NPDB ();
			MySqlCommand delPostCmd = db.connection.CreateCommand ();
			MySqlCommand delPostComsCmd = db.connection.CreateCommand ();
			delPostCmd.CommandText = string.Format("DELETE FROM posts WHERE id={0}",id);
			delPostCmd.ExecuteNonQuery ();
			delPostComsCmd.CommandText = string.Format("DELETE FROM comments WHERE pid={0}",id);
			delPostComsCmd.ExecuteNonQuery ();
			db.connection.Close ();
			TempData ["PostDeleted"] = "Post at '("+id+")' Deleted.";
			return RedirectToAction ("Index", "Home");
		}

		/*private void ExecuteCommand(MySqlConnection c)
		{
			//string query = "INSERT INTO posts (id, title, post_time, author, tags, body) VALUES('1', 'Bagro Bang - Two', '"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"', 'DSykex', 'bang bang bruh', 'fdafdfdsffjsdhflkdjshfldjsfhlajhfdlsjfhdalhfsdjhljdafsfsfsfsfsfsfafdsf')";
			MySqlCommand cmd = new MySqlCommand(query, c);
			cmd.ExecuteNonQuery();
		}*/

		private ActionResult UpdatePost()
		{
			AccountSetup ();
			return RedirectToAction ("Index", "Home");
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult PostComment(string comment, int postId)
		{
			Post post = Post.GetPost(postId);
			Comment com = new Comment();
			NPDB db = new NPDB ();
			MySqlCommand cmd = db.connection.CreateCommand ();
		
			if (IsMember)
			{
				Account user = Account.GetUser(User.Identity.Name);
				com.id = Comment.Max () + 1;
				com.postId = post.id;
				com.userName = user.displayName ?? user.username;
				com.comment = comment;

				cmd.CommandText = string.Format ("INSERT INTO comments (id, uname, ctime, body, pid) " +
				                                 "VALUES ('{0}','{1}','{2}','{3}','{4}');",
				                                 com.id,
				                                 com.userName,
				                                 _time,
				                                 com.comment,
				                                 com.postId);
				cmd.ExecuteNonQuery ();
				TempData ["CommentSuccess"] = "Comment Posted! :)";
			}
			db.connection.Close ();
			return RedirectToAction ("p", "Home", new { id = post.id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult DeleteComment(int id, int postId)
		{
			Post post = Post.GetPost (postId);
			NPDB db = new NPDB ();
			MySqlCommand cmd = db.connection.CreateCommand ();
			cmd.CommandText = string.Format ("DELETE FROM comments WHERE id={0}", id);
			cmd.ExecuteNonQuery ();
			db.connection.Close ();
			TempData["DeleteMessage"] = "Comment at '("+id+")' deleted.";
			return RedirectToAction ("p", "Home", new {id = postId});
		}
	}
}

