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
using System.Xml;

namespace Netpress
{
	public class NPDB
	{
		private XmlDocument dbsettings;
		public MySqlConnection connection;
		public string cstr;

		public NPDB ()
		{
			dbsettings = new XmlDocument ();
			dbsettings.Load ("npdb/db.xml");
			string server = dbsettings.SelectSingleNode ("ConnectionInfo/Server").InnerText;
			string database = dbsettings.SelectSingleNode ("ConnectionInfo/Database").InnerText;
			string usr = dbsettings.SelectSingleNode ("ConnectionInfo/uId").InnerText;
			string password = dbsettings.SelectSingleNode ("ConnectionInfo/Password").InnerText;

			cstr = string.Format("server={0};database={1};uid={2};password={3};",server,database,usr,password);
			connection = new MySqlConnection (cstr);

			try
			{
				connection.Open();
				Console.WriteLine("Take off!");
			}
			catch(Exception ex) 
			{
				Console.WriteLine (ex.ToString ());
			}
		}

		public void Init(MySqlConnection c)
		{

		}
	}

	public class NPDBEntity : NPDB
	{
		public NPDBEntity ()
		{

		}

		public List<object> entityProps;
	}
}

