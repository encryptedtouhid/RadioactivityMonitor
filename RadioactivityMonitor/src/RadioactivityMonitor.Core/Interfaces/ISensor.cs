namespace RadioactivityMonitor.Core.Interfaces;

/// <summary>
/// Interface for sensors that measure radioactivity.
/// </summary>
public interface ISensor
{
    /// <summary>
    /// Gets the next reading from the sensor.
    /// </summary>
    /// <returns>The radioactivity value.</returns>
    double NextMeasure();
}
