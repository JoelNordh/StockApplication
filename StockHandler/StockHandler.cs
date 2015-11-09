using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataConverter;
using StockHandler;

namespace StockHandler
{
    public class StockClass
    {
        public DateTime date;
        public double value;

        public StockClass()
        {
        }

        public StockClass(double value, DateTime date)
        {
            this.value = value;
            this.date = date;
        }
    }

    public class StockHandler
    {
        CsvStockParser parser = new CsvStockParser();
        StockTrader trader;

        public ObservableCollection<DataClass> priceList;
        public ObservableCollection<StockClass> priceList2;
        public ObservableCollection<StockClass> movingAvrage20;
        public ObservableCollection<StockClass> movingAvrage50;
        public ObservableCollection<StockClass> movingAvrage100;
        public ObservableCollection<StockClass> UpperBolinger = new ObservableCollection<StockClass>();
        public ObservableCollection<StockClass> LowerBolinger = new ObservableCollection<StockClass>();
        public ObservableCollection<StockClass> RSI;

        public StockHandler(StockTrader trader)
        {
            priceList = new ObservableCollection<DataClass>();
            priceList2 = new ObservableCollection<StockClass>();

            movingAvrage20 = new ObservableCollection<StockClass>();
            movingAvrage50 = new ObservableCollection<StockClass>();
            movingAvrage100 = new ObservableCollection<StockClass>();
            RSI = new ObservableCollection<StockClass>();

            this.trader = trader;
        }

        public void addTestData(DataClass data) 
        {
            priceList.Add(data);
            
            priceList2.Add(new StockClass(data.closingPrice, data.date));

            string breakpointString = data.date.Date.ToString();

            CalculateMovingAvrage(20, movingAvrage20);
            CalculateMovingAvrage(50, movingAvrage50);
            CalculateMovingAvrage(100, movingAvrage100);
            CalculateRSI(14);
            if (movingAvrage20.Count > 0)
            {
                CalculateBolinger();
            }

            if(MarketAnalyzer.analyzeMA(movingAvrage20, movingAvrage50) == MarketAnalyzer.signal.BUYSIGNAL)
            {
                trader.buyStock(1, data);
            }
            else if(MarketAnalyzer.analyzeMA(movingAvrage20, movingAvrage50) == MarketAnalyzer.signal.SELLSIGNAL)
            {
                trader.sellStock(1, data);
            }
            MarketAnalyzer.signal signal = MarketAnalyzer.analyzeRSI(RSI);
            if (signal == MarketAnalyzer.signal.BUYSIGNAL)
            {
                trader.buyStock(1, data);
            }
            else if (signal == MarketAnalyzer.signal.SELLSIGNAL)
            {
                trader.sellStock(1, data);
            }
        }

        private void CalculateMovingAvrage(int avrage, ObservableCollection<StockClass> currentList)
        {
            ObservableCollection<StockClass> stockData = new ObservableCollection<StockClass>();
            double lastAvrage = 0;

             if(priceList.Count < avrage)
            {
                return;
            }

            lastAvrage = 0;
            for (int i = priceList.Count - avrage; i < priceList.Count; i++)
            {
                lastAvrage += priceList[i].closingPrice;
            }
            lastAvrage = lastAvrage / avrage;
            currentList.Add(new StockClass(lastAvrage, priceList.Last().date));
        }

        private void CalculateBolinger()
        {
            double deviationSquare = 0;
            for (int i = priceList.Count-20; i < priceList.Count; i++)
            {
                deviationSquare += Math.Pow(movingAvrage20.Last().value - priceList[i].closingPrice, 2);
            }

            deviationSquare = Math.Sqrt(deviationSquare / 20);

            UpperBolinger.Add(new StockClass(movingAvrage20.Last().value + (deviationSquare * 2), movingAvrage20.Last().date));
            LowerBolinger.Add(new StockClass(movingAvrage20.Last().value - (deviationSquare * 2), movingAvrage20.Last().date));
        }

        private void CalculateRSI(int RSIHistory)
        {
            double sumGain = 0;
            double sumLoss = 0;
            
            if(RSIHistory > priceList.Count)
            {
                return;
            }

            for (int i = priceList.Count - RSIHistory + 1; i < priceList.Count; i++)
            {
                var difference = priceList[i].closingPrice - priceList[i - 1].closingPrice;
                if (difference >= 0)
                {
                    sumGain += difference;
                }
                else
                {
                    sumLoss -= difference;
                }
            }
            if (sumGain == 0) RSI.Add(new StockClass(0, priceList.Last().date));
            if (Math.Abs(sumLoss) < 0.0000001) RSI.Add(new StockClass(100, priceList.Last().date));
            var relativeStrength = sumGain / sumLoss;
            RSI.Add(new StockClass(100.0 - (100.0 / (1 + relativeStrength)), priceList.Last().date));

            //RSI.Last().value = RSI.Last().value + 1000;
        }

    }
}

