using DownloaderV2.Helpers;
using DownloaderV2.Decoders;
using DownloaderContext.Types;

namespace DownloaderV2.LogRouter;

public static class LogRouter
{
    private static string DefaultClassLocation { get; set; } = null!;
    private static string ResponseTypesClassLocation { get; set; } = null!;

    private static readonly Dictionary<ResponseType, PropertySetterBeforeSave> ResponseTypes = new();

    public static void Configure(string defaultClassLocation, string responseTypesClassLocation)
    {
        DefaultClassLocation = defaultClassLocation ?? throw new ArgumentNullException(nameof(defaultClassLocation));
        ResponseTypesClassLocation = responseTypesClassLocation ?? throw new ArgumentNullException(nameof(responseTypesClassLocation));
    }

    public static void AddResponseType(ResponseType responseType, PropertySetterBeforeSave propertySetter)
    {
        ResponseTypes.TryAdd(responseType, propertySetter);
    }

    private static PropertySetterBeforeSave? GetSetterBeforeSave(ResponseType response) => ResponseTypes.ContainsKey(response) ? ResponseTypes[response] : null;

    public static PreSaveActionBinder GetBinder(ResponseType type)
    {
        if (string.IsNullOrEmpty(DefaultClassLocation) || string.IsNullOrEmpty(ResponseTypesClassLocation))
        {
            throw new InvalidOperationException("LogRouter is not configured. Please call LogRouter.Configure with the correct class locations.");
        }

        var classLocation = ResponseTypes.TryGetValue(type, out var propertySetter)
            ? string.Format(ResponseTypesClassLocation, propertySetter.DbSetName)
            : string.Format(DefaultClassLocation, type);

        var responseType = Type.GetType(classLocation) ?? ApplicationLogger.LogAndThrowDynamic(new ArgumentException(string.Format(ExceptionMessages.ClassSpecificationError, classLocation)));

        var genericResponseType = typeof(GenericResponse<>).MakeGenericType(responseType);

        return new PreSaveActionBinder(genericResponseType, GetSetterBeforeSave(type));
    }

    public static ILogResponse GetLogType(ResponseType type, IEnumerable<IReadOnlyDictionary<string, DataDecoder>> listOfData)
    {
        var binder = GetBinder(type);
        return (ILogResponse)Activator.CreateInstance(binder.TypeHolder, listOfData, binder.BeforeSave)!;
    }
}