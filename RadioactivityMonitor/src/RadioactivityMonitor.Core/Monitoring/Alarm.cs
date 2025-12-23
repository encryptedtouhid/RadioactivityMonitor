namespace RadioactivityMonitor.Core.Monitoring;

using RadioactivityMonitor.Core.Interfaces;
using RadioactivityMonitor.Core.Sensors;

/// <summary>
/// Monitors radioactivity levels and triggers an alarm if readings fall outside safe thresholds.
/// </summary>
public class Alarm
{
    private const double LowThreshold = 17;
    private const double HighThreshold = 21;

    private readonly ISensor _sensor;
    private bool _alarmOn = false;
    private long _alarmCount = 0;

    /// <summary>
    /// Initializes a new instance of the Alarm class with a default Sensor.
    /// </summary>
    public Alarm() : this(new Sensor())
    {
    }

    /// <summary>
    /// Initializes a new instance of the Alarm class with a specified sensor.
    /// </summary>
    /// <param name="sensor">The sensor to use for radioactivity readings.</param>
    public Alarm(ISensor sensor)
    {
        _sensor = sensor ?? throw new ArgumentNullException(nameof(sensor));
    }

    /// <summary>
    /// Checks the current radioactivity value and updates the alarm state.
    /// </summary>
    public void Check()
    {
        double value = _sensor.NextMeasure();

        if (value < LowThreshold || HighThreshold < value)
        {
            _alarmOn = true;
            _alarmCount += 1;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the alarm is currently triggered.
    /// </summary>
    public bool AlarmOn
    {
        get { return _alarmOn; }
    }

    /// <summary>
    /// Gets the number of times the alarm has been triggered.
    /// </summary>
    public long AlarmCount
    {
        get { return _alarmCount; }
    }

    /// <summary>
    /// Gets the low threshold value for testing purposes.
    /// </summary>
    public static double GetLowThreshold() => LowThreshold;

    /// <summary>
    /// Gets the high threshold value for testing purposes.
    /// </summary>
    public static double GetHighThreshold() => HighThreshold;
}
