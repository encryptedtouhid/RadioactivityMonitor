namespace RadioactivityMonitor.Core.Interfaces;

/// <summary>
/// Interface for radioactivity sensors.
/// Allows for dependency injection and testability.
/// </summary>
public interface ISensor
{
    /// <summary>
    /// Gets the next measurement from the sensor.
    /// </summary>
    /// <returns>The radioactivity value in appropriate units.</returns>
    double NextMeasure();
}
