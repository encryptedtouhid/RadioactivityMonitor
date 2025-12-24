namespace RadioactivityMonitor.Core.Monitoring;

using RadioactivityMonitor.Core.Interfaces;
using RadioactivityMonitor.Core.Sensors;

/// <summary>
/// Watches radioactivity levels and turns on an alarm if the reading is too high or too low.
/// </summary>
public class Alarm
{
    private const double LowThreshold = 17;
    private const double HighThreshold = 21;

    private readonly ISensor _sensor;
    private bool _alarmOn = false;
    private long _alarmCount = 0;
    private double _lastReading;

    /// <summary>
    /// Creates a new Alarm with a default sensor.
    /// </summary>
    public Alarm() : this(new Sensor())
    {
    }

    /// <summary>
    /// Creates a new Alarm with the sensor you give it.
    /// </summary>
    /// <param name="sensor">The sensor to read values from.</param>
    public Alarm(ISensor sensor)
    {
        _sensor = sensor ?? throw new ArgumentNullException(nameof(sensor));
    }

    /// <summary>
    /// Reads the sensor and turns on the alarm if the value is outside the safe range (17 to 21).
    /// </summary>
    public void Check()
    {
        double value = _sensor.NextMeasure();
        _lastReading = value;

        if (value < LowThreshold || HighThreshold < value)
        {
            _alarmOn = true;
            _alarmCount += 1;
        }
    }

    /// <summary>
    /// Returns true if the alarm is on, false if it's off.
    /// </summary>
    public bool AlarmOn
    {
        get { return _alarmOn; }
    }

    /// <summary>
    /// Returns how many times the alarm has been triggered.
    /// </summary>
    public long AlarmCount
    {
        get { return _alarmCount; }
    }

    /// <summary>
    /// Returns the last reading from the sensor.
    /// </summary>
    public double LastReading
    {
        get { return _lastReading; }
    }

    /// <summary>
    /// Returns the low threshold value (17).
    /// </summary>
    public static double GetLowThreshold() => LowThreshold;

    /// <summary>
    /// Returns the high threshold value (21).
    /// </summary>
    public static double GetHighThreshold() => HighThreshold;
}
