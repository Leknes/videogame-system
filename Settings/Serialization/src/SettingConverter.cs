using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System;

namespace Senkel.VideoGame.Settings.Serialization;

/// <summary>
/// Represents a json converter that is capable of serializing/deserializing setting renders.
/// </summary>
public sealed class SettingConverter : JsonConverter<IReadOnlyDictionary<string, object>>
{ 
    /// <summary>
    /// Creates a new instance of the <see cref="SettingConverter"/> class that can write json data and read json data using a specified definition.
    /// </summary>
    /// <param name="definition">The definition to use for reading json data.</param>
    public SettingConverter(IReadOnlyDictionary<string, Type>? definition = null)
    {
        Definition = definition;
    }

    /// <summary>
    /// Pairs the names of the settings to the type of the corresponding setting value.
    /// </summary>
    public IReadOnlyDictionary<string, Type>? Definition;
     /// <inheritdoc/>
     
    public override IReadOnlyDictionary<string, object> ReadJson(JsonReader reader, Type objectType, IReadOnlyDictionary<string, object>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (Definition == null)
            throw new InvalidOperationException("A definition for deserializing settings has to be set in order to deserialize.");

        var settingObject = JObject.Load(reader);
        
        bool TryGetValue(JProperty setting, [MaybeNullWhen(false)] out object settingValue)
        {
            if (!Definition.TryGetValue(setting.Name, out var settingType))
            {
                settingValue = null;

                return false;
            }

            settingValue = setting.Value.ToObject(settingType, serializer);

            return settingValue != null;
        }

        var settingDictionary = new Dictionary<string, object>(settingObject.Count);
         
        IEnumerator<JProperty> enumerator = settingObject.Properties().GetEnumerator();

        if (!enumerator.MoveNext())
            return settingDictionary;
         
        if (TryGetValue(enumerator.Current, out var settingValue))
            settingDictionary.Add(enumerator.Current.Name, settingValue);
         
        while(enumerator.MoveNext())
        {
            if (!TryGetValue(enumerator.Current, out settingValue))
                continue;

            if (settingDictionary.ContainsKey(enumerator.Current.Name))
                settingDictionary[enumerator.Current.Name] = settingValue; 
            else
                settingDictionary.Add(enumerator.Current.Name, settingValue);
        }

        return settingDictionary;
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, IReadOnlyDictionary<string, object>? value, JsonSerializer serializer)
    {  
        if (value == null)
        {
            writer.WriteNull();

            return;
        }
         
        var jsonObject = new JObject();

        foreach (var setting in value)
            jsonObject.Add(setting.Key, JToken.FromObject(setting.Value, serializer));
       
        jsonObject.WriteTo(writer);
    }

    
}
 