using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Extensions
{
	public static class HttpClientExtensions
	{
		 public  static async Task<T> ReadContentAs<T>(this HttpResponseMessage Response)
		{
			if (!Response.IsSuccessStatusCode)
			{
				throw new ApplicationException($"Something went wrong calling the API:{Response.ReasonPhrase}");
			}

			string dataAsString = await Response.Content.ReadAsStringAsync().ConfigureAwait(false);
			return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		}
	}
}
