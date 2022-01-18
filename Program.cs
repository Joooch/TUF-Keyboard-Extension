using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TUF_Keyboard_Extension.Modes;
using System.Drawing;

namespace TUF_Keyboard_Extension
{
    class Program
    {
        //[DllImport("user32.dll")] public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        //[DllImport("kernel32.dll")] static extern IntPtr GetConsoleWindow();


        static NotifyIcon notifyIcon = new NotifyIcon();

        static void Main()
        {
            //ShowWindow(GetConsoleWindow(), 0);
            KeyboardAPI keyboardAPI = new KeyboardAPI();
            keyboardAPI.Start();

            System.Timers.Timer tickTimer = new System.Timers.Timer(5);
            tickTimer.AutoReset = true;
            tickTimer.Interval = 1;
            tickTimer.Start();

            BaseMode currentMode = new KeyPressMode(keyboardAPI, tickTimer); // default mode

            notifyIcon.DoubleClick += (s, e) =>
            {
                currentMode?.Dispose();
                Application.Exit();
            };
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Visible = true;
            notifyIcon.Text = Application.ProductName;

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("KeyPress Sensor", null, (s, e) => {
                currentMode?.Dispose();
                currentMode = null;

                currentMode = new KeyPressMode(keyboardAPI, tickTimer);
                currentMode.Start();
            });
            contextMenu.Items.Add("KeyPress Sensor (Reversed)", null, (s, e) => {
                currentMode?.Dispose();
                currentMode = null;

                currentMode = new ReversedKeyPressMode(keyboardAPI, tickTimer);
                currentMode.Start();
            });

            contextMenu.Items.Add("Music Visualization", null, (s, e) => {
                currentMode?.Dispose();
                currentMode = null;

                currentMode = new MusicMode(keyboardAPI, tickTimer);
                currentMode.Start();
            });

            // todo: color settings

            contextMenu.Items.Add("Exit", null, (s, e) => {
                Application.Exit();
            });
            notifyIcon.ContextMenuStrip = contextMenu;

            Application.Run();

            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            keyboardAPI.Stop();

            Environment.Exit(0); // hard exit
            return;
        }
    }
}