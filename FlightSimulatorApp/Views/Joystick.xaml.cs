using FlightSimulatorApp.ViewModels;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(Joystick),
            new FrameworkPropertyMetadata(125.0, FrameworkPropertyMetadataOptions.AffectsRender, onXChanged));
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(Joystick),
            new FrameworkPropertyMetadata(125.0, FrameworkPropertyMetadataOptions.AffectsRender, onYChanged));
        public Joystick()
        {
            InitializeComponent();
            centerPoint = new Point(Base.Width / 2 - KnobBase.Width / 2, Base.Height / 2 - KnobBase.Height / 2);
            Base.MouseMove += Base_MouseMove;
            Knob.MouseDown += Knob_MouseDown;
            Base.MouseUp += Base_MouseUp;
            centerOfKnob = Knob.Resources["CenterKnob"] as Storyboard;
            X = Convert.ToDouble(GetValue(XProperty));
            Y = Convert.ToDouble(GetValue(YProperty));
        }
        private static void onXChanged(DependencyObject JS, DependencyPropertyChangedEventArgs eventArgs)
        {
            (JS as Joystick).knobPosition.X = (double)eventArgs.NewValue;
        }

        private static void onYChanged(DependencyObject JS, DependencyPropertyChangedEventArgs eventArgs)
        {
            (JS as Joystick).knobPosition.Y = (double)eventArgs.NewValue;
        }

        public double X
        {
            get { return Convert.ToDouble(GetValue(XProperty)); }
            set
            {
                SetValue(XProperty, value);
                knobPosition.X = X;
            }
        }
        public double Y
        {
            get { return Convert.ToDouble(GetValue(YProperty)); }
            set
            {
                SetValue(YProperty, value);
                knobPosition.Y = Y;
            }
        }

        private Point centerPoint;
        private double startOfX = 0;
        private double startOfY = 0;
        private readonly Storyboard centerOfKnob;
        private bool isMousePressed = false;
        private void moveKnobToCenter()
        {
            centerOfKnob.Begin();
            X = centerPoint.X;
            Y = centerPoint.Y;
        }
        private void Base_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMousePressed = false;
            Knob.ReleaseMouseCapture();
            moveKnobToCenter();
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            centerOfKnob.Stop();
            isMousePressed = true;
            Point mousePositionRelativeToKnob = e.MouseDevice.GetPosition(Knob);
            startOfX = KnobBase.Width / 2 - mousePositionRelativeToKnob.X;
            startOfY = KnobBase.Height / 2 - mousePositionRelativeToKnob.Y;
            Knob.CaptureMouse();
        }
        private void Base_MouseMove(object sender, MouseEventArgs e)
        {
            /// If mousePressed, move the Joystick's knob!
            if (isMousePressed)
            {
                Point mousePosition = e.MouseDevice.GetPosition(Base);
                double newX = mousePosition.X - KnobBase.Width / 2 + startOfX;
                double newY = mousePosition.Y - KnobBase.Height / 2 + startOfY;
                double circleRadius = Base.Width / 2;
                double distX = newX - centerPoint.X;
                double distY = newY - centerPoint.Y;

                double result = Math.Sqrt(distX * distX + distY * distY);

                double maxDistanceFromCenter = circleRadius - KnobBase.Width / 2;
                if (result <= maxDistanceFromCenter)
                {
                    X = newX;
                    Y = newY;
                }
                else
                {
                    double vX = newX - centerPoint.X;
                    double vY = newY - centerPoint.Y;
                    double magV = Math.Sqrt(vX * vX + vY * vY);
                    X = centerPoint.X + vX / magV * maxDistanceFromCenter;
                    Y = centerPoint.Y + vY / magV * maxDistanceFromCenter;
                }
            }
        }
        private void CenterKnob_Completed(object sender, EventArgs e)
        {
            
        }

    }
}
