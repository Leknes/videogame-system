namespace Senkel.VideoGame.Settings;

/// <summary>
/// Acts as a mediator between the manager of the setting and the setting value itself.
/// </summary>
public interface ISettingMediator
{
    /// <summary>
    /// The managed setting value.
    /// </summary>
    public ISettingValue Value { get; }

    /// <summary>
    /// Tries to apply the preview value of the managed setting value to the inner value.
    /// </summary>
    /// <returns>If the inner value has been modified.</returns>
    public bool TryApplyPreview();

    /// <summary>
    /// Sets the preview value back to the inner value of the managed setting value.
    /// </summary>
    public void DiscardPreview();
}

/// <summary>
/// Represents the inner setting value that can be managed externally.
/// </summary>
public interface ISettingValue
{
    /// <summary>
    /// Activates the setting value, rendering the caller of the method responsible for managing the setting value.
    /// </summary>
    /// <returns>The meditiator that is used for managing the setting value.</returns>
    public ISettingMediator Activate();
 
    /// <summary>
    /// The name of the setting value that may be used for serialization and deserialization of the setting.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The inner setting value.
    /// </summary>
    public object Value { get; }
}


/// <summary>
/// Represents the inner setting value that can be managed externally with the specified setting type.
/// </summary>
/// <typeparam name="T">The type of the inner setting value.</typeparam>
public interface ISettingValue<T> where T : struct
{
    /// <summary>
    /// The inner setting value.
    /// </summary>
    public T Value { get; }
}

/// <summary>
/// Represents the inner setting value that can be managed externally and can only be activated once with the specified setting type.
/// </summary>
/// <typeparam name="T">The type of the inner setting value.</typeparam>
public class SettingValue<T> : ISettingValue<T>, ISettingValue where T : struct
{
    private readonly SettingPreview<T> _preview;

    private ISettingPreview Preview => _preview;

    private bool _active;

    public SettingValue(SettingPreview<T> preview)
    {
        _preview = preview;
    }

    private protected T ActiveValue { get; private set; }

    public T Value
    {
        get
        {
            if (!_active)
                throw new InvalidOperationException("The setting has to be active in order to represent a value.");

            return ActiveValue;
        }
    }


    object ISettingValue.Value => Value;
     
    string ISettingValue.Name => Preview.Name;

    ISettingMediator ISettingValue.Activate()
    {
        if (_active)
            throw new InvalidOperationException("Setting value has already been activated. This might happen because multiple setting managers are trying to access the setting simultaneously.");

        _active = true;

        ActiveValue = _preview.Value;

        return new Mediator(this);
    }

    private protected virtual bool TryApplyPreview()
    {
        bool modified = !ActiveValue.Equals(_preview);

        ActiveValue = _preview.Value;

        return modified;
    }

    private void DiscardPreview()
    {
        _preview.Value = ActiveValue;
    }

    private class Mediator : ISettingMediator
    {
        public SettingValue<T> Value { get; init; }

        ISettingValue ISettingMediator.Value => Value;

        public Mediator(SettingValue<T> value)
        {
            Value = value;
        }

        public bool TryApplyPreview()
        {
            return Value.TryApplyPreview();
        }

        public void DiscardPreview()
        {
            Value.DiscardPreview();
        }
    }
}


