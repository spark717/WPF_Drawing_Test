using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfDrawingTest
{
    public class BackgroundPrimitive : Primitive
    {
        public Rect Rectangle { get; set; }



        public BackgroundPrimitive(Rect rectangle)
        {
            if (rectangle == null)
                throw new NullReferenceException();

            Rectangle = rectangle;
        }



        protected override void Draw()
        {
            using (DrawingContext context = this.RenderOpen())
            {
                context.DrawRectangle(Brushes.White, new Pen(Brushes.Red, 1), Rectangle);
            }
        }
    }
}
