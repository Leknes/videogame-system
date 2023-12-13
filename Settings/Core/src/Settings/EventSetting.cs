namespace Senkel.VideoGame.Settings;

/// <summary>
/// Represents a setting with a modifiable preview value and an inner setting value that fires the <see cref="Modified"/> event when any changes occur.
/// </summary>
/// <typeparam name="T">The type of setting value.</typeparam>
public abstract class EventSetting<T> : Setting<T, EventSettingValue<T>>, IEventSettingValue<T> where T : struct
{
    /// <summary>
    /// Creates a new instance with the preview set to the default value.
    /// </summary>
    public EventSetting() : base() { }

    /// <summary>
    /// Creates a new setting with the given value or the default value if the value could not be validated as the preview value.
    /// </summary>
    /// <param name="value">The initial preview value.</param>
    public EventSetting(T value) : base(value) { }

    public event Action<T>? Modified
    {
        add => SettingValue.Modified += value;
        remove => SettingValue.Modified -= value;
    }

    private protected sealed override EventSettingValue<T> CreateSettingValue()
    {
        return new EventSettingValue<T>(this);
    }
}