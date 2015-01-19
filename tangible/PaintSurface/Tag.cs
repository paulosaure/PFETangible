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
        protected static int id = 0;

        public Tag()
        {
          
        }
        
        public Tag(long v, Point p)
        {
            this.put = true;
            this.position = p;
            this.value = v;
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

        public int getId()
        {
            return id;
        }

        public void setPosition(Point p)
        {
            position = p;
        }
        public void setPut(bool p)
        {
            put = p;
        }
    }
}
