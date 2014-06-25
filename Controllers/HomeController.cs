using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MySql.Data.MySqlClient;
using MySql.Data;
using MySql.Data.Entity;
using System.Data;
using MySql.Data.Types;
using System.Data.Common;
using System.Configuration;
using Netpress.Models;

namespace Netpress.Controllers
{
	public class HomeController : Controller
	{
		private MySqlConnection con;
		private string cString;

		public NPDB db;
		public int postsPerPage;
		public NPDBEntity entity;
		public Netpress.Models.Post[] posts;

		public ActionResult Index (/*int id*/)
		{
			db = new NPDB ();
			posts = new Netpress.Models.Post[3];
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
				p.body = (string)reader ["body"];
				p.time = (DateTime)reader ["post_time"];
				posts [i] = p;

				//TempData ["posts"] = posts;
			}

			reader.Close ();
			//ExecuteCommand (db.connection);
			db.connection.Close ();

			return View (posts);
		}

		private void Initialize()
		{

		}

		private void ExecuteCommand(MySqlConnection c)
		{
			string query = "INSERT INTO posts (id, title, post_time, author, tags, body) VALUES('1', 'Bagro Bang - Two', '"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"', 'DSykex', 'bang bang bruh', 'fdafdfdsffjsdhflkdjshfldjsfhlajhfdlsjfhdalhfsdjhljdafsfsfsfsfsfsfafdsf')";
			MySqlCommand cmd = new MySqlCommand(query, c);
			cmd.ExecuteNonQuery();
		}

		private void SelectCommand(MySqlConnection c)
		{

		}
	}
}

