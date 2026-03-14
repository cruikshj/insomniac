using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Insomniac;

class Program
{
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern uint SetThreadExecutionState(uint esFlags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool DestroyIcon(IntPtr handle);

    [Flags]
    enum EXECUTION_STATE : uint
    {
        ES_CONTINUOUS = 0x80000000,
        ES_SYSTEM_REQUIRED = 0x00000001,
        ES_DISPLAY_REQUIRED = 0x00000002,
    }

    static NotifyIcon? trayIcon;
    static ToolStripMenuItem? activateItem;
    static ToolStripMenuItem? deactivateItem;
    static bool isActive;

    [STAThread]
    static void Main()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            MessageBox.Show(
                "This application only works on Windows operating systems.",
                "Insomniac",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        activateItem = new ToolStripMenuItem("Activate", null, (_, _) => Activate());
        deactivateItem = new ToolStripMenuItem("Deactivate", null, (_, _) => Deactivate());
        var exitItem = new ToolStripMenuItem("Exit", null, (_, _) =>
        {
            if (isActive) Deactivate();
            Application.Exit();
        });

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add(activateItem);
        contextMenu.Items.Add(deactivateItem);
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add(exitItem);

        trayIcon = new NotifyIcon
        {
            Text = "Insomniac",
            Visible = true,
            ContextMenuStrip = contextMenu,
        };

        Activate();

        Application.Run();

        trayIcon.Visible = false;
        trayIcon.Dispose();
    }

    static void Activate()
    {
        var result = SetThreadExecutionState(
            (uint)(EXECUTION_STATE.ES_CONTINUOUS |
                   EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                   EXECUTION_STATE.ES_DISPLAY_REQUIRED));
        isActive = true;
        UpdateTrayIcon();

        if (result == 0 && trayIcon != null)
        {
            trayIcon.ShowBalloonTip(
                5000,
                "Insomniac Warning",
                "Failed to set execution state. Sleep prevention may not work.",
                ToolTipIcon.Warning);
        }
    }

    static void Deactivate()
    {
        SetThreadExecutionState((uint)EXECUTION_STATE.ES_CONTINUOUS);
        isActive = false;
        UpdateTrayIcon();
    }

    static void UpdateTrayIcon()
    {
        if (trayIcon == null) return;

        var oldIcon = trayIcon.Icon;
        trayIcon.Icon = CreateEyeIcon(isActive);
        oldIcon?.Dispose();

        trayIcon.Text = isActive ? "Insomniac - Active" : "Insomniac - Inactive";
        if (activateItem != null) activateItem.Enabled = !isActive;
        if (deactivateItem != null) deactivateItem.Enabled = isActive;
    }

    static Icon CreateEyeIcon(bool open)
    {
        using var bitmap = new Bitmap(32, 32);
        using var g = Graphics.FromImage(bitmap);
        g.Clear(Color.Transparent);
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (open)
        {
            // Open eye: white ellipse with iris and pupil
            g.FillEllipse(Brushes.White, 2, 10, 28, 12);
            using var outlinePen = new Pen(Color.Black, 2f);
            g.DrawEllipse(outlinePen, 2, 10, 28, 12);
            using var irisBrush = new SolidBrush(Color.SteelBlue);
            g.FillEllipse(irisBrush, 11, 11, 10, 10);
            g.FillEllipse(Brushes.Black, 13, 13, 6, 6);
            g.FillEllipse(Brushes.White, 16, 13, 3, 3);
        }
        else
        {
            // Closed eye: upper arc of eyelid with a closing line at the base
            using var pen = new Pen(Color.Black, 2.5f);
            g.DrawArc(pen, 2, 10, 28, 12, 180, 180);
            g.DrawLine(pen, 2, 16, 30, 16);
        }

        var hIcon = bitmap.GetHicon();
        var icon = (Icon)Icon.FromHandle(hIcon).Clone();
        DestroyIcon(hIcon);
        return icon;
    }
}
