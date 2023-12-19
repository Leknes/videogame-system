using System.Diagnostics;
using Senkel.Model.Measuring;

namespace Senkel.VideoGame.Metadata;

/// <summary>
/// Represents the metadata of the game including the current play time of the game as well as an integer that indicates the progress status of the game.
/// </summary>
public class VideoGameMetadata
{
    private readonly IStopwatch _stopwatch;

    private readonly TimeSpan _initialPlayTime = TimeSpan.Zero;
     
     /// Represents the current play time of the game that is managed using the initially set <see cref="IStopwatch"/> object.
    /// </summary>
    public TimeSpan PlayTime => _initialPlayTime + _stopwatch.Elapsed;

    /// <summary>
    /// Creates a new instance of the <see cref="VideoGameMetadata"/> class that has the initially set play time as well as the specified progress status.
    /// </summary> 
    /// <param name="playtime">The initial play time captured in the metadata.</param>
    /// <param name="stopwatch">The stopwatch to use for counting elapsed play time.</param>
    public VideoGameMetadata(TimeSpan playtime, IStopwatch stopwatch) : this(stopwatch)
    { 
        _initialPlayTime = playtime;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="VideoGameMetadata"/> class that has the initial <see cref="PlayTime"/> value set to <see cref="TimeSpan.Zero"/>.
    /// </summary>
    /// <param name="stopwatch">The stopwatch to use for counting elapsed play time.</param>
    public VideoGameMetadata(IStopwatch stopwatch)
    {
        _stopwatch = stopwatch;
    }

    /// <summary>
    /// Starts or resumes recording the play time of the metadata using the initially set <see cref="IStopwatch"/> object.
    /// </summary>
    public void StartRecording()
    {
        _stopwatch.Start();
    }

    /// <summary>
    /// Pauses the recording of the play time of the metadata.
    /// </summary>
    public void StopRecording()
    {
        _stopwatch.Stop();
    }
}