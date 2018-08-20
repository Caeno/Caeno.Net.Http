using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caeno.Net.Http.ApiRequests;
using Caeno.Net.Http.Authentication;
using Caeno.Net.Http.Models;

namespace Caeno.Net.Http.Abstractions
{

    /// <summary>
    /// Represents an Interface for a implementation of Web API service.
    /// </summary>
    public interface IHttpHelper
	{

		#region Properties

		/// <summary>
		/// Gets or sets the base URI of the API.
		/// </summary>
		string BaseUri { get; set; }

		/// <summary>
		/// Gets or sets the Request processing timeout.
		/// </summary>
		/// <value>The timeout.</value>
		TimeSpan Timeout { get; set; }

		/// <summary>
		/// Gets or sets the authentication provider used by authenticated requests.
		/// </summary>
		IAuthenticationProvider AuthenticationProvider { get; set; }

		#endregion

		/// <summary>
		/// Process the specified API Request.
		/// </summary>
		/// <param name="request">An API Request object.</param>
		/// <param name="isAuthenticated">A flag indicating if the Request should be made using the Authentication provider.</param>
        /// <param name="isAuthenticationOptional">A flag indicating if an Authenticated Request should be optional.</param>
		/// <typeparam name="TResult">The type of the result object.</typeparam>
		/// <returns>An WebApiResponse object with the results of the call.</returns>
		Task<WebApiResponse<TResult>> RequestAsync<TResult>(IApiRequest request, bool isAuthenticated = false, bool isAuthenticationOptional = false);

		/// <summary>
		/// Process the specified download API Request.
		/// </summary>
		/// <param name="request">An API Request object.</param>
		/// <param name="isAuthenticated">A flag indicating if the Request should be made using the Authentication provider.</param>
		Task<WebApiResponse<byte[]>> DownloadAsync(IApiRequest request, bool isAuthenticated = false);

		#region Regular Request Methods

		/// <summary>
		/// Process a GET request against the specified action.
		/// </summary>
		/// <param name="action">The Action Name.</param>
		/// <param name="isAuthenticated">A flag indicating if the Request should be made using the Authentication provider.</param>
		/// <typeparam name="TResult">The type of the return object.</typeparam>
		/// <returns>An WebApiResponse object with the results of the call.</returns>
		Task<WebApiResponse<TResult>> GetAsync<TResult>(string action, bool isAuthenticated = false);

		/// <summary>
		/// Process a POST request against the specified action using an JSON object body.
		/// </summary>
		/// <param name="action">The Action Name.</param>
		/// <param name="postData">Post data.</param>
		/// <param name="isAuthenticated">A flag indicating if the Request should be made using the Authentication provider.</param>
		/// <typeparam name="TResult">The type of the return object.</typeparam>
		/// <returns>An WebApiResponse object with the results of the call.</returns>
		Task<WebApiResponse<TResult>> PostJsonAsync<TResult>(string action, object postData, bool isAuthenticated = false);

		/// <summary>
		/// Process a POST request against the specified action using a Form body.
		/// </summary>
		/// <param name="action">The Action Name.</param>
		/// <param name="values">A dictionary with the Form parameters to compose the request body.</param>
		/// <param name="isAuthenticated">A flag indicating if the Request should be made using the Authentication provider.</param>
		/// <typeparam name="TResult">The type of the result object.</typeparam>
		/// <returns>An WebApiResponse object with the results of the call.</returns>
		Task<WebApiResponse<TResult>> PostFormAsync<TResult>(string action, IDictionary<string, string> values, bool isAuthenticated = false);

		/// <summary>
		/// Process a PUT request against the specified action.
		/// </summary>
		/// <param name="action">The Action Name.</param>
		/// <param name="postData">Post data.</param>
		/// <param name="isAuthenticated">A flag indicating if the Request should be made using the Authentication provider.</param>
		/// <typeparam name="TResult">The type of the return object.</typeparam>
		/// <returns>An WebApiResponse object with the results of the call.</returns>
		Task<WebApiResponse<TResult>> PutJsonAsync<TResult>(string action, object postData, bool isAuthenticated = false);

		#endregion

	}

}

