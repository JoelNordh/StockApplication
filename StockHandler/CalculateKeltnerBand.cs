using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace StockHandler
{
    class CalculateKeltnerBand : ICalculator
    {
        private Collection<StockData> lowerKeltner;
        private Collection<StockData> upperKeltner;
        private Collection<StockData> ATR;
        private Collection<StockData> EMA;

        public CalculateKeltnerBand(Collection<StockData> lowerKeltner, Collection<StockData> upperKeltner, Collection<StockData> ATR, Collection<StockData> EMA)
        {
            this.lowerKeltner = lowerKeltner;
            this.upperKeltner = upperKeltner;
            this.ATR = ATR;
            this.EMA = EMA;
        }

        public void Calculate()
        {
            if(EMA.Count == 0)
            {
                return;
            }
            upperKeltner.Add(new StockData(EMA.Last().value + (2 * ATR.Last().value), ATR.Last().date));
            lowerKeltner.Add(new StockData(EMA.Last().value - (2 * ATR.Last().value), ATR.Last().date));
        }
    }
}