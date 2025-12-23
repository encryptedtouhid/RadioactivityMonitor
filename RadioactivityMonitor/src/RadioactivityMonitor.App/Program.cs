using RadioactivityMonitor.Core.Monitoring;
using RadioactivityMonitor.Core.Sensors;

Console.WriteLine("=== Nuclear Power Plant Radioactivity Monitor ===");
Console.WriteLine($"Safe range: {Alarm.GetLowThreshold()} - {Alarm.GetHighThreshold()}");
Console.WriteLine();

var sensor = new Sensor();
var alarm = new Alarm(sensor);

Console.WriteLine("Starting monitoring simulation (10 readings)...");
Console.WriteLine();

for (int i = 1; i <= 10; i++)
{
    var previousState = alarm.AlarmOn;
    alarm.Check();

    var status = alarm.AlarmOn ? "ALARM!" : "Normal";
    var stateChanged = !previousState && alarm.AlarmOn ? " [NEW]" : "";

    Console.WriteLine($"Reading {i:D2}: Status = {status}{stateChanged}");

    await Task.Delay(500);
}

Console.WriteLine();
Console.WriteLine($"Final alarm state: {(alarm.AlarmOn ? "TRIGGERED" : "OFF")}");
Console.WriteLine($"Total alarm triggers: {alarm.AlarmCount}");
Console.WriteLine();
Console.WriteLine("Monitoring complete.");
