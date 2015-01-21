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
        private bool putOnRightCase;


        public Action(long v, Point p)
        {
            this.value = v;
            this.position = p;
            this.putOnRightCase = false;
            Tag.id++;
        }

        public long getItem()
        {
            return item;
        }

        public void setItem(long i)
        {
            item = i;
        }

       

        public bool getPutInRightCase()
        {
            return putOnRightCase;
        }
       

        public void setPutInRightCase(bool p)
        {
            putOnRightCase = p;
        }
    }
}
