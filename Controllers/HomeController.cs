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

			return View (posts);
		}

		public ActionResult p(int id)
		{
			Post post = Post.GetPost (id);

			return View (post);
		}

		public ActionResult edit_post(int? id)
		{
			int _id = (id == null) ? 0 : id.Value;
			string _time = DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss");
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

		/*private void ExecuteCommand(MySqlConnection c)
		{
			//string query = "INSERT INTO posts (id, title, post_time, author, tags, body) VALUES('1', 'Bagro Bang - Two', '"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"', 'DSykex', 'bang bang bruh', 'fdafdfdsffjsdhflkdjshfldjsfhlajhfdlsjfhdalhfsdjhljdafsfsfsfsfsfsfafdsf')";
			MySqlCommand cmd = new MySqlCommand(query, c);
			cmd.ExecuteNonQuery();
		}*/

		private ActionResult UpdatePost()
		{
			return RedirectToAction ("Index", "Home");
		}

		private void SelectCommand(MySqlConnection c)
		{

		}
	}
}

