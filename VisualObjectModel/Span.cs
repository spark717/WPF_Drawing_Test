using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;




namespace WpfDrawingTest.VisualObjectModel
{
    public class Span : Primitives.Line
    {
        public int Id { get; set; }

        public Pen HitArea { get; set; }

        public Pen HitPen { get; set; }      



        public Span(Point point1, Point point2, Pen pen, Pen hitArea, Pen hitPen) : base(point1, point2, pen)
        {
            if (hitArea == null || hitPen == null)
                throw new NullReferenceException();

            HitArea = hitArea;
            HitPen = hitPen;
        }

        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            Draw();
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();

            DrawHit();
        }

        protected override void Draw()
        {
            using (DrawingContext context = RenderOpen())
            {
                DrawLine(context, HitArea);

                DrawLine(context, Pen);            
            }
        }

        private void DrawHit()
        {
            using (DrawingContext context = RenderOpen())
            {
                DrawLine(context, HitArea);

                DrawLine(context, HitPen);

                double radius = HitPen.Thickness / 8;

                context.DrawEllipse(HitPen.Brush, HitPen, Point1, radius, radius);

                context.DrawEllipse(HitPen.Brush, HitPen, Point2, radius, radius);
            }
        }      
    }
}
