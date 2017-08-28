using System;
using System.Net.Http;
using System.Text;
using Caeno.Net.Http.Models;
using Newtonsoft.Json;

namespace Caeno.Net.Http.RequestContents
{
	/// <summary>
	/// Represents a JSON Request Content.
	/// </summary>
	public class JsonRequestContent : RequestContent
	{

		/// <summary>
		/// Gets or sets the object that represents the request content.
		/// </summary>
		/// <value>The content of the post.</value>
		public object postContent { get; set; }

		public JsonRequestContent(object content) {
			postContent = content;
		}

		internal override HttpContent AsHttpContent() {
			var json = JsonConvert.SerializeObject(postContent);
			return new StringContent(json, Encoding.UTF8, "application/json");
		}

	}
}

