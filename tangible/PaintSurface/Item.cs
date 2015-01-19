using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PaintSurface
{
    class Item : Tag
    {
        private List<long> actions;

        public Item(long v, Point p)
        {
            this.actions = new List<long>();
            this.value = v;
            this.position = p;
            Tag.id++;
        }

        public List<long> getActions()
        {
            return actions;
        }

        public void addAction(long v)
        {
            actions.Add(v);
        }
    }
}
