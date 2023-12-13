
namespace Senkel.VideoGame.Settings;

/// <summary>
/// Represents a complete setting that both provides a modifiable preview value and manages an inner setting value.
/// </summary>
public interface ISetting : ISettingPreview, ISettingValue { }

/// <summary>
/// An abstract class for a complete setting with a specifiable setting value class type.
/// </summary>
/// <typeparam name="T">The type of the setting value.</typeparam>
/// <typeparam name="TSettingValue">The type of the setting value class.</typeparam>
public abstract class Setting<T, TSettingValue> : SettingPreview<T>, ISetting, ISettingValue<T> where T : struct where TSettingValue : SettingValue<T>
{  
    /// <summary>
    /// The modifiable preview value.
    /// </summary>
    public T PreviewValue { get => base.Value; set => base.Value = value; }

    /// <summary>
    /// The inner setting value.
    /// </summary>
    public new T Value => SettingValue.Value;

    private protected readonly TSettingValue SettingValue;
    private ISettingValue ISettingValue => SettingValue;

    T ISettingValue<T>.Value => Value; 
    string ISettingValue.Name => ISettingValue.Name;
    object ISettingValue.Value => ISettingValue.Value;

    /// <summary>
    /// Creates a new instance with the preview set to the default value.
    /// </summary>
    public Setting() : base()
    {
        SettingValue = CreateSettingValue();
    }

    /// <summary>
    /// Creates a new setting with the given value or the default value if the value could not be validated as the preview value.
    /// </summary>
    /// <param name="value">The initial preview value.</param>
    public Setting(T value) : base(value)
    {
         SettingValue = CreateSettingValue();
    }

    protected override string Name => GetNameFromType("setting");

    private protected abstract TSettingValue CreateSettingValue();
    
    ISettingMediator ISettingValue.Activate() => ISettingValue.Activate();
}

/// <summary>
/// Represents a complete setting that both provides a modifiable preview value and manages an inner setting value.
/// </summary>
public abstract class Setting<T> : Setting<T, SettingValue<T>>, ISettingValue, ISettingValue<T> where T : struct
{
    /// <summary>
    /// Creates a new instance with the preview set to the default value.
    /// </summary>
    public Setting() : base() { }

    /// <summary>
    /// Creates a new setting with the given value or the default value if the value could not be validated as the preview value.
    /// </summary>
    /// <param name="value">The initial preview value.</param>
    public Setting(T value) : base(value) { }

    private protected override SettingValue<T> CreateSettingValue()
    {
        return new SettingValue<T>(this);
    }
}