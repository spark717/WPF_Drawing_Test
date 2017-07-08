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

namespace WpfDrawingTest
{
    public partial class MainWindow : Window
    {
        private Point _lastMouesePos = new Point();

        public Pen PenSimpleLine { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            PenSimpleLine = new Pen(Brushes.Black, 3);

            this.MouseMove += MainWindow_MouseMove;

            this.MouseDown += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    OnMouseDown();
            };

            DrawLine(new Point(0, 0), new Point(50, 50), PenSimpleLine);
        }



        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                OnMouseDrag();
            }
            else
            {
                OnMouseMove();
            }
        }

        private void OnMouseDrag()
        {

        }

        private void OnMouseMove()
        {
            Canva.HitTest(Mouse.GetPosition(this));
        }

        private void OnMouseDown()
        {
            _lastMouesePos = Mouse.GetPosition(this);
        }

        private void DrawLine(Point point1, Point point2, Pen pen)
        {
            DrawingVisual drawing = new DrawingVisual();

            using (DrawingContext drawingContext = drawing.RenderOpen())
            {
                drawingContext.DrawLine(pen, point1, point2);
            }

            Canva.AddVisual(drawing);
        }
    }



    public class DrawingCanvas : FrameworkElement
    {
        private ContainerVisual _container = new ContainerVisual();


        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }



        public DrawingCanvas()
        {
            base.AddVisualChild(_container);
        }



        protected override Visual GetVisualChild(int index)
        {
            return _container;
        }  

        public void AddVisual(Visual visual)
        {
            _container.Children.Add(visual);
        }

        public void HitTest(Point point)
        {
            VisualTreeHelper.HitTest(_container, null, new HitTestResultCallback(myCallback), new PointHitTestParameters(point));
        }

        public HitTestResultBehavior myCallback(HitTestResult result)
        {
            if (result.VisualHit.GetType() == typeof(DrawingVisual))
            {
                ((DrawingVisual)result.VisualHit).Opacity = 0.4;
            }

            // Stop the hit test enumeration of objects in the visual tree.
            return HitTestResultBehavior.Stop;
        }
    }
}
