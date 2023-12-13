namespace Senkel.VideoGame.Settings;

/// <summary>
/// Reports the current settings whenever the given <see cref="SettingManager"/> class is modified by using an appropriate <see cref="SettingRenderer"/> class.
/// </summary>
public sealed class SettingReporter
{
    /// <summary>
    /// Fires whenever the corresponding setting manager is modified containing the new current settings.
    /// </summary>
    public event Action<IReadOnlyDictionary<string, object>>? Modified;

    private readonly SettingRenderer _renderer;

    /// <summary>
    /// Creates a new instance with the given setting manager and setting renderer where the renderer has to be handling the manager.
    /// </summary>
    /// <param name="manager">The setting manager to report.</param>
    /// <param name="renderer">The setting renderer to use.</param>
    /// <exception cref="ArgumentException">Throws if the setting renderer does not render the given setting manager.</exception>
    public SettingReporter(SettingManager manager, SettingRenderer renderer)
    {
        if(!renderer.IsRendering(manager))
            throw new ArgumentException("Renderer is not managed by the given manager.");

        _renderer = renderer;

        manager.Modified += OnModified;
    }

    /// <summary>
    /// Creates a new instance with the given setting manager and privately creates a new setting renderer based on the setting manager.
    /// </summary>
    /// <param name="manager">The setting manager to report.</param>
    public SettingReporter(SettingManager manager)
    {
        _renderer = new SettingRenderer(manager);

        manager.Modified += OnModified;
    }
 
    private void OnModified()
    {
        Modified?.Invoke(_renderer.Render());
    }
}