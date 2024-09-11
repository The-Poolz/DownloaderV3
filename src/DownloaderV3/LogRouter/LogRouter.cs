using System.Reflection;
using DownloaderV3.Helpers;
using DownloaderV3.Decoders;
using DownloaderContext.Attributes;

namespace DownloaderV3.LogRouter;

public static class LogRouter
{
    private static Dictionary<string, string> _responseTypes = new();

    public static void Initialize(Type contextType)
    {
        _responseTypes = contextType.Assembly.GetTypes()
            .Where(x => x.GetCustomAttributes<ResponseModelAttribute>().Any())
            .ToDictionary(key => key.Name, value => $"{value.FullName}, {contextType.Assembly.GetName().Name}")!;
    }

    public static PreSaveActionBinder GetBinder(string responseModel)
    {
        var classLocation = _responseTypes.FirstOrDefault(x => x.Key == responseModel).Value
            ?? throw new InvalidOperationException($"Response '{responseModel}' not implement with '{nameof(ResponseModelAttribute)}' attribute.");

        var responseType = Type.GetType(classLocation) ?? ApplicationLogger.LogAndThrowDynamic(new ArgumentException(string.Format(ExceptionMessages.ClassSpecificationError, classLocation)));

        var genericResponseType = typeof(GenericResponse<>).MakeGenericType(responseType);

        return new PreSaveActionBinder(genericResponseType, null);
    }

    public static ILogResponse GetLogType(string responseModel, IEnumerable<IReadOnlyDictionary<string, DataDecoder>> listOfData)
    {
        var binder = GetBinder(responseModel);
        return (ILogResponse)Activator.CreateInstance(binder.TypeHolder, listOfData, binder.BeforeSave)!;
    }
}