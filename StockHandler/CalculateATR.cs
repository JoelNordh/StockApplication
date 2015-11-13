using System;
using System.Collections.ObjectModel;
using DataConverter;
using System.Linq;

namespace StockHandler
{
    class CalculateATR : ICalculator
    {
        private Collection<StockData> ATR;
        private ObservableCollection<DataClass> priceList;
        private int period;

        public CalculateATR(Collection<StockData> ATR, ObservableCollection<DataClass> priceList, int period)
        {
            this.ATR = ATR;
            this.priceList = priceList;
            this.period = period;
        }

        public void Calculate()
        {
            double HighLow = priceList.Last().highPrice - priceList.Last().lowPrice;
            double HighClosingPrice, LowClosingPrice;
            double trueRange;  
            if(priceList.Count == 0)
            {
                return;
            }
            else if(priceList.Count == 1)
            {
                ATR.Add(new StockData(HighLow, priceList.Last().date));
                return;
            }
            HighClosingPrice = Math.Abs(priceList.Last().highPrice - priceList[priceList.Count - 2].closingPrice);
            LowClosingPrice = Math.Abs(priceList.Last().lowPrice - priceList[priceList.Count - 2].closingPrice);

            trueRange = Math.Max(HighLow, Math.Max(HighClosingPrice, LowClosingPrice));
            if(priceList.Count < period)
            {
                ATR.Add(new StockData((trueRange+ATR.Last().value)/ATR.Count, priceList.Last().date));
                return;
            }

            ATR.Add(new StockData((ATR.Last().value * (period - 1) + trueRange) / period, priceList.Last().date));
        }
    }
}