﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Raggle.Abstractions.Json;

public static class JsonObjectConverter
{
    public static JsonSerializerOptions Options { get; set; } = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        MaxDepth = 32,
    };

    public static bool TryConvertTo<T>(object? obj, [NotNullWhen(true)] out T value)
    {
        var json = ConvertTo<T>(obj);
        if (json != null)
        {
            value = json;
            return true;
        }

        value = default!;
        return false;
    }

    public static T? ConvertTo<T>(object? obj)
    {
        try
        {
            if (obj is T t)
            {
                return t;
            }
            else if (obj is JsonDocument doc)
            {
                // Case 2: The object is a JsonDocument
                return doc.Deserialize<T>(Options);
            }
            else if (obj is JsonElement el)
            {
                // Case 3: The object is a JsonElement
                return el.Deserialize<T>(Options);
            }
            else if (obj is JsonObject jo)
            {
                // Case 4: The object is a JsonObject
                return jo.Deserialize<T>(Options);
            }
            else
            {
                // Case 5: Attempt to serialize and deserialize the object to type T
                string jsonString = JsonSerializer.Serialize(obj, Options);
                return JsonSerializer.Deserialize<T>(jsonString, Options);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return default;
        }
    }
}
