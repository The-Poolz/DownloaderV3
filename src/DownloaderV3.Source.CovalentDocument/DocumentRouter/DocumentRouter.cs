using System.Reflection;
using DownloaderV3.Destination.Attributes;
using DownloaderV3.Source.CovalentDocument.Helpers;
using DownloaderV3.Source.CovalentDocument.Decoders;

namespace DownloaderV3.Source.CovalentDocument.DocumentRouter;

public static class DocumentRouter
{
    private static Dictionary<string, string> _responseTypes = new();

    public static void Initialize(Type destinationType)
    {
        _responseTypes = destinationType.Assembly.GetTypes()
            .Where(x => x.GetCustomAttributes<ResponseModelAttribute>().Any())
            .ToDictionary(key => key.Name, value => $"{value.FullName}, {destinationType.Assembly.GetName().Name}")!;
    }

    public static PreSaveActionBinder GetBinder(string responseModel)
    {
        var classLocation = _responseTypes.FirstOrDefault(x => x.Key == responseModel).Value
            ?? throw new InvalidOperationException($"Response '{responseModel}' not implement with '{nameof(ResponseModelAttribute)}' attribute.");

        // TODO: Logger
        //var responseType = Type.GetType(classLocation) ?? ApplicationLogger.LogAndThrowDynamic(new ArgumentException(string.Format(ExceptionMessages.ClassSpecificationError, classLocation)));
        var responseType = Type.GetType(classLocation) ?? throw new ArgumentException(string.Format(ExceptionMessages.ClassSpecificationError, classLocation));

        var genericResponseType = typeof(GenericResponse<>).MakeGenericType(responseType);

        return new PreSaveActionBinder(genericResponseType, null);
    }

    public static IDocumentResponse GetDocumentType(string responseModel, IEnumerable<IReadOnlyDictionary<string, DataDecoder>> listOfData)
    {
        var binder = GetBinder(responseModel);
        return (IDocumentResponse)Activator.CreateInstance(binder.TypeHolder, listOfData, binder.BeforeSave)!;
    }
}