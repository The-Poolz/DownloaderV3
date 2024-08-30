using Newtonsoft.Json.Linq;

namespace DownloaderV2.Tests.Results.CovalentResults;

public class BaseInputData<T>
{
    public BaseInputData(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("JSON can't be null or whitespace.", nameof(json));
        }

        Json = JObject.Parse(json);
        ResponseData = Json.ToObject<T>() ?? throw new NullReferenceException("Can`t deserialize object");
    }

    public JObject Json { get; }
    public T ResponseData { get; }
}