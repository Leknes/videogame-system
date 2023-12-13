namespace Senkel.VideoGame.CoreGame;

/// <summary>
/// Represents an object that is capable of exiting the core game.
/// </summary>
public interface ICoreGameExiter
{
    /// <summary>
    /// Fired when the core game is exited.
    /// </summary>
    public event Action CoreGameExited;
}