using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PaintSurface
{

    class Action : Tag
    {
        long item;

        public Action(long v, Point p)
        {
            this.value = v;
            this.position = p;
        }

        public long getItem()
        {
            return item;
        }

        public void setItem(long i)
        {
            item = i;
        }
    }
}
