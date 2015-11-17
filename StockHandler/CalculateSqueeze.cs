using DataHandler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StockHandler
{
    internal class CalculateSqueeze : ICalculator
    {
        private Collection<StockData> squeezeCurve;
        private Collection<StockData> squeezePoints;
        private Collection<StockData> noSqueezePoints;
        private Collection<StockData> lowerKeltinger;
        private Collection<StockData> lowerBolinger;
        private Collection<StockData> upperKeltinger;
        private Collection<StockData> upperBolinger;
        private Collection<StockData> SMA;
        private Collection<StockData> EMA;
        private Collection<DataClass> priceList;
        private int period;

        public CalculateSqueeze(Collection<StockData> squeezeCurve, Collection<StockData> squeezePoints, Collection<StockData> noSqueezePoints, Collection<StockData> lowerKeltinger, Collection<StockData> lowerBolinger, Collection<StockData> upperKeltinger, Collection<StockData> upperBolinger, Collection<StockData> SMA, Collection<StockData> EMA, Collection<DataClass> priceList,int period)
        {
            this.squeezeCurve = squeezeCurve;
            this.squeezePoints = squeezePoints;
            this.lowerKeltinger = lowerKeltinger;
            this.lowerBolinger = lowerBolinger;
            this.upperKeltinger = upperKeltinger;
            this.upperBolinger = upperBolinger;
            this.period = period;
            this.SMA = SMA;
            this.EMA = EMA;
            this.priceList = priceList;
            this.noSqueezePoints = noSqueezePoints;
        }

        public void Calculate()
        {
            if(upperKeltinger.Count < period || lowerKeltinger.Count < period)
            {
                return;
            }


            if (lowerBolinger.Last().value > lowerKeltinger.Last().value && upperBolinger.Last().value < upperKeltinger.Last().value)
            {
                //Squeeze ON
                squeezePoints.Add(new StockData(0, priceList.Last().date));
            }
            else if (lowerBolinger.Last().value < lowerKeltinger.Last().value && upperBolinger.Last().value > upperKeltinger.Last().value)
            {
                //Squeeze OFF
                noSqueezePoints.Add(new StockData(0, priceList.Last().date));
            }
            else
            {
                
            }

            double[] average = { highest(priceList.ToList().GetRange(upperKeltinger.Count - period, period)),
                lowest(priceList.ToList().GetRange(lowerKeltinger.Count - period, period))};

            squeezeCurve.Add(new StockData(priceList.Last().closingPrice - new double[] { (average.Average()), SMA.Last().value }.Average(), priceList.Last().date));

        }

        private double highest(List<DataClass> data)
        {
            return data.Max(x => x.highPrice);
        }

        private double lowest(List<DataClass> data)
        {
            return data.Min(x => x.lowPrice);
        }

        
    }
}