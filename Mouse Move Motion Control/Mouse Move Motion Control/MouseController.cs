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
        #region DLL Imports
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
        #endregion

        private const uint LeftDown = 0x02;
        private const uint LeftUp = 0x04;
        private const uint RightDown = 0x08;
        private const uint RightUp = 0x10;

        private static ButtonStatus leftMouseButtonStatus = ButtonStatus.Released;
        private static ButtonStatus rightMouseButtonStatus = ButtonStatus.Released;

        private static int maximumXPosition = (int)Math.Round(SystemParameters.PrimaryScreenWidth);
        private static int maximumYPosition = (int)Math.Round(SystemParameters.PrimaryScreenHeight);

        private static int currentXPosition;
        private static int currentYPosition;

        public static ButtonStatus LeftMouseButtonStatus
        {
            get
            {
                return leftMouseButtonStatus;
            }
        }

        public static ButtonStatus RightMouseButtonStatus
        {
            get
            {
                return rightMouseButtonStatus;
            }
        }

        #region CursorMove
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
        #endregion

        #region Left Mouse Button Actions
        public static void PressLeftMouseButton()
        {
            leftMouseButtonStatus = ButtonStatus.Pressed;

            LeftMouseDown();
        }

        public static void ReleaseLeftMouseButton()
        {
            leftMouseButtonStatus = ButtonStatus.Released;

            LeftMouseUp();
        }

        public static void HoldLeftMouseButton()
        {
            leftMouseButtonStatus = ButtonStatus.Hold;
        }
        #endregion

        #region Right Mouse Button Actions
        public static void PressRightMouseButton()
        {
            rightMouseButtonStatus = ButtonStatus.Pressed;

            RightMouseDown();
        }

        public static void ReleaseRightMouseButton()
        {
            rightMouseButtonStatus = ButtonStatus.Released;

            RightMouseUp();
        }

        public static void HoldRightMouseButton()
        {
            rightMouseButtonStatus = ButtonStatus.Hold;
        }
        #endregion

        #region Private Methods
        private static void LeftMouseDown()
        {
            mouse_event(LeftDown, 50, 50, 0, UIntPtr.Zero);
        }

        private static void LeftMouseUp()
        {
            mouse_event(LeftUp, 50, 50, 0, UIntPtr.Zero);
        }

        private static void RightMouseDown()
        {
            mouse_event(RightDown, 50, 50, 0, UIntPtr.Zero);
        }

        private static void RightMouseUp()
        {
            mouse_event(RightUp, 50, 50, 0, UIntPtr.Zero);
        }
        #endregion
    }
}
