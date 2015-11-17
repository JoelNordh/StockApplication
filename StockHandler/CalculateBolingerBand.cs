using DataHandler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHandler
{
    public class CalculateBolingerBand : ICalculator
    {
        Collection<StockData> UpperBolinger;
        Collection<StockData> LowerBolinger;
        Collection<DataClass> priceList;
        Collection<StockData> movingAvrage;

        public CalculateBolingerBand(Collection<StockData> UpperBolinger, Collection<StockData> LowerBolinger, Collection<DataClass> priceList , Collection<StockData> movingAvrage)
        {
            this.UpperBolinger = UpperBolinger;
            this.LowerBolinger = LowerBolinger;
            this.priceList = priceList;
            this.movingAvrage = movingAvrage;
        }

        public void Calculate()
        {
            double deviationSquare = 0;
            if (priceList.Count - 20 < 0)
            {
                return;
            }
            for (int i = priceList.Count - 20; i < priceList.Count; i++)
            {
                deviationSquare += Math.Pow(movingAvrage.Last().value - priceList[i].closingPrice, 2);
            }

            deviationSquare = Math.Sqrt(deviationSquare / 20);

            UpperBolinger.Add(new StockData(movingAvrage.Last().value + (deviationSquare * 2), movingAvrage.Last().date));
            LowerBolinger.Add(new StockData(movingAvrage.Last().value - (deviationSquare * 2), movingAvrage.Last().date));
        }
    }
}
