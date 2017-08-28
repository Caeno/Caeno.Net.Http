using System;

namespace Caeno.Net.Http.Models
{

	/// <summary>
	/// An object that represents the response of an Web API Request.
	/// </summary>
	public class WebApiResponse<TResult>
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ClickSmile.Patient.Services.Models.WebApiResponse`1"/> class for a succesful request.
		/// </summary>
		/// <param name="results">The response Results object.</param>
		/// <param name="statusCode">Status code of the Response.</param>
		public WebApiResponse(TResult results, int statusCode = 200) {
			IsSuccess = true;
			StatusCode = statusCode;
			Results = results;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ClickSmile.Patient.Services.Models.WebApiResponse`1"/> class for a failed request.
		/// </summary>
		/// <param name="errorMessage">The response error message.</param>
		/// <param name="statusCode">The response Status code.</param>
		/// <param name="errorType">The type of error that has ocurred.</param>
		public WebApiResponse(string errorMessage, int statusCode, WebApiResponseErrorType errorType = WebApiResponseErrorType.General) {
			IsSuccess = false;
			ErrorMessage = errorMessage;
			StatusCode = statusCode;
			ErrorType = errorType;
		}

		/// <summary>
		/// Gets a flag indicating if the process has been processed succesfully.
		/// </summary>
		public bool IsSuccess { get; private set; }

		/// <summary>
		/// Gets the Type of Error on case of failure response.
		/// </summary>
		public WebApiResponseErrorType? ErrorType { get; private set; }

		/// <summary>
		/// Gets the error message for responses ended as errors.
		/// </summary>
		public string ErrorMessage { get; private set; }

		/// <summary>
		/// Gets the response Status code.
		/// </summary>
		public int StatusCode { get; private set; }

		/// <summary>
		/// If the response has no errors represents the Resulting response data.
		/// </summary>
		/// <value>The results.</value>
		public TResult Results { get; private set; }

	}

}