using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WpfDrawingTest.VisualObjectModel;



namespace WpfDrawingTest
{
    


    public partial class MainWindow : Window
    {
        private Point _lastMousePos = new Point();

        //private float _scale = 1;

        public Pen SpanPen { get; set; }

        public Pen SpanHitArea { get; set; }

        public Pen SpanHitPen { get; set; }



        public MainWindow()
        {
            InitializeComponent();

            InitPens();

            InitCanvas();

            InitGraph();
            
            this.MouseMove += MainWindow_MouseMove;

            this.MouseDown += MainWindow_MouseDown;

            this.MouseWheel += MainWindow_MouseWheel;

            this.MouseDoubleClick += MainWindow_MouseDoubleClick;
        }

        private void MainWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Span primitive = Canva.OnMouseClick(e, true) as Span;

            //if (primitive == null)
            //    return;

            //MessageBox.Show(primitive.Id.ToString());

            Canva.OnMouseClick(e, true);
        }

        private void OnMouseDrag()
        {
            Point mousePos = Mouse.GetPosition(this);

            Vector offset = mousePos - _lastMousePos;

            Canva.Translate(offset.X, offset.Y);

            _lastMousePos = mousePos;
        }

        private void OnMouseMove()
        {
            Canva.OnMouseMove();
        }

        private void OnMouseDown()
        {
            _lastMousePos = Mouse.GetPosition(this);
        }

        private void OnMouseWheel(float delta)
        {
            //одно деление скрола
            //int oneDiv = 120;

            float offset = 1.5f;

            if (delta < 0)
                offset = 1 / 1.5f;

            Canva.Scale(offset);
        }

        private void InitCanvas()
        {
            Canva.Width = this.Width;
            Canva.Height = this.Height;
            Canva.AddBackgroundPrimitive();
        }

        private void InitGraph()
        {
            AddSpan(new Point(0, 0), new Point(50, 50), 1);
            AddSpan(new Point(50, 50), new Point(120, 90), 2);
            AddSpan(new Point(120, 90), new Point(80, 200), 3);
            AddSpan(new Point(80, 200), new Point(250, 250), 4);
        }

        private void InitPens()
        {
            SpanPen = new Pen(Brushes.Black, 3);

            SpanHitPen = new Pen(Brushes.Green, 6);

            SolidColorBrush spanHitAreaBrush = new SolidColorBrush(Colors.Red);
            spanHitAreaBrush.Opacity = 0;

            SpanHitArea = new Pen(spanHitAreaBrush, 40);
        }

        private void AddSpan(Point point1, Point point2, int id)
        {
            Span spanVisual = new Span(point1, point2, SpanPen, SpanHitArea, SpanHitPen);

            spanVisual.Id = id;

            spanVisual.ReDraw();

            Canva.AddVisual(spanVisual);

            //test
            spanVisual.MouseDoubleClick += SpanVisual_MouseDoubleClick;
        }

        private void SpanVisual_MouseDoubleClick(Primitive sender, MouseButtonEventArgs e = null)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                MessageBox.Show(((Span)sender).Id.ToString());
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

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                OnMouseDown();
        }

        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            OnMouseWheel(e.Delta);
        }
    }   
}