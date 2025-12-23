namespace RadioactivityMonitor.Tests;

using RadioactivityMonitor.Core.Monitoring;
using RadioactivityMonitor.Tests.Fakes;

public class AlarmTests
{
    private static readonly double LowThreshold = Alarm.GetLowThreshold();
    private static readonly double HighThreshold = Alarm.GetHighThreshold();

    #region Initial State Tests

    [Fact]
    public void AlarmOn_WhenNewlyCreated_ShouldBeFalse()
    {
        // Arrange
        var sensor = new StubSensor(18.0);
        var alarm = new Alarm(sensor);

        // Act & Assert
        Assert.False(alarm.AlarmOn);
    }

    [Fact]
    public void AlarmCount_WhenNewlyCreated_ShouldBeZero()
    {
        // Arrange
        var sensor = new StubSensor(18.0);
        var alarm = new Alarm(sensor);

        // Act & Assert
        Assert.Equal(0, alarm.AlarmCount);
    }

    [Fact]
    public void AlarmOn_BeforeCheckIsCalled_ShouldBeFalse()
    {
        // Arrange
        var sensor = new StubSensor(15.0); // Below threshold
        var alarm = new Alarm(sensor);

        // Assert - Alarm should not be on until Check() is called
        Assert.False(alarm.AlarmOn);
    }

    #endregion

    #region Low Threshold Tests

    [Fact]
    public void Check_WhenValueBelowLowThreshold_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(16.0); // Below 17.0
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenValueAtLowThreshold_ShouldNotTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(LowThreshold); // Exactly 17.0
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.False(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenValueJustBelowLowThreshold_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(LowThreshold - 0.001);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenValueSignificantlyBelowLowThreshold_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(5.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    #endregion

    #region High Threshold Tests

    [Fact]
    public void Check_WhenValueAboveHighThreshold_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(22.0); // Above 21.0
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenValueAtHighThreshold_ShouldNotTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(HighThreshold); // Exactly 21.0
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.False(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenValueJustAboveHighThreshold_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(HighThreshold + 0.001);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenValueSignificantlyAboveHighThreshold_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(100.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    #endregion

    #region Normal Range Tests

    [Fact]
    public void Check_WhenValueInNormalRange_ShouldNotTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(19.0); // Between 17.0 and 21.0
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.False(alarm.AlarmOn);
    }

    [Theory]
    [InlineData(17.0)]  // At low threshold
    [InlineData(18.0)]
    [InlineData(19.0)]  // Middle
    [InlineData(20.0)]
    [InlineData(21.0)]  // At high threshold
    public void Check_WhenValueWithinSafeRange_ShouldNotTriggerAlarm(double value)
    {
        // Arrange
        var sensor = new StubSensor(value);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.False(alarm.AlarmOn);
    }

    [Theory]
    [InlineData(16.99)]
    [InlineData(15.0)]
    [InlineData(0.0)]
    [InlineData(-5.0)]
    public void Check_WhenValueBelowSafeRange_ShouldTriggerAlarm(double value)
    {
        // Arrange
        var sensor = new StubSensor(value);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    [Theory]
    [InlineData(21.01)]
    [InlineData(25.0)]
    [InlineData(100.0)]
    [InlineData(1000.0)]
    public void Check_WhenValueAboveSafeRange_ShouldTriggerAlarm(double value)
    {
        // Arrange
        var sensor = new StubSensor(value);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    #endregion

    #region Multiple Check Tests

    [Fact]
    public void Check_WhenCalledMultipleTimesWithNormalValues_ShouldNotTriggerAlarm()
    {
        // Arrange
        var sensor = new SequenceSensor(18.0, 19.0, 20.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();
        alarm.Check();
        alarm.Check();

        // Assert
        Assert.False(alarm.AlarmOn);
        Assert.Equal(0, alarm.AlarmCount);
    }

    [Fact]
    public void Check_WhenAlarmTriggered_ShouldStayTriggered()
    {
        // Arrange - Note: This tests current behavior that alarm stays on once triggered
        var sensor = new SequenceSensor(15.0, 19.0, 20.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check(); // Low value - triggers alarm
        Assert.True(alarm.AlarmOn);

        alarm.Check(); // Normal value
        alarm.Check(); // Normal value

        // Assert - Alarm stays on (current behavior - see improvements)
        Assert.True(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenNormalThenAbnormal_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new SequenceSensor(19.0, 19.0, 25.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();
        alarm.Check();
        Assert.False(alarm.AlarmOn);

        alarm.Check(); // High value

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    #endregion

    #region Alarm Count Tests

    [Fact]
    public void AlarmCount_WhenAlarmTriggeredOnce_ShouldBeOne()
    {
        // Arrange
        var sensor = new StubSensor(15.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.Equal(1, alarm.AlarmCount);
    }

    [Fact]
    public void AlarmCount_WhenAlarmTriggeredMultipleTimes_ShouldIncrementEachTime()
    {
        // Arrange - All values are out of range
        var sensor = new SequenceSensor(15.0, 25.0, 10.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();
        alarm.Check();
        alarm.Check();

        // Assert
        Assert.Equal(3, alarm.AlarmCount);
    }

    [Fact]
    public void AlarmCount_WhenNormalValuesFollowedByAbnormal_ShouldCountOnlyAbnormal()
    {
        // Arrange
        var sensor = new SequenceSensor(18.0, 19.0, 25.0, 15.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check(); // Normal
        alarm.Check(); // Normal
        alarm.Check(); // Abnormal
        alarm.Check(); // Abnormal

        // Assert
        Assert.Equal(2, alarm.AlarmCount);
    }

    [Fact]
    public void AlarmCount_WhenNoAlarmTriggered_ShouldRemainZero()
    {
        // Arrange
        var sensor = new SequenceSensor(18.0, 19.0, 20.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();
        alarm.Check();
        alarm.Check();

        // Assert
        Assert.Equal(0, alarm.AlarmCount);
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [Fact]
    public void Check_WhenValueIsZero_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(0.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenValueIsNegative_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(-10.0);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenValueIsMaxDouble_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(double.MaxValue);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    [Fact]
    public void Check_WhenValueIsMinDouble_ShouldTriggerAlarm()
    {
        // Arrange
        var sensor = new StubSensor(double.MinValue);
        var alarm = new Alarm(sensor);

        // Act
        alarm.Check();

        // Assert
        Assert.True(alarm.AlarmOn);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_WhenSensorIsNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Alarm(null!));
    }

    [Fact]
    public void Constructor_WithDefaultConstructor_ShouldNotThrow()
    {
        // Act & Assert - Should not throw
        var alarm = new Alarm();
        Assert.NotNull(alarm);
        Assert.False(alarm.AlarmOn);
    }

    #endregion

    #region Threshold Constant Tests

    [Fact]
    public void LowThreshold_ShouldBe17()
    {
        // Assert
        Assert.Equal(17.0, Alarm.GetLowThreshold());
    }

    [Fact]
    public void HighThreshold_ShouldBe21()
    {
        // Assert
        Assert.Equal(21.0, Alarm.GetHighThreshold());
    }

    [Fact]
    public void Thresholds_LowShouldBeLessThanHigh()
    {
        // Assert
        Assert.True(Alarm.GetLowThreshold() < Alarm.GetHighThreshold());
    }

    #endregion
}
