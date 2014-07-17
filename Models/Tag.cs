using System;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Netpress.Models
{
	public class Tag
	{
		public int _id;
		public string _tagName;

		public Tag ()
		{

		}

		public Tag(int id, string tagName)
		{
			_id = id;
			_tagName = tagName;
		}

		public static Tag Init(int id, string tagName)
		{
			return new Tag (id, tagName);
		}

		public static int Max()
		{
			NPDB db = new NPDB ();

			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM tags");
			MySqlDataReader reader = fetch.ExecuteReader ();

			int count = 0;

			while (reader.Read()) {
				count++;
			}
			reader.Close ();
			db.connection.Close ();

			count = (count == 0) ? 1 : count;

			return count;
		}

		public static Tag GetTag(string tagName)
		{
			NPDB db = new NPDB ();
			Tag t = new Tag ();
			MySqlCommand com = db.connection.CreateCommand ();
			com.CommandText = string.Format ("SELECT * FROM tags WHERE tag_name={0}", tagName);

			MySqlDataReader reader = com.ExecuteReader ();

			while (reader.Read()) {
				t = Init ((int)reader ["id"], (string)reader ["tagName"]);
			}
			reader.Close ();
			db.connection.Close ();

			return t;
		}

		public static void AddToDb(Tag tag)
		{
			NPDB db = new NPDB ();
			MySqlCommand com = new MySqlCommand ();

			com.CommandText = string.Format("INSERT INTO tags (id, tag_name) VALUES ('{0}', '{1}')",tag._id, tag._tagName);
			com.ExecuteNonQuery ();
			db.connection.Close ();
		}

		public static void AddToDb(int id, string tagName)
		{
			NPDB db = new NPDB ();
			MySqlCommand com = db.connection.CreateCommand ();

			com.CommandText = string.Format("INSERT INTO tags (id, tag_name) VALUES ('{0}', '{1}')",id,tagName);
			com.ExecuteNonQuery ();
			db.connection.Close ();
		}
	}
}

