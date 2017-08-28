using System;
using System.Net.Http;
using System.Collections.Generic;
using Caeno.Net.Http.RequestContents;
using Caeno.Net.Http.Models;

namespace Caeno.Net.Http.ApiRequests
{

	/// <summary>
	/// Represents the required information to compose an API Request.
	/// </summary>
	public interface IApiRequest
	{

		/// <summary>
		/// Gets the Request Method.
		/// </summary>
		RequestMethod Method { get; }

		/// <summary>
		/// Gets the Requet Path.
		/// </summary>
		/// <value>The path.</value>
		string Path { get; }

		/// <summary>
		/// Gets the Request Query String Parameters.
		/// </summary>
		IDictionary<string, object> QueryStringParameters { get; }

		/// <summary>
		/// Gets the Request Content.
		/// </summary>
		RequestContent Content { get; }

		/// <summary>
		/// Gets the Request Headers.
		/// </summary>
		/// <value>The headers.</value>
		IDictionary<string, string> Headers { get; }

	}
}

