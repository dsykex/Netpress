using System;
using System.Collections.Generic;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Netpress.Models
{
	public class Comment
	{
		public int id;
		public string userName;
		public string comment;
		public int postId;
		public DateTime time;

		public Comment ()
		{

		}

		public Comment(int _id, string _userName, DateTime _time, string _comment, int _postId)
		{
			id = _id;
			comment = _comment;
			userName = _userName;
			postId = _postId;
			time = _time;
		}

		public static Comment Init(int _id, string _userName, DateTime _time, string _comment, int _postId)
		{
			return new Comment( _id, _userName, _time, _comment, _postId);
		}

		public static Comment GetComment(int id)
		{
			NPDB db = new NPDB ();
			Comment user = new Comment ();
			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM comments WHERE id={0}",id);

			if (id != 0) {
				MySqlDataReader reader = fetch.ExecuteReader ();

				while (reader.Read()) {
					user = Comment.Init(
						(int)reader["id"],
						(string)reader["uname"],
						(DateTime)reader["ctime"],
						(string)reader["body"],
						(int)reader["pid"]);
				}
				reader.Close ();
				db.connection.Close ();
			} else {
				user = null;
			}
			return (user == null) ? null : user;
		}

		public static int Max()
		{
			NPDB db = new NPDB ();

			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM comments");
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
	}
}

