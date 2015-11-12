using DataConverter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHandler
{
    class CalculateRSI : ICalculator
    {
        private Collection<StockData> RSI;
        private Collection<StockData> priceList;
        private int RSIHistory;

        public CalculateRSI(Collection<StockData> RSI, Collection<StockData> priceList, int RSIHistory)
        {
            this.RSI = RSI;
            this.priceList = priceList;
            this.RSIHistory = RSIHistory;
        }

        public void Calculate()
        {
            double sumGain = 0;
            double sumLoss = 0;

            if (RSIHistory > priceList.Count)
            {
                return;
            }

            for (int i = priceList.Count - RSIHistory + 1; i < priceList.Count; i++)
            {
                var difference = priceList[i].value - priceList[i - 1].value;
                if (difference >= 0)
                {
                    sumGain += difference;
                }
                else
                {
                    sumLoss -= difference;
                }
            }

            if (sumGain == 0)
            {
                RSI.Add(new StockData(0, priceList.Last().date));
            }
            else if (Math.Abs(sumLoss) < 0.0000001)
            {
                RSI.Add(new StockData(100, priceList.Last().date));
            }

            var relativeStrength = sumGain / sumLoss;
            RSI.Add(new StockData(100.0 - (100.0 / (1 + relativeStrength)), priceList.Last().date));
        }
    }
}
