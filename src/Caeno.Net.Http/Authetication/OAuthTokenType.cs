using System;
namespace Caeno.Net.Http.Authentication
{

	/// <summary>
	/// An enumeration with the OAuth Token Types.
	/// </summary>
	public enum OAuthTokenType
	{
		/// <summary>
		/// Bearer Token.
		/// </summary>
		Bearer
	}

	/// <summary>
	/// Provides extension Methods for the <see cref="OAuthTokenType"/> enumeration.
	/// </summary>
	public static class OAuthTokenTypeExtensions {

		/// <summary>
		/// Gets the description of this instance.
		/// </summary>
		/// <returns>The Token Type Descrition.</returns>
		public static string GetDescription(this OAuthTokenType type) {
			switch (type) {
				case OAuthTokenType.Bearer:
					return "Bearer";
			}

			return null;
		}

	}

}

