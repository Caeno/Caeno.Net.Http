using System;
using System.Collections.Generic;
using Caeno.Net.Http.Models;
using Caeno.Net.Http.RequestContents;

namespace Caeno.Net.Http.ApiRequests
{

	/// <summary>
	/// An abstract IApiRequest class implementation that allows overriding just the basic request information.
	/// </summary>
	public abstract class BaseApiRequest : IApiRequest
	{

		#region Properties

		public virtual RequestContent Content => null;

		public virtual IDictionary<string, string> Headers => null;

		public abstract RequestMethod Method { get; }

		public abstract string Path { get; }

		public virtual IDictionary<string, object> QueryStringParameters => null;

		#endregion

	}
}
