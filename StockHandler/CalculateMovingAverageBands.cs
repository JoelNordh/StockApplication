using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHandler
{
    public class CalculateMovingAverageBands : ICalculator
    {
        private Collection<StockData> movingAvrage100;
        private Collection<StockData> movingAvrageUpper;
        private Collection<StockData> movingAvrageLower;

        public CalculateMovingAverageBands(Collection<StockData> movingAvrageLower, Collection<StockData> movingAvrageUpper, Collection<StockData> movingAvrage100)
        {
            this.movingAvrage100 = movingAvrage100;
            this.movingAvrageLower = movingAvrageLower;
            this.movingAvrageUpper = movingAvrageUpper;
        }

        public void Calculate()
        {
            if (movingAvrage100.Count > 0)
            {
                movingAvrageUpper.Add(new StockData(movingAvrage100.Last().value + 100, movingAvrage100.Last().date));
                movingAvrageLower.Add(new StockData(movingAvrage100.Last().value - 100, movingAvrage100.Last().date));
            }
        }
    }
}
