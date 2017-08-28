using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Caeno.Net.Http.Abstractions;
using Caeno.Net.Http.ApiRequests;
using Caeno.Net.Http.Authentication;
using Caeno.Net.Http.Models;
using Newtonsoft.Json;

namespace Caeno.Net.Http.Services
{

    /// <summary>
    /// Basic implementation of the IWebApiService interface allowing Get and Post calls to HTTP Endpoints.
    /// </summary>
    public class WebApiService : IWebApiService
	{
		static readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(15);
		static readonly HttpMethod[] SUPPORTED_METHODS = {
			HttpMethod.Get, HttpMethod.Post, HttpMethod.Put, HttpMethod.Delete
		};

		HttpClient _client;
		HttpClient _authenticatedClient;

		public WebApiService(string baseUri) {
			BaseUri = baseUri;
            _client = new HttpClient {
                Timeout = DEFAULT_TIMEOUT
            };
        }


		#region Properties

        /// <summary>
        /// Gets or sets the logger for this instance.
        /// </summary>
        public ILogger Logger { get; set; }

		/// <summary>
		/// Gets or sets the base URI to be used on Endpoint resolution.
		/// </summary>
		public string BaseUri { get; set; }

		/// <summary>
		/// Gets or sets the Request Timeout.
		/// </summary>
		public TimeSpan Timeout { 
			get { return _client.Timeout; }
			set { 
				_client.Timeout = value;
				_authenticatedClient.Timeout = value;
			}
		}

		private IAuthenticationProvider _authenticationProvider;
		public IAuthenticationProvider AuthenticationProvider { 
			get { return _authenticationProvider; }
			set {
				_authenticationProvider = value;
				if (_authenticationProvider != null) {
					_authenticatedClient = new HttpClient();
					_authenticatedClient.Timeout = Timeout;
					foreach (var entry in _authenticationProvider.GetAuthenticationHeaders()) {
						_authenticatedClient.DefaultRequestHeaders.Add(entry.Key, entry.Value);
					}
				} else {
					_authenticatedClient = null;
				}
			}
		}

		#endregion

		Uri BuildUri(string path, IDictionary<string, object> queryStringParams) {
            var builder = new UriBuilder(BaseUri);
            builder.Path = path;

			var query = new StringBuilder();
			if (queryStringParams != null) {
				bool isFirst = true;
				foreach (var param in queryStringParams) {
                    query.Append(isFirst ? string.Empty : "&");
                    query.Append($"{param.Key}={param.Value}");

					isFirst = false;
				}
			}

            builder.Query = query.ToString();
            return builder.Uri;
		}

		public async Task<WebApiResponse<TResult>> RequestAsync<TResult>(IApiRequest apiRequest, bool isAuthenticated = false) {
			HttpResponseMessage response = null;

			// Configure the request
			var requestUri = BuildUri(apiRequest.Path, apiRequest.QueryStringParameters);
            Logger?.Trace("{0}", requestUri);

			var request = new HttpRequestMessage {
				RequestUri = requestUri,
				Method = apiRequest.Method.ToHttpMethod()
			};

			// Add Headers
			if (apiRequest.Headers != null)
				foreach (var entry in apiRequest.Headers)
					request.Headers.Add(entry.Key, entry.Value);

			// Add Content if it's a post or put method call
			if ((apiRequest.Method == RequestMethod.Post || apiRequest.Method == RequestMethod.Put) &&
				 apiRequest.Content != null) {
				request.Content = apiRequest.Content.AsHttpContent();
			}
			
			// Finaly Add authentication
			if (isAuthenticated) {
				if (AuthenticationProvider == null)
					throw new ArgumentNullException(nameof(AuthenticationProvider), "An authentication provider must be provided for authenticated requests.");

				foreach (var entry in AuthenticationProvider.GetAuthenticationHeaders()) {
					request.Headers.Add(entry.Key, entry.Value);
				}
			}

			// Handles possible exceptions
			try {
				response = await _client.SendAsync(request);
			} catch (TaskCanceledException ex) {
                Logger?.Trace("Error trying execute Request: {0}", ex.Message);
				return new WebApiResponse<TResult>(ex.Message, -1, WebApiResponseErrorType.Timeout);
			} catch (Exception ex) {
                Logger?.Trace("Error trying execute Request: {0}", ex.Message);
				return new WebApiResponse<TResult>(ex.Message, -1);
			}

			return await ProccessResult<TResult>(response);
		}

