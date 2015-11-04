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
        TestClass testClass = new TestClass(new CsvStockParser());
        CsvStockParser parser = new CsvStockParser();

        ObservableCollection<DataClass> priceList;
        ObservableCollection<StockClass> movingAvrage20;
        ObservableCollection<StockClass> UpperBolinger = new ObservableCollection<StockClass>();
        ObservableCollection<StockClass> LowerBolinger = new ObservableCollection<StockClass>();

        public StockHandler()
        {
            testClass.StockDataAdded += GotNewStockData;
            priceList = parser.Parse(new System.IO.StreamReader("history.csv"));
        }

        private void GotNewStockData(object sender, NewDataEventArgs args)
        {
            priceList.Add(args.Data);
            Console.WriteLine(args.Data.closingPrice.ToString());
        }

        private ObservableCollection<StockClass> CalculateMovingAvrage(int avrage)
        {
            ObservableCollection<StockClass> stockData = new ObservableCollection<StockClass>();
            double lastAvrage = 0;
            for (int i = avrage; i < priceList.Count; i++)
            {
                lastAvrage = 0;
                for (int y = i - avrage; y < i; y++)
                {
                    lastAvrage += priceList[y].closingPrice;
                }
                lastAvrage = lastAvrage / avrage;

                stockData.Add(new StockClass(lastAvrage, priceList[i].date));
            }
            return stockData;
        }

        private void CalculateBolinger()
        {
            for (int i = 0; i < movingAvrage20.Count; i++)
            {
                double deviationSquare = 0;
                for (int y = 20 + i; y > i; y--)
                {
                    deviationSquare += Math.Pow(movingAvrage20[i].value - priceList[y].closingPrice, 2);
                }
                deviationSquare = Math.Sqrt(deviationSquare / 20);

                UpperBolinger.Add(new StockClass(movingAvrage20[i].value + (deviationSquare * 2), movingAvrage20[i].date));
                LowerBolinger.Add(new StockClass(movingAvrage20[i].value - (deviationSquare * 2), movingAvrage20[i].date));
            }
        }

        public ObservableCollection<DataClass> getPriceList()
        {
            return priceList;
        }

        /*public ObservableCollection<DataClass> getMovingAvrage20()
        {
            return movingAvrage20;
        }*/
    }
}
