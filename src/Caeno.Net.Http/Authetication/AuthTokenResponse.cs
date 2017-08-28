using System;
using Newtonsoft.Json;

namespace Caeno.Net.Http.Authentication
{

	/// <summary>
	/// Represents the Response of an Authentication Token Request .
	/// </summary>
	public class AuthTokenResponse
	{

		/// <summary>
		/// Gets or sets the type of the token.
		/// </summary>
		[JsonProperty("token_type")]
		public string TokenType { get; set; }

		/// <summary>
		/// Gets or sets the access token.
		/// </summary>
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		/// <summary>
		/// Gets or sets the expiration time in seconds.
		/// </summary>
		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }

		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		[JsonProperty("error")]
		public string Error { get; set; }

		/// <summary>
		/// Gets or sets the error description.
		/// </summary>
		[JsonProperty("error_description")]
		public string ErrorDescription { get; set; }

		/// <summary>
		/// Gets a flag indicating if this Response has been processed succesfully.
		/// </summary>
		public bool IsSuccess {
			get {
				return string.IsNullOrWhiteSpace(Error) &&
					  string.IsNullOrWhiteSpace(ErrorDescription);
			}
		}

	}

}

