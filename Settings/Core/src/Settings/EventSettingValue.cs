namespace Senkel.VideoGame.Settings;


/// <summary>
/// Represents a setting value which fires an event whenever the value is modifed.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEventSettingValue<T> : ISettingValue<T> where T : struct
{
    /// <summary>
    /// Fires whenever the setting value has been modified with the new value as an event argument.
    /// </summary>
    public event Action<T>? Modified;
}

/// <summary>
/// Represents a setting value that can be activated and managed. It fires an event whenever the value is modifed.
/// </summary>
/// <typeparam name="T">The type of the setting value.</typeparam>
public class EventSettingValue<T> : SettingValue<T>, IEventSettingValue<T> where T : struct
{
    /// <summary>
    /// Creates a new instance which is based on the given preview value.
    /// </summary>
    /// <param name="preview">The preview value to manage.</param>
    public EventSettingValue(SettingPreview<T> preview) : base(preview) { }
     
    /// <summary>
    /// Fires whenever the setting value has been modified with the new value as an event argument.
    /// </summary>
    public event Action<T>? Modified;

    private protected sealed override bool TryApplyPreview()
    {
        bool modified = base.TryApplyPreview();

        if(modified)
            Modified?.Invoke(ActiveValue);

        return modified;
    }
}