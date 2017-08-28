using System;
using System.Net.Http;
namespace Caeno.Net.Http.Models
{

	/// <summary>
	/// The avaliable HTTP Request Methods.
	/// </summary>
	public enum RequestMethod
	{
		Get,
		Post,
		Put,
		Delete
	}

	public static class RequestMethodExtensions {

		/// <summary>
		/// Get this Request Method as an HttpMethod instance.
		/// </summary>
		/// <returns>The equivalent HttpMethod.</returns>
		public static HttpMethod ToHttpMethod(this RequestMethod method) {
			switch (method) {
				case RequestMethod.Get:
					return HttpMethod.Get;
				case RequestMethod.Post:
					return HttpMethod.Post;
				case RequestMethod.Put:
					return HttpMethod.Put;
				case RequestMethod.Delete:
					return HttpMethod.Delete;
				default:
					throw new ArgumentException();
			}
		}

	}

}

