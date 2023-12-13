namespace Senkel.VideoGame.CoreGame;

/// <summary>
/// Represents an object that can can both loaded and unloaded.
/// </summary>
public interface ILoadable
{
    /// <summary>
    /// Loads this object.
    /// </summary>
    public void Load();

    /// <summary>
    /// Unloads this object, so that it has to be loaded again.
    /// </summary>
    public void Unload();
}