using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Mouse_Move_Motion_Control
{
    public static class MouseController
    {
        private const uint LeftDown = 0x02;
        private const uint LeftUp = 0x04;
        private const uint RightDown = 0x08;
        private const uint RightUp = 0x10;

        private static int maximumXPosition = (int)Math.Round(SystemParameters.PrimaryScreenWidth);
        private static int maximumYPosition = (int)Math.Round(SystemParameters.PrimaryScreenHeight);

        private static int currentXPosition;
        private static int currentYPosition;

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

        public static void RightClick(uint x, uint y)
        {
            MouseEvent(RightDown | RightUp, x, y, 0, UIntPtr.Zero);
        }

        public static void DoubleClick(uint x, uint y)
        {
            MouseEvent(LeftDown | LeftUp, x, y, 0, UIntPtr.Zero);

            Thread.Sleep(150);

            MouseEvent(LeftDown | LeftUp, x, y, 0, UIntPtr.Zero);
        }

        public static void MouseDown()
        {
            MouseEvent(LeftDown, 50, 50, 0, UIntPtr.Zero);
        }

        public static void MouseUp()
        {
            MouseEvent(LeftUp, 50, 50, 0, UIntPtr.Zero);
        }

        public static void ResetCursor()
        {
            currentXPosition = 0;
            currentYPosition = 0;
        }

        public static void MoveCursorBy(float x, float y)
        {
            currentXPosition += (int)Math.Round(x);
            currentYPosition += (int)Math.Round(y);

            currentXPosition = currentXPosition >= 0 ? currentXPosition : 0;
            currentYPosition = currentYPosition >= 0 ? currentYPosition : 0;

            currentXPosition = currentXPosition <= maximumXPosition ? currentXPosition : maximumXPosition;
            currentYPosition = currentYPosition <= maximumYPosition ? currentYPosition : maximumYPosition;

            if (!SetCursorPos(currentXPosition, currentYPosition))
            {
                Debug.WriteLine($"Could not move mouse to [{x}, {y}].");
            }
        }
    }
}
