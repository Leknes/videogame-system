namespace Senkel.VideoGame.Settings;

/// <summary>
/// A container class granting access to the <see cref="SettingManager"/> class, the corresponding <see cref="SettingRenderer"/> 
/// class as well as the <see cref="SettingReporter"/> class based on a given setting value collection.
/// </summary>
public class SettingResult
{
    /// <summary>
    /// An instance of the <see cref="SettingManager"/> class based on the given setting value collection.
    /// </summary>
    public readonly SettingManager Manager;

    private SettingReporter? _reporter;
    private SettingRenderer? _renderer; 

    /// <summary>
    /// An instance of the <see cref="SettingReporter"/> class based on the given <see cref="Manager"/> and <see cref="Renderer"/>.
    /// </summary>
    public SettingReporter Reporter
    {
        get
        {
            if(_reporter == null)
                _reporter = new SettingReporter(Manager, Renderer);

            return _reporter;
        }
    }

    /// <summary>
    /// An instance of the <see cref="SettingRenderer"/> class based on the given <see cref="Manager"/>.
    /// </summary>
    public SettingRenderer Renderer
    {
        get
        {
            if(_renderer == null)
                _renderer = new SettingRenderer(Manager);

            return _renderer;
        }
    }

    /// <summary>
    /// Creates a new setting result as well as a corresponding instance of the <see cref="SettingManager"/> class.
    /// </summary>
    /// <param name="settings">The setting value collection to use.</param>
    public SettingResult(IReadOnlyCollection<ISettingValue> settings)
    {
        Manager = new SettingManager(settings);
    }
}