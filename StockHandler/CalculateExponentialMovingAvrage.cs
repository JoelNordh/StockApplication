using System;
using System.Collections.ObjectModel;
using DataConverter;
using System.Linq;

namespace StockHandler
{
    class CalculateExponentialMovingAvrage : ICalculator
    {
        private Collection<StockData> exponentialMovingAvrage;
        private Collection<DataClass> priceList;
        private Collection<StockData> SMA;
        private int average;

        public CalculateExponentialMovingAvrage(Collection<StockData> exponentialMovingAvrage, Collection<StockData> SMA, ObservableCollection<DataClass> priceList, int average)
        {
            this.exponentialMovingAvrage = exponentialMovingAvrage;
            this.SMA = SMA;
            this.average = average;
            this.priceList = priceList;
        }

        public void Calculate()
        {
            double sumMA = 0;
            double multiplier;
            if(SMA.Count < average)
            {
                return;
            }

            for(int i=SMA.Count - average; i < SMA.Count; i++)
            {
                sumMA += SMA[i].value;
            }
            sumMA /= average;

            multiplier = (2.0 / (average + 1.0));

            if (exponentialMovingAvrage.Count == 0)
            {
                exponentialMovingAvrage.Add(new StockData(priceList.Last().closingPrice * multiplier, priceList.Last().date));
            }
            else
            {
                exponentialMovingAvrage.Add(new StockData(
                    (priceList.Last().closingPrice - exponentialMovingAvrage.Last().value) * multiplier + exponentialMovingAvrage.Last().value
                    , priceList.Last().date));
            }
        }
    }
}