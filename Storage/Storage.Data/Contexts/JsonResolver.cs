﻿using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Storage.Data.Entity;

namespace Storage.Data.Contexts;

public class JsonResolver : DefaultContractResolver
{
private readonly Dictionary<Type, HashSet<string>> _ignores = new();

public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
    {
        if (!_ignores.ContainsKey(type))
            _ignores[type] = new HashSet<string>();

        foreach (var prop in jsonPropertyNames)
            _ignores[type].Add(prop);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (IsIgnored(property.DeclaringType, property.PropertyName))
        {
            property.ShouldSerialize = i => false;
            property.Ignored = true;
        }

        return property;
    }

    private bool IsIgnored(Type type, string jsonPropertyName)
    {
        if (!_ignores.ContainsKey(type))
            return false;

        return _ignores[type].Contains(jsonPropertyName);
    }
}