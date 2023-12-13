using System.Collections;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace Senkel.VideoGame.Settings;

/// <summary>
/// Manages a collection of setting values by handling discarding and applying the corresponding previews.
/// </summary>
public sealed class SettingManager
{ 
    /// <summary>
    /// Fires whenever the setting preview values are applied and any changes occured.
    /// </summary>
    public event Action? Modified;

    private readonly IReadOnlyList<ISettingMediator> _settings;

    /// <summary>
    /// The setting values managed by the manager.
    /// </summary>
    public readonly IReadOnlyList<ISettingValue> Settings;

    /// <summary>
    /// Instantiates a new manager handling the given setting values.
    /// </summary>
    /// <param name="settings">The setting value collection.</param>
    public SettingManager(IReadOnlyCollection<ISettingValue> settings)
    {
        var settingArray = new ISettingMediator[settings.Count];

        int index = 0;

        foreach(var setting in settings)
            settingArray[index++] = setting.Activate();

        _settings = settingArray;

        Settings = new SettingCollection(settingArray);
    }

    /// <summary>
    /// Applies the preview values for every setting and fires the <see cref="Modified"/> event if any changes occured.
    /// </summary>
    public void ApplyPreview()
    {
        bool modified = false;

        for(int i = _settings.Count - 1; i >= 0; i--)
        {
            if(_settings[i].TryApplyPreview())
                modified = true;
        }

        if(modified)
            Modified?.Invoke();
    }

    /// <summary>
    /// Sets every setting preview back to the corresponding setting value.
    /// </summary>
    public void DiscardPreview()
    {
        for(int i = _settings.Count - 1; i >= 0; i--)
            _settings[i].DiscardPreview();
    }

    private class SettingCollection : IReadOnlyList<ISettingValue>
    {
        private readonly IReadOnlyList<ISettingMediator> _settings;

        public SettingCollection(IReadOnlyList<ISettingMediator> entries)
        {
            _settings = entries; 
        }

        public ISettingValue this[int index] => _settings[index].Value;

        public int Count => _settings.Count;

        public IEnumerator<ISettingValue> GetEnumerator()
        {
            foreach(var setting in _settings)
                yield return setting.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

 
}