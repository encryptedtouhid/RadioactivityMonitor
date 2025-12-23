namespace RadioactivityMonitor.Tests.Fakes;

using RadioactivityMonitor.Core.Interfaces;

/// <summary>
/// A sensor implementation that returns values from a predefined sequence.
/// Useful for testing scenarios that require multiple readings.
/// </summary>
public class SequenceSensor : ISensor
{
    private readonly double[] _values;
    private int _currentIndex;

    public SequenceSensor(params double[] values)
    {
        if (values == null || values.Length == 0)
        {
            throw new ArgumentException("At least one value must be provided.", nameof(values));
        }

        _values = values;
        _currentIndex = 0;
    }

    public int CallCount => _currentIndex;

    public double NextMeasure()
    {
        if (_currentIndex >= _values.Length)
        {
            return _values[^1]; // Return last value if sequence exhausted
        }

        return _values[_currentIndex++];
    }
}
