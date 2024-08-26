using DownloaderV2.Helpers;
using DownloaderV2.Decoders;
using DownloaderContext.Types;

namespace DownloaderV2.LogRouter;

public static class LogRouter
{
    private const string DefaultClassLocation = "CovalentDB.Models.ResponseModels.{0}, CovalentDB";
    private const string ResponseTypesClassLocation = "CovalentDB.Models.ResponseModels.{0}, CovalentDB";

    public static readonly IReadOnlyDictionary<ResponseType, PropertySetterBeforeSave> ResponseTypes = new Dictionary<ResponseType, PropertySetterBeforeSave>
    {
        { ResponseType.SignUpEventPoolActivated, new PropertySetterBeforeSave("EventName", "NewPoolActivated", "SignUpEvent") },
        { ResponseType.SignUpEventPoolDeactivated, new PropertySetterBeforeSave("EventName", "PoolDeactivated", "SignUpEvent") },
    };

    private static PropertySetterBeforeSave? GetSetterBeforeSave(ResponseType response) =>
        ResponseTypes.ContainsKey(response) ? ResponseTypes[response] : null;

    public static PreSaveActionBinder GetBinder(ResponseType type)
    {
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