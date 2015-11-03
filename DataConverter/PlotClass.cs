using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public class PlotClass
    {
        public DateTime date;
        public double value;

        public PlotClass()
        {
        }

        public PlotClass(double value, DateTime date)
        {
            this.value = value;
            this.date = date;
        }
    }
}
