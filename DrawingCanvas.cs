using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;



namespace WpfDrawingTest
{
    public delegate void DrawingCanvasMouseEventHandler(Primitive primitive);


    public class DrawingCanvas : FrameworkElement
    {
        private ContainerVisual _hitableContainer = new ContainerVisual();

        private Primitive _lastHitPrimitive;

        private BackgroundPrimitive _backgroundPrimitive;

        private TranslateTransform _translate = new TranslateTransform(0, 0);


        public event DrawingCanvasMouseEventHandler MouseEnterPrimitive;

        public event DrawingCanvasMouseEventHandler MouseLeavePrimitive;

        public event DrawingCanvasMouseEventHandler MouseClickPrimitive;

        public event DrawingCanvasMouseEventHandler MouseDoubleClickPrimitive;


        protected override int VisualChildrenCount
        {
            get
            {
                return 2;
            }
        }



        public DrawingCanvas()
        {
            base.AddVisualChild(_hitableContainer);
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 1)
                return _hitableContainer;
            else
                return _backgroundPrimitive;
        }

        public void AddVisual(Primitive visual)
        {
            _hitableContainer.Children.Add(visual);
        }

        public Primitive OnMouseMove()
        {
            Point mousePos = Mouse.GetPosition(this);

            Primitive primitive = GetHitTestPrimitive(mousePos);

            if (primitive == null)
                return null;

            if (primitive != _lastHitPrimitive)
            {
                primitive.OnMouseEnter();

                _lastHitPrimitive.OnMouseLeave();

                _lastHitPrimitive = primitive;
            }

            return primitive;
        }

        public Primitive OnMouseClick(MouseButtonEventArgs e, bool doubleClick)
        {
            Point mousePos = Mouse.GetPosition(this);

            Primitive primitive = GetHitTestPrimitive(mousePos);

            if (primitive == null)
                return null;

            if (doubleClick)
                primitive.OnMouseDoubleClick(e);
            else
                primitive.OnMouseClick(e);

            return primitive;
        }

        private Primitive GetHitTestPrimitive(Point point)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, point);

            if (hitTestResult == null)
                return null;

            Primitive primitive = hitTestResult.VisualHit as Primitive;

            return primitive;
        }

        public void AddBackgroundPrimitive()
        {
            Rect rectangle = new Rect(0, 0, this.Width, this.Height);

            BackgroundPrimitive background = new BackgroundPrimitive(rectangle);

            background.ReDraw();

            base.AddVisualChild(background);

            _lastHitPrimitive = background;

            _backgroundPrimitive = background;
        }

        public void Translate(double offsetX, double offsetY)
        {
            _translate.X += offsetX;
            _translate.Y += offsetY;

            _hitableContainer.Transform = _translate;
        }

        public void Scale(float scale)
        {
            foreach (Primitives.Line line in _hitableContainer.Children)
            {
                Point newPoint1 = new Point(
                    line.Point1.X * scale,
                    line.Point1.Y * scale);

                Point newPoint2 = new Point(
                    line.Point2.X * scale,
                    line.Point2.Y * scale);

                line.SetPosition(newPoint1, newPoint2);

                line.ReDraw();            
            };

            Point mousePos = Mouse.GetPosition(this);

            double offsetX = (mousePos.X - _translate.X) * (1 - scale);
            double offsetY = (mousePos.Y - _translate.Y) * (1 - scale);

            Translate(offsetX, offsetY);
        }
    }
}
