using Newtonsoft.Json;
using RuleEngine.Domain.Ext.RemoteFieldExpr;

namespace RuleEngine.Infrastructure.Ext.RemoteFieldExpr;

public class ApiFieldFetcher : IFieldFetcher
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public ApiFieldFetcher(HttpClient httpClient, string apiUrl)
    {
        _httpClient = httpClient;
        _apiUrl = apiUrl;
    }

    public async Task<object> FetchFieldValueAsync(string fieldName)
    {
        var response = await _httpClient.GetStringAsync($"{_apiUrl}/{fieldName}");

        if (response != null)
        {
            // Assuming the API returns JSON and the value is in the "value" field.
            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            if (jsonResponse != null && jsonResponse.TryGetValue("value", out var fieldValue))
            {
                return fieldValue;
            }
        }

        throw new Exception($"Unable to fetch value for field {fieldName} from API.");
    }
}