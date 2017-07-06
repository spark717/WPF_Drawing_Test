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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                Pen drawingpen = new Pen(Brushes.Black, 1);
                //dc.DrawLine(drawingpen, new Point(0, 50), new Point(50, 0));
                //dc.DrawLine(drawingpen, new Point(50, 0), new Point(100, 50));
                //dc.DrawLine(drawingpen, new Point(0, 50), new Point(100, 50));

                dc.DrawRectangle(Brushes.Black, drawingpen, new Rect(25, 25, 50, 50));
            }

            DrawingCanva.AddVisual(visual);


            DrawingVisual visual2 = new DrawingVisual();

            using (DrawingContext dc = visual2.RenderOpen())
            {
                Pen drawingpen = new Pen(Brushes.Red, 1);
                dc.DrawLine(drawingpen, new Point(0, 0), new Point(DrawingCanva.ActualWidth, DrawingCanva.ActualHeight));
            }

            DrawingCanva.AddVisual(visual2);









            DrawingCanva.MouseMove += DrawingCanva_MouseMove;

            DrawingCanva.MouseDown += DrawingCanva_MouseDown;

            DrawingCanva.MouseWheel += DrawingCanva_MouseWheel;

            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawingVisual visual2 = (DrawingVisual)DrawingCanva.visuals[1];

            using (DrawingContext dc = visual2.RenderOpen())
            {
                Pen drawingpen = new Pen(Brushes.Red, 1);
                dc.DrawLine(drawingpen, new Point(0, 0), new Point(DrawingCanva.ActualWidth, DrawingCanva.ActualHeight));
            }
        }

        private void DrawingCanva_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //DrawingVisual visual = ((DrawingVisual)DrawingCanva.visuals[0]);

            //if (visual.Transform == null)
            //    visual.Transform = new ScaleTransform(1, 1, 50, 50);

            //((ScaleTransform)visual.Transform).ScaleX += e.Delta / 240.0;
            //((ScaleTransform)visual.Transform).ScaleY += e.Delta / 240.0;

            //visual.Transform.Transform(new Point());
        }

        private void DrawingCanva_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OldMousePosition = Mouse.GetPosition(DrawingCanva);

            //using (DrawingContext dc = visual.RenderOpen())
            //{
            //    dc.PushTransform(new TranslateTransform(0, 2));
            //}

            //using (DrawingContext dc = visual.RenderOpen())
            //{
            //    dc.PushTransform(new TranslateTransform(0.001, 0.001));
            //}
            //}
        }

        public Point OldMousePosition { get; set; }

        private void DrawingCanva_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            Point mousePoint = Mouse.GetPosition(DrawingCanva);

            DrawingVisual visual = ((DrawingVisual)DrawingCanva.visuals[0]);

            if (visual.Transform == null)
                visual.Transform = new TranslateTransform(0, 0);

            double dx = (OldMousePosition - mousePoint).X;
            double dy = (OldMousePosition - mousePoint).Y;
            
            ((TranslateTransform)visual.Transform).X -= dx;
            ((TranslateTransform)visual.Transform).Y -= dy;

            visual.Transform.Transform(new Point());

            OldMousePosition = mousePoint;
        }
    }



    public class DrawingCanvas : Panel
    {
        public List<Visual> visuals = new List<Visual>();

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);

            return hitResult.VisualHit as DrawingVisual;
        }




        private List<Visual> hits = new List<Visual>();

        public List<Visual> GetVisuals(Geometry region)
        {
            hits.Clear();

            GeometryHitTestParameters parameters = new GeometryHitTestParameters(region);

            HitTestResultCallback callback = new HitTestResultCallback(this.HitTestCallback);

            VisualTreeHelper.HitTest(this, null, callback, parameters);

            return hits;
        }

        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            GeometryHitTestResult geometryResult = (GeometryHitTestResult)result;
            DrawingVisual visual = result.VisualHit as DrawingVisual;
            if (visual != null &&
                geometryResult.IntersectionDetail == IntersectionDetail.FullyInside)
            {
                hits.Add(visual);
            }
            return HitTestResultBehavior.Continue;
        }

    }
}
