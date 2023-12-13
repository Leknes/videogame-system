namespace Senkel.VideoGame.CoreGame;
 
/// <summary>
/// Responsible of managing loadable objects based on the the specified <see cref="ICoreGameStarter"/> and <see cref="ICoreGameExiter"/> implementations.
/// </summary>
public sealed class CoreGameManager
{
    private readonly ICoreGameStarter _starter;
    private readonly ICoreGameExiter _exiter;

    /// <summary>
    /// Creates a new instance of the <see cref="CoreGameManager"/> class, that is based on the specified <see cref="ICoreGameStarter"/> and <see cref="ICoreGameExiter"/> implementations.
    /// </summary>
    /// <param name="starter">The starter that is used for loading subscribed objects.</param>
    /// <param name="exiter">The exiter that is used for unloading subscribed objects.</param>
    public CoreGameManager(ICoreGameStarter starter, ICoreGameExiter exiter)
    {
        _starter = starter;
        _exiter = exiter;
    }

    /// <summary>
    /// Subscribes the specified <see cref="ILoadable"/> implementation, so that it loads when the core game is started and unloads when it exits.
    /// </summary>
    /// <param name="loadable">The object to subscribe to the manager.</param>
    public void Subscribe(ILoadable loadable)
    {
        _starter.CoreGameStarted += loadable.Load;
        _exiter.CoreGameExited += loadable.Load;
    }

    /// <summary>
    /// Unsubscribes the specified <see cref="ILoadable"/> implementation from the manager.
    /// </summary>
    /// <param name="loadable">The object to usnsubscribe from the manager.</param>
    public void Unsubscribe(ILoadable loadable)
    {
        _starter.CoreGameStarted -= loadable.Load;
        _exiter.CoreGameExited -= loadable.Load;
    }
}