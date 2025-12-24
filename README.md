# Radioactivity Monitor

A .NET console application that simulates monitoring radioactivity levels in a nuclear power plant. The system triggers an alarm when sensor readings fall outside the safe range (17-21).

## Technical Requirements

### Prerequisites

- **.NET 9.0 SDK** or later
- **Docker** (optional, for containerized deployment)

### Supported Platforms

- Windows (x64, x86)
- macOS (x64, ARM64)
- Linux (x64, ARM64)

## Project Structure

```
RadioactivityMonitor/
├── src/
│   ├── RadioactivityMonitor.Core/     # Core library with Alarm, Sensor, and interfaces
│   └── RadioactivityMonitor.App/      # Console application
├── tests/
│   └── RadioactivityMonitor.Tests/    # Unit tests (xUnit)
├── Dockerfile                         # Production Docker image
├── Dockerfile.tests                   # Test runner Docker image
└── RadioactivityMonitor.sln           # Solution file
```

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/encryptedtouhid/RadioactivityMonitor.git
cd RadioactivityMonitor
```

### Build the Solution

```bash
dotnet build RadioactivityMonitor/RadioactivityMonitor.sln
```

### Run the Application

```bash
dotnet run --project RadioactivityMonitor/src/RadioactivityMonitor.App
```

#### Expected Output

```
=== Nuclear Power Plant Radioactivity Monitor ===
Safe range: 17 - 21

Starting monitoring simulation (10 readings)...

Reading Sequence No : 01 ,  Value = 20.56 ,  Status = Normal
Reading Sequence No : 02 ,  Value = 16.25 ,  Status = ALARM! [NEW]
...

Final alarm state: TRIGGERED
Total alarm triggers: X

Monitoring complete.
```

## Running Tests

### Run All Tests

```bash
dotnet test RadioactivityMonitor/RadioactivityMonitor.sln
```

### Run Tests with Detailed Output

```bash
dotnet test RadioactivityMonitor/RadioactivityMonitor.sln --verbosity normal
```

### Run Tests with Code Coverage

```bash
dotnet test RadioactivityMonitor/RadioactivityMonitor.sln --collect:"XPlat Code Coverage"
```

## Docker

### Build and Run the Application

```bash
# Build the Docker image
docker build -t radioactivity-monitor .

# Run the application
docker run --rm radioactivity-monitor
```

### Build and Run Tests in Docker

```bash
# Build the test image
docker build -f Dockerfile.tests -t radioactivity-monitor-tests .

# Run tests
docker run --rm radioactivity-monitor-tests
```

## Test Summary

The project includes **41 unit tests** covering:

| Test Category | Description |
|---------------|-------------|
| Initial State Tests | Verify alarm starts in correct default state |
| Low Threshold Tests | Verify alarm triggers when reading < 17 |
| High Threshold Tests | Verify alarm triggers when reading > 21 |
| Normal Range Tests | Verify alarm stays off for values 17-21 |
| Multiple Check Tests | Verify behavior across multiple readings |
| Alarm Count Tests | Verify alarm trigger counting |
| Edge Cases | Zero, negative, and extreme values |
| Constructor Tests | Null handling and default constructor |
| Threshold Constant Tests | Verify threshold values |