		public async Task<WebApiResponse<byte[]>> DownloadAsync(IApiRequest apiRequest, bool isAuthenticated = false) {
			HttpResponseMessage response = null;

			// Configure the request
			var requestUri = BuildUri(apiRequest.Path, apiRequest.QueryStringParameters);
			var request = new HttpRequestMessage {
				RequestUri = requestUri,
				Method = apiRequest.Method.ToHttpMethod()
			};

			// Add Content if it's a post or put method call
			if (apiRequest.Method == RequestMethod.Post || apiRequest.Method == RequestMethod.Put)
				request.Content = apiRequest.Content.AsHttpContent();

			// Finaly Add authentication
			if (isAuthenticated) {
				if (AuthenticationProvider == null)
					throw new ArgumentNullException(nameof(AuthenticationProvider), "An authentication provider must be provided for authenticated requests.");

				foreach (var entry in AuthenticationProvider.GetAuthenticationHeaders()) {
					request.Headers.Add(entry.Key, entry.Value);
				}
			}

			// Handles possible exceptions
			try {
				response = await _client.SendAsync(request);
			} catch (TaskCanceledException ex) {
                Logger?.Trace("Error trying execute Request: {0}", ex.Message);
				return new WebApiResponse<byte[]>(ex.Message, -1, WebApiResponseErrorType.Timeout);
			} catch (Exception ex) {
                Logger?.Trace("Error trying execute Request: {0}", ex.Message);
				return new WebApiResponse<byte[]>(ex.Message, -1);
			}

			if (response.IsSuccessStatusCode) {
				var fileBytes = await response.Content.ReadAsByteArrayAsync();
				return new WebApiResponse<byte[]>(fileBytes, (int)response.StatusCode);
			}

			var responseStr = await response.Content.ReadAsByteArrayAsync();
			return new WebApiResponse<byte[]>(responseStr, (int)response.StatusCode);
		}


		#region Public API for Regular Request Methods

		/// <summary>
		/// Performs an asynchronous HTTP GET Request against the specified action and deserializes it's JSON result to an object of the specified <typeparamref name="TResult"/> parameter.
		/// </summary>
		/// <returns>A WebApiResponse object with the results of the Request Call.</returns>
		/// <param name="action">
		/// The URL Action method to be perform the Request. It'll be used in conjunction with the BaseUri defined for this instance.
		/// </param>
		/// <typeparam name="TResult">
		/// The Type of the object being returned.
		/// This type is deserialized from the JSON result of the request.
		/// </typeparam>
		public async Task<WebApiResponse<TResult>> GetAsync<TResult>(string action, bool isAuthenticated = false) {
			HttpResponseMessage response;
			var requestUri = $"{BaseUri}/{action}";

			// Handles possible exceptions
			try {
				response = await _client.GetAsync(requestUri);
			} catch (TaskCanceledException ex) {
				return new WebApiResponse<TResult>(ex.Message, -1, WebApiResponseErrorType.Timeout);
			} catch (Exception ex) {
				return new WebApiResponse<TResult>(ex.Message, -1);
			}

			// Handle the Response
			return await ProccessResult<TResult>(response);
		}

