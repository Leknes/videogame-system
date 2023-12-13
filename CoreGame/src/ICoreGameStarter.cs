namespace Senkel.VideoGame.CoreGame;

/// <summary>
/// Represents an object that is capable of starting the core game.
/// </summary>
public interface ICoreGameStarter
{
    /// <summary>
    /// Fires when the core game is started.
    /// </summary>
    public event Action CoreGameStarted;
}