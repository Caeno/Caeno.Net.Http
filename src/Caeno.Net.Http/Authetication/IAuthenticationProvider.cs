using System;
using System.Collections.Generic;

namespace Caeno.Net.Http.Authentication
{

	/// <summary>
	/// An Inteface for an Web API Authentication Provider.
	/// </summary>
	public interface IAuthenticationProvider
	{

		/// <summary>
		/// Gets a Dictionary with the Authentication Headers that will be injected in authenticated requests.
		/// </summary>
		/// <returns>A Dictionary with the Authentication Headers.</returns>
		IDictionary<string, string> GetAuthenticationHeaders();

	}
}

