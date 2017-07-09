using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;




namespace WpfDrawingTest.Primitives
{
    public class Line : Primitive
    {
        public Point Point1 { get; private set; }

        public Point Point2 { get; private set; }

        public Pen Pen { get; private set; }



        public Line(Point point1, Point point2, Pen pen)
        {
            if (point1 == null || point2 == null || pen == null)
                throw new NullReferenceException();

            Point1 = point1;
            Point2 = point2;
            Pen = pen;
        }



        public void SetPosition(Point point1, Point point2)
        {
            if (point1 == null || point2 == null)
                throw new NullReferenceException();

            Point1 = point1;
            Point2 = point2;
        }

        protected override void Draw()
        {
            using (DrawingContext context = this.RenderOpen())
            {
                DrawLine(context, Pen);
            }
        }

        protected void DrawLine(DrawingContext context, Pen pen)
        {
            context.DrawLine(pen, Point1, Point2);
        }
    }
}