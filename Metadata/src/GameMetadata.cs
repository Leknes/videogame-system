using System.Diagnostics;
using Senkel.Unity.API;

namespace Senkel.VideoGame.Metadata;

/// <summary>
/// Represents the metadata of the game including the current play time of the game as well as an integer that indicates the progress status of the game.
/// </summary>
public class GameMetadata
{
    private readonly IStopwatch _stopwatch;
    private readonly double _initialPlayTime = 0;

    /// <summary>
    /// Represents the progress status of the current game as an integer.
    /// </summary>
    public int ProgressStatus = 0;

    /// <summary>
    /// Represents the current play time of the game that is managed using the initially set <see cref="IStopwatch"/> object.
    /// </summary>
    public double PlayTime => _initialPlayTime + _stopwatch.Elapsed;

    /// <summary>
    /// Creates a new instance of the <see cref="GameMetadata"/> class that has the initially set play time as well as the specified progress status.
    /// </summary>
    /// <param name="progressStatus">The progress status of the metadata.</param>
    /// <param name="playtime">The initial play time captured in the metadata.</param>
    /// <param name="stopwatch">The stopwatch to use for counting elapsed play time.</param>
    public GameMetadata(int progressStatus, int playtime, IStopwatch stopwatch)
    {
        ProgressStatus = progressStatus;

        _initialPlayTime = playtime;
        _stopwatch = stopwatch;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="GameMetadata"/> class that has the <see cref="ProgressStatus"/> and the initial <see cref="PlayTime"/> value set to zero.
    /// </summary>
    /// <param name="stopwatch">The stopwatch to use for counting elapsed play time.</param>
    public GameMetadata(IStopwatch stopwatch)
    {
        _stopwatch = stopwatch;
    }

    /// <summary>
    /// Starts or resumes recording the play time of the metadata using the initially set <see cref="IStopwatch"/> object.
    /// </summary>
    public void RecordPlayTime()
    {
        _stopwatch.Start();
    }

    /// <summary>
    /// Pauses the recording of the play time of the metadata.
    /// </summary>
    public void PausePlayTime()
    {
        _stopwatch.Stop();
    }


}