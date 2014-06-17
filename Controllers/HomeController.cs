using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MySql.Data.MySqlClient;
using MySql.Data;
using MySql.Data.Types;
using System.Data.Common;
using System.Configuration;


namespace Netpress.Controllers
{
	public class HomeController : Controller
	{
<<<<<<< HEAD
		private MySqlConnection con;
		private string server;
		private string database;
		private string uid;
		private string password;

		public int postsPerPage;

=======
>>>>>>> parent of 19e55e1... Added a new variable
		public ActionResult Index ()
		{
			server = "localhost";
			database = "npdb";
			uid = "root";
			password = "ascend1@1#_";
			string connectionString;
			connectionString = "SERVER=" + server + ";" + "DATABASE=" + 
				database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + "; Convert Zero Datetime=True; Allow Zero Datetime=true";



			ViewData ["Message"] = "Welcome to ASP.NET MVC on Mono!";
			Initialize ();

			string[] titles = new string[10];

			MySqlConnection con = new MySqlConnection (connectionString);

			try{
				con.Open();

				MySqlCommand com = con.CreateCommand();
				com.CommandText = "SELECT * FROM posts";

				MySqlDataReader reader = com.ExecuteReader();

				while(reader.Read())
				{
					DateTime dTime = DateTime.Parse(reader["post_time"].ToString());
					string sTime = dTime.ToString("MMM");
					titles[0] = sTime;
				}
				reader.Close();
				con.Close();
			}
			catch(Exception ex) {
				Console.WriteLine (ex.ToString ());
			}


			ViewBag.Time = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			return View ();
		}

		private void Initialize()
		{

		}

		private void ExecuteCommand(MySqlConnection c)
		{
			string query = "INSERT INTO posts (id, title, post_time, author, tags, body) VALUES('2', 'Bagro Bang', '"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"', 'DSykex', 'bang bang bruh', 'fdafdfdsfdafsfsfsfsfsfsfafdsf')";

			MySqlCommand cmd = new MySqlCommand(query, c);

			cmd.ExecuteNonQuery();
	
		}

		private void SelectCommand(MySqlConnection c)
		{

		}
	}
}

