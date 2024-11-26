namespace Storage.Data;

public class JsonIgnoreStorageAttribute : Attribute
{
    public string[] PropertyNames { get; }

    public JsonIgnoreStorageAttribute(params string[] propertyNames)
    {
        PropertyNames = propertyNames;
    }
}