using System;
using System.Collections.Generic;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Netpress.Models
{
	public class Post
	{
		public int id;
		public string title;
		public string tags;
		public DateTime time;
		public string author;
		public string body;
		public string thumbnail;

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

		public static Post[] GetRelPosts(int id)
		{
			Post p = GetPost (id);
			NPDB db = new NPDB ();
			Post[] _posts;
			List<Post> relPosts = new List<Post> ();
			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM posts");

			int i = 0;
			int _i = 0;
			int length = 0;

			MySqlDataReader reader = fetch.ExecuteReader ();

			while (reader.Read()) {
				i++;
			}


			_posts = new Post[i];
			reader.Close ();
			if (id != 0) {

				reader = fetch.ExecuteReader ();
				while (reader.Read()) {

					Models.Post _post = new Netpress.Models.Post();
					_post.id = (int)reader ["id"];
					_post.title = (string)reader ["title"];
					_post.author = (string)reader ["author"];
					_post.tags = (string)reader ["tags"];
					_post.thumbnail = (string)reader ["thumbnail"];
					_post.body = (string)reader ["body"];
					_post.time = (DateTime)reader ["post_time"];
					_posts [_i] = _post;
					_i++;
				}
				_i = 0;
				foreach (Post _p in _posts) {
					if (!string.IsNullOrEmpty (_p.tags)) {

						string[] pTags = _p.tags.Split (' ');

						foreach (string t in pTags) {
							if (p.tags.Contains (t)) {
								//relPosts [_i] = _p;
								relPosts.Add (_p);
								_i++;
								break;
							}
						}
					}

				}

				reader.Close ();
				db.connection.Close ();
			} 
			Post[] _relPosts = relPosts.ToArray();
			return (_relPosts != null) ? _relPosts : null; 
		}

		public static Comment[] GetComments(int id)
		{
			Post post = Post.GetPost (id);
			List<Comment> comments = new List<Comment> ();
			NPDB db = new NPDB ();
			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format ("SELECT * FROM comments WHERE pid={0}", post.id);
			MySqlDataReader reader = fetch.ExecuteReader ();
			int counter = 0;
			while (reader.Read()) {
				comments.Add(Comment.Init(
					(int)reader["id"],
					(string)reader["uname"],
					(DateTime)reader["ctime"],
					(string)reader["body"],
					(int)reader["pid"]));
			}
			reader.Close ();
			db.connection.Close ();

			return (comments.ToArray ().Length > 0) ? comments.ToArray() : null;
		}
	}
}

