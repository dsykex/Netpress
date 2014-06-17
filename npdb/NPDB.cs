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
		private MySqlConnection connection;

		public NPDB ()
		{

		}

		public void Init(MySqlConnection c)
		{
			dbsettings = new XmlDocument ();
			dbsettings.Load ("~/npdb/db.xml");
			string server = dbsettings.SelectSingleNode ("Server").InnerText;
			string database = dbsettings.SelectSingleNode ("Database").InnerText;
			string usr = dbsettings.SelectSingleNode ("UserId").InnerText;
			string password = dbsettings.SelectSingleNode ("Password").InnerText;
		
			string cString = string.Format("server={0};");
			connection = new MySqlConnection (cString);

			try
			{
				connection.Open();
			}
			catch(Exception ex) 
			{
				Console.WriteLine (ex.ToString ());
			}
		}
	}
}

