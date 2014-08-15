using System;
using System.Collections.Generic;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Netpress.Models
{
	public class Account
	{

		public int id;
		public string username;
		public string password;
		public string displayName;
		public string email;
		public string rank;
		public string profilePic;

		public Account ()
		{

		}

		public Account(int _id, string _username, string _password, string _displayName, string _email, string _rank, string _profilePic)
		{
			id = _id;
			username = _username;
			password = _password;
			displayName = _displayName;
			email = _email;
			rank = _rank;
			profilePic = _profilePic;
		}

		public static Account Init(int _id, string _username, string _password, string _displayName, string _email, string _rank, string _profilePic)
		{
			return new Account (_id, _username, _password, _displayName, _email, _rank, _profilePic);
		}

		public static int Max()
		{
			NPDB db = new NPDB ();

			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM accounts");
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

		public static Account GetUser(int id)
		{
			NPDB db = new NPDB ();
			Account user = new Account ();
			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM accounts WHERE id={0}",id);

			if (id != 0) {
				MySqlDataReader reader = fetch.ExecuteReader ();

				while (reader.Read()) {
					user = Account.Init(
						(int)reader["id"],
						(string)reader["username"],
						(string)reader["pword"],
						(string)reader["dname"],
						(string)reader["email"],
						(string)reader["rank"],
						(string)reader["profilepic"]
						);
				}
				reader.Close ();
				db.connection.Close ();
			} else {
				user = null;
			}
			return (user == null) ? null : user;
		}
		public static Account GetUser(string username)
		{
			NPDB db = new NPDB ();
			Account user = new Account ();
			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM accounts WHERE username='{0}'",username);

			if (!string.IsNullOrEmpty(username)) {
				MySqlDataReader reader = fetch.ExecuteReader ();

				while (reader.Read()) {
					user = Account.Init(
						(int)reader["id"],
						(string)reader["username"],
						(string)reader["pword"],
						(string)reader["dname"],
						(string)reader["email"],
						(string)reader["rank"],
						(string)reader["profilepic"]
						);
				}
				reader.Close ();
				db.connection.Close ();
			} else {
				user = null;
			}
			return (user == null) ? null : user;
		}
		public static Account GetUser(string username,string password)
		{
			NPDB db = new NPDB ();
			Account user = new Account ();
			MySqlCommand fetch = db.connection.CreateCommand ();
			fetch.CommandText = string.Format("SELECT * FROM accounts WHERE username='{0}' AND pword='{1}'",username,password);

			if (!string.IsNullOrEmpty(username)) {
				MySqlDataReader reader = fetch.ExecuteReader ();

				while (reader.Read()) {
					user = Account.Init(
						(int)reader["id"],
						(string)reader["username"],
						(string)reader["pword"],
						(string)reader["dname"],
						(string)reader["email"],
						(string)reader["rank"],
						(string)reader["profilepic"]
						);
				}
				reader.Close ();
				db.connection.Close ();
			} else {
				user = null;
			}
			return (string.IsNullOrEmpty(user.username)) ? null : user;
		}
	}
}

