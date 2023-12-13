namespace Senkel.VideoGame.Settings;

/// <summary>
/// Represents the preview of a setting that can be publicly modified. This class is mainly used for internal modification and collection of previews.
/// </summary>
public interface ISettingPreview
{
    /// <summary>
    /// The name of the preview that may be used for serialization and deserialization of the setting.
    /// </summary>
    public string Name {get;}

    /// <summary>
    /// The type of the preview value.
    /// </summary>
    public Type ValueType {get;}

    /// <summary>
    /// The modifiable preview value.
    /// </summary>
    public object Value {get; set;}

    /// <summary>
    /// Tries to set the preview value to the given value.
    /// </summary>
    /// <param name="value">The new preview value.</param>
    /// <returns>If the value could be set sucessfully.</returns>
    public bool TrySet(object value);
}

/// <summary>
/// Represents the preview of a setting that can be publicly modified with a specified type.
/// </summary>
/// <typeparam name="T">The type of the setting preview.</typeparam>
public interface ISettingPreview<T> where T : struct
{
    /// <summary>
    /// The modifiable preview value.
    /// </summary>
    public T Value {get; set;}

    /// <summary>
    /// Tries to set the preview value to the given value.
    /// </summary>
    /// <param name="value">The new preview value.</param>
    /// <returns>If the value could be set sucessfully.</returns>
    public bool TrySet(T value);
}

/// <summary>
/// Represents the preview of a setting that can be publicly modified with a specified type that has to be validated in order to be set.
/// </summary>
/// <typeparam name="T">The type of the setting preview.</typeparam>
public abstract class SettingPreview<T> : ISettingPreview<T>, ISettingPreview where T : struct
{
    /// <summary>
    /// Creates a new instance with the default value.
    /// </summary>
    public SettingPreview()
    {
        Value = GetDefault();
    }

    /// <summary>
    /// Creates a new instance with the given value or the default value if the value could not be validated.
    /// </summary>
    /// <param name="value">The initial preview value.</param>
    public SettingPreview(T value)
    {
        if(!TrySet(value))
            Value = GetDefault();
    }

    private T _value;
 
    public T Value
    {
        get => _value;

        set
        { 
            if (!Validate(value))
                throw new ArgumentOutOfRangeException($"New preview value {value} could not be validated.");

            _value = value;
        }
    }

    /// <summary>
    /// The name of the preview that may be used for serialization and deserialization of the setting.
    /// </summary>
    protected virtual string Name => GetNameFromType("preview");

    private protected string GetNameFromType(string suffix)
    {
        string name = GetType().Name;

        int index;

        int length = name.Length;

        int suffixLength = suffix.Length;

        for (index = length - 1; index > 0; index--)
        {
            if (name[index] == '`')
                length = index;

            if (index <= length - suffixLength && name.Substring(index, suffixLength).ToLowerInvariant() == suffix)
                length = index;
        }

        return name.Substring(0, length);
    }


    string ISettingPreview.Name => Name;
    
    Type ISettingPreview.ValueType => typeof(T);

    object ISettingPreview.Value { get => Value; set => Value = (T)value; }

    bool ISettingPreview.TrySet(object value)
    {
        return TrySet((T)value);
    }

    public bool TrySet(T value)
    {
        if (!Validate(value))
            return false;

        _value = value;

        return true;
    }
 
    /// <summary>
    /// Checks if the given value can be used as a valid preview value.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>If the value could be validated.</returns>
    protected abstract bool Validate(T value);

    /// <summary>
    /// Gets a default value that should be validatable. If no initial value has been provided the this value will be used as an initial preview value.
    /// </summary>
    /// <returns>The default value of the setting.</returns>
    protected abstract T GetDefault();

    
}