using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Leap;

namespace Mouse_Move_Motion_Control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller LeapController;

        private int cursorPositionX = 0;
        private int cursorPositionY = 0;

        private float cursorSpeed = 0; // Use values 0 (Slow) - 10 (Fast)

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartMotionControlButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.LeapController == null)
            {
                MouseController.ResetCursor();

                this.LeapController = new Controller();

                this.LeapController.FrameReady += this.LeapControllerFrameReady;
            }
        }

        private void StopMotionControlButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.LeapController != null)
            {
                this.LeapController.FrameReady -= this.LeapControllerFrameReady;

                this.LeapController = null;
            }
        }

        private void LeapControllerFrameReady(object sender, FrameEventArgs e)
        {
            Hand leftHand = e.frame.Hands.FirstOrDefault(x => x.IsLeft);
            Hand rightHand = e.frame.Hands.FirstOrDefault(x => x.IsRight);

            if (rightHand != null)
            {
                CheckLeftClick(rightHand);
                CheckRightClick(rightHand);

                if (rightHand.PalmPosition.y < 115.0f)
                {
                    float cursorSpeedMultiplier = (20.0f - this.cursorSpeed);

                    MouseController.MoveCursorBy(rightHand.PalmVelocity.x / cursorSpeedMultiplier, rightHand.PalmVelocity.z / cursorSpeedMultiplier);
                }
            }
        }

        private void CheckLeftClick(Hand rightHand)
        {
            float leftClickFingerDistance = this.FingerDistance(rightHand, Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_THUMB);

            if (leftClickFingerDistance < 50.0f)
            {
                if (MouseController.LeftMouseButtonStatus == ButtonStatus.Released)
                {
                    MouseController.PressLeftMouseButton();
                }
                else if (MouseController.LeftMouseButtonStatus == ButtonStatus.Pressed)
                {
                    MouseController.HoldLeftMouseButton();
                }
            }
            else
            {
                if (MouseController.LeftMouseButtonStatus != ButtonStatus.Released)
                {
                    MouseController.ReleaseLeftMouseButton();
                }
            }
        }

        private void CheckRightClick(Hand rightHand)
        {
            float rightClickFingerDistance = this.FingerDistance(rightHand, Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_THUMB);

            if (rightClickFingerDistance < 50.0f)
            {
                if (MouseController.RightMouseButtonStatus == ButtonStatus.Released)
                {
                    MouseController.PressRightMouseButton();
                }
                else if (MouseController.RightMouseButtonStatus == ButtonStatus.Pressed)
                {
                    MouseController.HoldRightMouseButton();
                }
            }
            else
            {
                if (MouseController.RightMouseButtonStatus != ButtonStatus.Released)
                {
                    MouseController.ReleaseRightMouseButton();
                }
            }
        }

        private float FingerDistance(Hand hand, Finger.FingerType firstFingerType, Finger.FingerType secondFingerType)
        {
            if (hand != null)
            {
                Finger firstFinger = hand.Fingers.FirstOrDefault(f => f.Type == firstFingerType);
                Finger secondFinger = hand.Fingers.FirstOrDefault(f => f.Type == secondFingerType);

                if (firstFinger != null && secondFinger != null)
                {
                    return firstFinger.TipPosition.DistanceTo(secondFinger.TipPosition);
                }
            }

            return 100.0f;
        }

    }
}
