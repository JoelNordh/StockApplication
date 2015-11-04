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

        public ObservableCollection<DataClass> priceList;
        public ObservableCollection<StockClass> movingAvrage20;
        public ObservableCollection<StockClass> movingAvrage50;
        public ObservableCollection<StockClass> movingAvrage100;
        public ObservableCollection<StockClass> UpperBolinger = new ObservableCollection<StockClass>();
        public ObservableCollection<StockClass> LowerBolinger = new ObservableCollection<StockClass>();

        public StockHandler()
        {
            priceList = new ObservableCollection<DataClass>();

            movingAvrage20 = CalculateMovingAvrage(20);
            movingAvrage50 = CalculateMovingAvrage(50);
            movingAvrage100 = CalculateMovingAvrage(100);

            CalculateBolinger();
        }

        public void addTestData(DataClass data)
        {
            priceList.Add(data);
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

    }
}
