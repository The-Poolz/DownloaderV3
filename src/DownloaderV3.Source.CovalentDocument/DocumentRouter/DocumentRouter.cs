using System.Reflection;
using DownloaderV3.Destination.Attributes;
using DownloaderV3.Source.CovalentDocument.Helpers;
using DownloaderV3.Source.CovalentDocument.Decoders;

namespace DownloaderV3.Source.CovalentDocument.DocumentRouter;

public static class DocumentRouter
{
    private static Dictionary<string, string> _responseTypes = new();
    public static readonly Dictionary<string, PropertySetterBeforeSave> PropertySetters = new();

    public static void Initialize(Type destinationType)
    {
        _responseTypes = destinationType.Assembly.GetTypes()
            .Where(x => x.GetCustomAttributes<ResponseModelAttribute>().Any())
            .ToDictionary(key => key.Name, value => $"{value.FullName}, {destinationType.Assembly.GetName().Name}");

        AddDynamicResponseTypes(destinationType);
    }

    private static void AddDynamicResponseTypes(Type destinationType)
    {
        var typesWithPropertySetter = destinationType.Assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<PropertySetterAttribute>() != null);

        foreach (var type in typesWithPropertySetter)
        {
            var propertySetter = type.GetCustomAttribute<PropertySetterAttribute>();

            _responseTypes[type.Name] = _responseTypes[propertySetter!.DbSetName];

            PropertySetters[type.Name] = new PropertySetterBeforeSave(propertySetter!.PropertyName, propertySetter.PropertyValue, propertySetter.DbSetName);
        }
    }

    public static PreSaveActionBinder GetBinder(string responseModel)
    {
        var classLocation = _responseTypes.FirstOrDefault(x => x.Key == responseModel).Value
                            ?? throw new InvalidOperationException($"Response '{responseModel}' not implement with '{nameof(ResponseModelAttribute)}' attribute.");

        var responseType = Type.GetType(classLocation) ?? throw new ArgumentException(string.Format(ExceptionMessages.ClassSpecificationError, classLocation));

        var genericResponseType = typeof(GenericResponse<>).MakeGenericType(responseType);

        return new PreSaveActionBinder(genericResponseType, PropertySetters.GetValueOrDefault(responseModel));
    }

    public static IDocumentResponse GetDocumentType(string responseModel, IEnumerable<IReadOnlyDictionary<string, DataDecoder>> listOfData)
    {
        var binder = GetBinder(responseModel);

        var instance = Activator.CreateInstance(binder.TypeHolder, listOfData, binder.BeforeSave) as IDocumentResponse;

        if (instance != null && binder.BeforeSave is PropertySetterBeforeSave setter) setter.Run(instance);

        return instance!;
    }
}