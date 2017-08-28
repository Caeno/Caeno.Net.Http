using System;
using System.Collections.Generic;

namespace Caeno.Net.Http.Authentication
{
	
	public class OAuthAuthenticationProvider : IAuthenticationProvider
	{

		public OAuthTokenType TokenType { get; set; } = OAuthTokenType.Bearer;

		public string AuthToken { get; set; }
		
		public OAuthAuthenticationProvider(string authToken) {
			AuthToken = authToken;
		}

		public IDictionary<string, string> GetAuthenticationHeaders() {
			return new Dictionary<string, string> {
				["Authorization"] = $"{TokenType.GetDescription()} {AuthToken}"
			};
		}

	}

}

