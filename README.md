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

### Build from source
```bash
cd Insomniac
dotnet build
```

### Build single-file executable
```bash
cd Insomniac
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
```

The single-file executable will be created in `bin/Release/net10.0/win-x64/publish/Insomniac.exe`

### Download pre-built releases

Pre-built single-file executables are available on the [Releases](../../releases) page. These are automatically built and packaged by GitHub Actions whenever a new release is created.

## Usage

### Run from source
```bash
cd Insomniac
dotnet run
```

### Run compiled binary
```bash
cd Insomniac/bin/Release/net10.0/win-x64/publish
./Insomniac.exe
```

Or simply double-click the `Insomniac.exe` file from Windows Explorer.

The application will:
1. Set the execution state to keep the system and display awake
2. Display a confirmation message
3. Wait for user input (press any key or Ctrl+C to exit)
4. Restore normal power settings when exiting

## How it Works

The application calls the Windows `SetThreadExecutionState` API function with the appropriate flags to prevent the system from entering sleep mode or turning off the display. When the application exits (either through pressing any key or Ctrl+C), it automatically restores the normal power management settings.

## Platform Compatibility

This application is designed specifically for Windows and uses Windows-specific API calls. It will not function on Linux or macOS systems - the application will exit with an error message (exit code 1) when run on non-Windows platforms.

## License

This project is open source and available under standard licensing terms.