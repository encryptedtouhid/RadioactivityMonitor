namespace RadioactivityMonitor.Tests.Fakes;

using RadioactivityMonitor.Core.Interfaces;

/// <summary>
/// A stub sensor implementation for testing purposes.
/// Returns a predefined value to allow deterministic testing.
/// </summary>
public class StubSensor : ISensor
{
    private readonly double _value;

    public StubSensor(double value)
    {
        _value = value;
    }

    public double NextMeasure()
    {
        return _value;
    }
}
