using System;
using System.Net.Http;

namespace Caeno.Net.Http.RequestContents
{

	/// <summary>
	/// Provides a base class for Content to be posted on a Request message.
	/// </summary>
	public abstract class RequestContent
	{

		/// <summary>
		/// Provides this Content as an HttpContent object.
		/// </summary>
		/// <returns>The http content.</returns>
		abstract internal HttpContent AsHttpContent();

	}
}

