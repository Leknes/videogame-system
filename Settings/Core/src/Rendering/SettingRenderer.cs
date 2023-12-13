using System.Collections;
using System.Reflection;

namespace Senkel.VideoGame.Settings;

/// <summary>
/// Generates a dictionary containing every important information about the settings on demand based on a given <see cref="SettingManager"/> class.
/// </summary>
public sealed class SettingRenderer : ObjectModel.Rendering.IRenderer<IReadOnlyDictionary<string, object>>
{
    private readonly SettingManager _manager;

    /// <summary>
    /// Creates and instance that is able to render the given setting manager.
    /// </summary>
    /// <param name="manager">The manager to render.</param>
    public SettingRenderer(SettingManager manager)
    {
        _manager = manager;
    }
  
    /// <summary>
    /// Generates a dictionary containing every important information about the settings.
    /// </summary>
    /// <returns>The rendered setting dictionary.</returns>
    public IReadOnlyDictionary<string, object> Render()
    {
        int settingCount = _manager.Settings.Count;

        Dictionary<string, object> result = new Dictionary<string, object>(settingCount);

        for(int i = 0; i < settingCount; i++)
        {
            var setting = _manager.Settings[i];

            result.Add(setting.Name, setting.Value);
        }

        return result;
    }

    internal bool IsRendering(SettingManager manager)
    {
        return manager == _manager;
    }
}