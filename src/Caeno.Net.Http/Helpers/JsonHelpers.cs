using System;
using Newtonsoft.Json;

namespace Caeno.Net.Http.Helpers
{

	/// <summary>
	/// Helpers for Working with JSON.
	/// </summary>
	public static class JsonHelpers
	{

		/// <summary>
		/// Try to Deserialize a JSON Object optionally returning null if there's an exception.
		/// </summary>
		/// <returns>The deserialized object.</returns>
		/// <param name="jsonObj">A String with the JSON representation of the Object.</param>
		/// <typeparam name="TObj">The type of Object being deserialized parameter.</typeparam>
		public static TObj TryDeserializeObject<TObj>(string jsonObj) 
				where TObj : class {
			TObj obj;
			try {
				obj = JsonConvert.DeserializeObject<TObj>(jsonObj);
			} catch (Exception) {
				return null;
			}

			return obj;
		}

	}
}