		/// <summary>
		/// Performs an asynchronous HTTP POST Request against the specified action and deserializes it's JSON result to an object of the specified <typeparamref name="TResult"/> parameter.
		/// </summary>
		/// <returns>A WebApiResponse object with the results of the Request Call.</returns>
		/// <param name="action">
		/// The URL Action method to be perform the Request. It'll be used in conjunction with the BaseUri defined for this instance.
		/// </param>
		/// <param name="postData">
		/// The object that will be serialized as JSON in the POST Content.
		/// </param>
		/// <typeparam name="TResult">
		/// The Type of the object being returned.
		/// This type is deserialized from the JSON result of the request.
		/// </typeparam>
		public async Task<WebApiResponse<TResult>> PostJsonAsync<TResult>(string action, object postData, bool isAuthenticated = false) {
			HttpResponseMessage response;
			var requestUri = $"{BaseUri}/{action}";

			// Prepares the POST Content
			var json = JsonConvert.SerializeObject(postData);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			// Handles possible exceptions
			try {
				response = await _client.PostAsync(requestUri, content);
			} catch (TaskCanceledException ex) {
				return new WebApiResponse<TResult>(ex.Message, -1, WebApiResponseErrorType.Timeout);
			} catch (Exception ex) {
				return new WebApiResponse<TResult>(ex.Message, -1);
			}

			// Handles the Response
			return await ProccessResult<TResult>(response);
		}

		public async Task<WebApiResponse<TResult>> PostFormAsync<TResult>(string action, IDictionary<string, string> values, bool isAuthenticated = false) {
			HttpResponseMessage response;
			var requestUri = $"{BaseUri}/{action}";

			// Prepares the Form Content
			var formEntries = values.Select(v => new KeyValuePair<string, string>(v.Key, v.Value));
			var formContent = new FormUrlEncodedContent(formEntries);

			// Handles possible exceptions
			try {
				response = await _client.PostAsync(requestUri, formContent);
			} catch (TaskCanceledException ex) {
				return new WebApiResponse<TResult>(ex.Message, -1, WebApiResponseErrorType.Timeout);
			} catch (Exception ex) {
				return new WebApiResponse<TResult>(ex.Message, -1);
			}

			// Handles the Response
			return await ProccessResult<TResult>(response);
		}

		/// <summary>
		/// Performs an asynchronous HTTP PUT Request against the specified action and deserializes it's JSON result to an object of the specified <typeparamref name="TResult"/> parameter.
		/// </summary>
		/// <returns>A WebApiResponse object with the results of the Request Call.</returns>
		/// <param name="action">
		/// The URL Action method to be perform the Request. It'll be used in conjunction with the BaseUri defined for this instance.
		/// </param>
		/// <param name="postData">
		/// The object that will be serialized as JSON in the POST Content.
		/// </param>
		/// <typeparam name="TResult">
		/// The Type of the object being returned.
		/// This type is deserialized from the JSON result of the request.
		/// </typeparam>
		public async Task<WebApiResponse<TResult>> PutJsonAsync<TResult>(string action, object postData, bool isAuthenticated = false) {
			HttpResponseMessage response;
			var requestUri = $"{BaseUri}/{action}";

			// Prepares the POST Content
			var json = JsonConvert.SerializeObject(postData);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			// Handles possible exceptions
			try {
				response = await _client.PutAsync(requestUri, content);
			} catch (TaskCanceledException ex) {
				return new WebApiResponse<TResult>(ex.Message, -1, WebApiResponseErrorType.Timeout);
			} catch (Exception ex) {
				return new WebApiResponse<TResult>(ex.Message, -1);
			}

			// Handles the Response
			return await ProccessResult<TResult>(response);
		}

        #endregion


        #region Protected API

		protected async Task<WebApiResponse<TResult>> ProccessResult<TResult>(HttpResponseMessage response) {
			if (response.IsSuccessStatusCode) {
				var responseJson = await response.Content.ReadAsStringAsync();
				var responseData = JsonConvert.DeserializeObject<TResult>(responseJson);
                Logger?.Trace("Response: {0}", responseJson);

				return new WebApiResponse<TResult>(responseData, (int)response.StatusCode);
			}

			var responseStr = await response.Content.ReadAsStringAsync();
            Logger?.Trace("Response: {0}", responseStr);

			return new WebApiResponse<TResult>(responseStr, (int)response.StatusCode);
		}

		#endregion

	}
}

