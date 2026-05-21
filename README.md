# Insomniac

A C# .NET 10 Windows system tray application that keeps your computer display on and prevents the system from going to sleep.

## Description

Insomniac runs quietly in the system tray with an eye icon. When active (open eye), it uses Windows API calls to set execution state flags that prevent the system from entering sleep mode or turning off the display. When inactive (closed eye), normal power management resumes.

![Insomniac tray icon states](insomnia.png)

The application uses the following Windows execution state flags:
- `ES_CONTINUOUS` - Informs the system that the state being set should remain in effect until the next call
- `ES_SYSTEM_REQUIRED` - Forces the system to be in the working state by resetting the system idle timer
- `ES_DISPLAY_REQUIRED` - Forces the display to be on by resetting the display idle timer

## Requirements

- Windows operating system
- .NET 10 SDK (for building from source)

## Building

### Build from source
```bash
cd Insomniac
dotnet build
```

### Build single-file executable
```bash
cd Insomniac
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The single-file executable will be created in `bin/Release/net10.0-windows/win-x64/publish/Insomniac.exe`

### Build installer

An [InnoSetup](https://jrsoftware.org/isinfo.php) installer script is provided in the `Installer/` directory.

```bash
# First publish the executable
cd Insomniac
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ../publish

# Then compile the installer (requires Inno Setup 6+)
cd ../Installer
ISCC.exe /DAppVersion=1.0.0 Insomniac.iss
```

The installer supports:
- **Per-user installation** (default, no administrator privileges required)
- **System-wide installation** (selectable at install time, requires administrator privileges)
- Start Menu shortcut
- Optional desktop shortcut
- Optional auto-start with Windows

### Download pre-built releases

Pre-built single-file executables and installers are available on the [Releases](../../releases) page. These are automatically built and packaged by GitHub Actions whenever a new release is created.

## Usage

### Run from source
```bash
cd Insomniac
dotnet run
```

### Run compiled binary
Double-click `Insomniac.exe` from Windows Explorer, or run it from the command line.

The application will:
1. Start activated (open eye icon in the system tray)
2. Keep the system and display awake
3. Right-click the tray icon to access:
   - **Activate** – enable sleep prevention (shown when inactive)
   - **Deactivate** – allow normal power management (shown when active)
   - **Exit** – close the application and restore normal power settings

The tray icon shows an open eye (👁) when active and a closed eye when inactive.

## How it Works

The application calls the Windows `SetThreadExecutionState` API function with the appropriate flags to prevent the system from entering sleep mode or turning off the display. When deactivated or when the application exits, it automatically restores the normal power management settings.

## Platform Compatibility

This application is designed specifically for Windows and uses Windows-specific API calls. It will not function on Linux or macOS systems.

## License

No license file is currently included in this repository.