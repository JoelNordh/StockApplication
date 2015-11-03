using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public class DataClass
    {
        public enum priceChooser { HIGHPRICE, LOWPRICE, CLOSINGPRICE, AVRAGEPRICE };

        public DateTime date;
        public priceChooser priceChoice;
        
        public double highPrice { get; set; }
        public double lowPrice { get; set; }
        public double closingPrice { get; set; }
        public double avragePrice { get; set; }
        public int volume { get; set; }
        public double turnover { get; set; }
        public int trades { get; set; }

        public DataClass()
        {
            this.date = new DateTime();
            this.highPrice = 0;
            this.lowPrice = 0;
            this.closingPrice = 0;
            this.avragePrice = 0;
            this.volume = 0;
            this.turnover = 0;
            this.trades = 0;
            priceChoice = priceChooser.AVRAGEPRICE;
        }

        
    }
}
