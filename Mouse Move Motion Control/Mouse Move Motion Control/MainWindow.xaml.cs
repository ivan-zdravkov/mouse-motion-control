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

        private void LeapControllerFrameReady(object sender, FrameEventArgs e)
        {
            Hand leftHand = e.frame.Hands.FirstOrDefault(x => x.IsLeft);
            Hand rightHand = e.frame.Hands.FirstOrDefault(x => x.IsRight);

            if (leftHand != null)
            {
                Finger indexFinger = leftHand.Fingers.FirstOrDefault(x => x.Type == Finger.FingerType.TYPE_INDEX);

                if (indexFinger != null)
                {
                    this.MoveCursor(indexFinger.TipVelocity.x / 20.0f, -indexFinger.TipVelocity.y / 20.0f);
                }
            }
        }

        private void MoveCursor(float fingerPositionX, float fingerPositionY)
        { 
            MouseController.MoveCursorBy(fingerPositionX, fingerPositionY);
        }

        private void StopMotionControlButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.LeapController != null)
            {
                this.LeapController.FrameReady -= this.LeapControllerFrameReady;

                this.LeapController = null;
            }
        }
    }
}
