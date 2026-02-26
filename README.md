# Insomniac

A C# .NET 10 console application that keeps your computer display on and prevents the system from going to sleep while running.

## Description

Insomniac uses Windows API calls to set execution state flags that prevent the system from entering sleep mode or turning off the display. This is useful when you need to keep your computer awake for long-running tasks, presentations, or monitoring.

The application uses the following Windows execution state flags:
- `ES_CONTINUOUS` - Informs the system that the state being set should remain in effect until the next call
- `ES_SYSTEM_REQUIRED` - Forces the system to be in the working state by resetting the system idle timer
- `ES_DISPLAY_REQUIRED` - Forces the display to be on by resetting the display idle timer

## Requirements

- Windows operating system
- .NET 10.0 or higher

## Building

```bash
cd Insomniac
dotnet build
```

## Usage

### Run from source
```bash
cd Insomniac
dotnet run
```

### Run compiled binary
```bash
cd Insomniac/bin/Debug/net10.0
./Insomniac.exe
```

The application will:
1. Set the execution state to keep the system and display awake
2. Display a confirmation message
3. Wait for user input (press any key or Ctrl+C to exit)
4. Restore normal power settings when exiting

## How it Works

The application calls the Windows `SetThreadExecutionState` API function with the appropriate flags to prevent the system from entering sleep mode or turning off the display. When the application exits (either through pressing any key or Ctrl+C), it automatically restores the normal power management settings.

## Platform Compatibility

This application is designed specifically for Windows and uses Windows-specific API calls. It will not function as intended on Linux or macOS systems, though it will compile and run without error on those platforms (displaying a warning message).

## License

This project is open source and available under standard licensing terms.