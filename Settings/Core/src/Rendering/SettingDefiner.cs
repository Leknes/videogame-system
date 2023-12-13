namespace Senkel.VideoGame.Settings;

/// <summary>
/// Used for setting the preview values of a given setting collection to the corresponding values in dictionaries. 
/// These values are found by the <see cref="ISettingPreview.Name"/> property and set the preview value that is linked to the name. 
/// This class may be used in order to deserialize the values of a setting collection.
/// </summary>
public sealed class SettingDefiner
{
    private readonly IReadOnlyCollection<ISettingPreview> _previews;

    /// <summary>
    /// The types of the given setting values linked to their setting names.
    /// </summary>
    public readonly IReadOnlyDictionary<string, Type> Definition;

    /// <summary>
    /// Creates a new instance based on the given settings.
    /// </summary>
    /// <param name="previews">The used setting collection.</param>
    public SettingDefiner(IReadOnlyCollection<ISettingPreview> previews)
    {
        _previews = previews;

        Dictionary<string, Type> definition = new Dictionary<string, Type>(previews.Count);

        foreach (var setting in previews)
            definition.Add(setting.Name, setting.ValueType);

        Definition = definition;
    }
  
    /// <summary>
    /// Sets every preview value that has a counterpart with the same name to the value paired with the name.
    /// </summary>
    /// <param name="values">The dictionary of values where the key are the setting names and the values are the new preview values.</param>
    /// <exception cref="InvalidCastException">Throws if a new preview value does not match the type of the corresponding setting.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the new preview value found in the dictionary could not be validated by the corresponding setting.</exception>
    public void Define(IReadOnlyDictionary<string, object> values)
    {
        foreach(var setting in _previews)
        {
            if(!values.TryGetValue(setting.Name, out object? value))
                continue;

            setting.Value = value;
        }
    }

    /// <summary>
    /// Tries to set every preview value that has a counterpart with the same name to the value paired with the name.
    /// </summary>
    /// <param name="values">The dictionary of values where the key are the setting names and the values are the new preview values.</param>
    /// <returns>If every preview value in the dictionary could be applied.</returns>
    /// <exception cref="InvalidCastException">Throws if a new preview value does not match the type of the corresponding setting.</exception>
    public bool TryDefine(IReadOnlyDictionary<string, object> values)
    {
        bool sucessful = true;

        foreach(var setting in _previews)
        {
            if(!values.TryGetValue(setting.Name, out object? value))
                continue;

            if(!setting.TrySet(value))
                sucessful = false;
        }

        return sucessful;
    }

}