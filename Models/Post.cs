using System;

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

