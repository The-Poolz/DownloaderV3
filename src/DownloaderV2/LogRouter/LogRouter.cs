using DownloaderV2.Helpers;
using DownloaderV2.Decoders;
using DownloaderContext.Models.Types;

namespace DownloaderV2.LogRouter;

public static class LogRouter
{
    private static string _defaultClassLocation = null!;
    private static string _responseTypesClassLocation = null!;

    public static void Initialize(Type contextType)
    {
        var assemblyName = contextType.Assembly.GetName().Name;
        var namespaceName = contextType.Namespace!;
        _defaultClassLocation = $"{namespaceName}.Models.ResponseModels.{{0}}, {assemblyName}";
        _responseTypesClassLocation = $"{namespaceName}.Models.ResponseModels.{{0}}, {assemblyName}";

        Console.WriteLine($"DefaultClassLocation: {_defaultClassLocation}");
        Console.WriteLine($"ResponseTypesClassLocation: {_responseTypesClassLocation}");
    }

    public static readonly IReadOnlyDictionary<ResponseType, PropertySetterBeforeSave> ResponseTypes = new Dictionary<ResponseType, PropertySetterBeforeSave>();

    private static PropertySetterBeforeSave? GetSetterBeforeSave(ResponseType response) =>
        ResponseTypes.ContainsKey(response) ? ResponseTypes[response] : null;

    public static PreSaveActionBinder GetBinder(ResponseType type)
    {
        var classLocation = ResponseTypes.TryGetValue(type, out var propertySetter)
            ? string.Format(_responseTypesClassLocation, propertySetter.DbSetName)
            : string.Format(_defaultClassLocation, type);

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