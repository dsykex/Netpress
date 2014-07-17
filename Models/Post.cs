using System;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Netpress.Models
{
	public class Post
	{
		public Post (int _id, string _title, string _tags, string _thumbnail, string _author, DateTime _time, string _body)
		{
			id = _id;
			title = _title;
			tags = _tags;
			thumbnail = _thumbnail;
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

			if (id != 0) {
				MySqlDataReader reader = fetch.ExecuteReader ();

				while (reader.Read()) {
					p = Post.Init (
						(int)reader ["id"],
						(string)reader ["title"],
						(string)reader ["tags"],
						(string)reader ["thumbnail"],
						(string)reader ["author"],
						(DateTime)reader ["post_time"],
						(string)reader ["body"]);
				}
				reader.Close ();
				db.connection.Close ();
			} else {
				p = null;
			}
			return (p == null) ? null : p;
		}

		public static int Max()
		{
			NPDB db = new NPDB ();

			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM posts");
			MySqlDataReader reader = fetch.ExecuteReader ();

			int count = 0;

			while (reader.Read()) {
				count++;
			}
			reader.Close ();
			db.connection.Close ();

			count = (count == 0) ? 0 : count;

			return count;
		}

		public static bool isNull(Post p)
		{
			return (p == null) ? true : false;
		}

		public static Post Init(int id, string title, string tags, string thumbnail, string author, DateTime time, string body)
		{
			return new Post (id, title, tags, thumbnail, author, time, body);
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
		public string thumbnail;
	}
}

