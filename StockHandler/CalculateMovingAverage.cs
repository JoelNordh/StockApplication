using DataConverter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHandler
{
    class CalculateMovingAverage : ICalculator
    {
        private Collection<StockData> movingAvrage;
        private Collection<DataClass> priceList;
        private int average;

        public CalculateMovingAverage(Collection<StockData> movingAverage, Collection<DataClass> priceList, int average)
        {
            this.movingAvrage = movingAverage;
            this.priceList = priceList;
            this.average = average;
        }

        public void Calculate()
        {
            double lastAvrage = 0;

            if (priceList.Count < average)
            {
                return;
            }

            for (int i = priceList.Count - average; i < priceList.Count; i++)
            {
                lastAvrage += priceList[i].closingPrice;
            }
            lastAvrage = lastAvrage / average;
            movingAvrage.Add(new StockData(lastAvrage, priceList.Last().date));
        }
    }
}
