using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApplication
{
    public class PlotClass : StockHandler.StockData
    {
        public PlotClass()
        {
        }

        public PlotClass(double value, DateTime date)
        {
            this.value = value;
            this.date = date;
        }

        public void buyIndicator(double price)
        {

        }
        
        public void sellIndicator(double price)
        {

        }
    }
}
