using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;



namespace WpfDrawingTest
{
    public delegate void PrimitiveMouseEventHabdler(Primitive sender, MouseButtonEventArgs e = null);



    public class Primitive : DrawingVisual
    {
        public event PrimitiveMouseEventHabdler MouseEnter;
        public event PrimitiveMouseEventHabdler MouseLeave;
        public event PrimitiveMouseEventHabdler MouseClick;
        public event PrimitiveMouseEventHabdler MouseDoubleClick;



        public Primitive()
        {
            
        }



        public virtual void ReDraw()
        {
            Draw();
        }

        protected virtual void Draw()
        {
        }



        public virtual void OnMouseEnter()
        {
            if (MouseEnter != null)
                MouseEnter(this);
        }

        public virtual void OnMouseLeave()
        {
            if (MouseLeave != null)
                MouseLeave(this);
        }

        public void OnMouseClick(MouseButtonEventArgs e)
        {
            if (e == null)
                throw new NullReferenceException();

            if (MouseClick != null)
                MouseClick(this, e);
        }

        public void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (e == null)
                throw new NullReferenceException();

            if (MouseDoubleClick != null)
                MouseDoubleClick(this, e);
        }
    }
}
