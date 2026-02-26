using System.Runtime.InteropServices;

namespace Insomniac;

class Program
{
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern uint SetThreadExecutionState(uint esFlags);

    [Flags]
    enum EXECUTION_STATE : uint
    {
        ES_CONTINUOUS = 0x80000000,
        ES_SYSTEM_REQUIRED = 0x00000001,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_AWAYMODE_REQUIRED = 0x00000040
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Insomniac - Keeping your computer awake");
        Console.WriteLine("========================================");
        Console.WriteLine();
        
        // Check if running on Windows
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Console.WriteLine("Error: This application only works on Windows operating systems.");
            Console.WriteLine("The SetThreadExecutionState API is a Windows-specific feature.");
            Environment.Exit(1);
            return;
        }
        
        // Set the execution state to keep the system and display awake
        uint previousState = SetThreadExecutionState(
            (uint)(EXECUTION_STATE.ES_CONTINUOUS | 
                   EXECUTION_STATE.ES_SYSTEM_REQUIRED | 
                   EXECUTION_STATE.ES_DISPLAY_REQUIRED));

        if (previousState == 0)
        {
            Console.WriteLine("Warning: Failed to set execution state. Error code: " + Marshal.GetLastWin32Error());
            Console.WriteLine("This application may not work correctly.");
        }
        else
        {
            Console.WriteLine("✓ System and display are now being kept awake");
        }

        Console.WriteLine();
        Console.WriteLine("Press Ctrl+C or any key to exit and restore normal power settings...");
        Console.WriteLine();

        // Set up Ctrl+C handler to restore execution state before exit
        Console.CancelKeyPress += (sender, e) =>
        {
            RestoreExecutionState();
            Environment.Exit(0);
        };

        // Wait for user input
        Console.ReadKey(true);

        // Restore normal execution state when exiting
        RestoreExecutionState();
        
        Console.WriteLine("Normal power settings restored. Goodbye!");
    }

    static void RestoreExecutionState()
    {
        Console.WriteLine();
        Console.WriteLine("Restoring normal power settings...");
        
        // Clear the execution state flags to restore normal power management
        SetThreadExecutionState((uint)EXECUTION_STATE.ES_CONTINUOUS);
        
        Console.WriteLine("✓ Normal power settings restored");
    }
}
