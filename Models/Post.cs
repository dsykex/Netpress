using System;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Netpress.Models
{
	public class Post
	{
		public Post (int _id, string _title, string _tags, string _author, DateTime _time, string _body)
		{
			id = _id;
			title = _title;
			tags = _tags;
			author = _author;
			time = _time;
			body = _body;
		}

		public static Post GetPost(int id)
		{
			NPDB db = new NPDB ();
			Post p = new Post ();
			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM posts WHERE id={0}",id);

			MySqlDataReader reader = fetch.ExecuteReader ();

			while (reader.Read()) {
				p = Post.Init (
					(int)reader ["id"],
					(string)reader ["title"],
					(string)reader ["tags"],
					(string)reader ["author"],
					(DateTime)reader ["post_time"],
					(string)reader ["body"]);
			}
			reader.Close ();
			db.connection.Close ();

			return p;
		}

		public static Post Init(int id, string title, string tags, string author, DateTime time, string body)
		{
			return new Post (id, title, tags, author, time, body);
		}

		public Post()
		{

		}

		public int id;
		public string title;
		public string tags;
		public DateTime time;
		public string author;
		public string body;
	}
}

