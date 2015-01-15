using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace PaintSurface
{
    class Tag
    {
        protected bool put;
        protected Point position;
        protected long value;

        public Tag()
        {

        }
        
        public Tag(long v, Point p)
        {
            put = true;
            position = p;
            value = v;
        }


        public long getValue()
        {
            return value;
        }
        public Point getPosition()
        {
            return position;
        }
        public bool getPut()
        {
            return put;
        }


        public void setPosition(Point p)
        {
            position = p;
        }
        public void setPut(bool p)
        {
            put = p;
        }
        public void setValue(long v)
        {
            value = v;
        }

    }
}
