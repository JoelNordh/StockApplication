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
    public class StockData
    {
        public DateTime date;
        public double value;

        public StockData()
        {
        }

        public StockData(double value, DateTime date)
        {
            this.value = value;
            this.date = date;
        }
    }

    public class StockHandler
    {
        CsvStockParser parser = new CsvStockParser();
        StockTrader trader;
        StockDataStorage stockDataStorage;
        

        public ObservableCollection<DataClass> priceList;

        //public ObservableCollection<StockData> UpperBolinger = new ObservableCollection<StockData>();
        //public ObservableCollection<StockData> LowerBolinger = new ObservableCollection<StockData>();

        public List<ICalculator> Calculators;

        public StockHandler(StockTrader trader, StockDataStorage stockDataStorage)
        {
            priceList = new ObservableCollection<DataClass>();

            this.stockDataStorage = stockDataStorage;

            stockDataStorage.Add(IdentifierConstants.MOVING_AVERAGE, 20, new ObservableCollection<StockData>());
            stockDataStorage.Add(IdentifierConstants.MOVING_AVERAGE, 50, new ObservableCollection<StockData>());
            stockDataStorage.Add(IdentifierConstants.MOVING_AVERAGE, 100, new ObservableCollection<StockData>());
            stockDataStorage.Add(IdentifierConstants.RSI, new ObservableCollection<StockData>());
            stockDataStorage.Add(IdentifierConstants.PRICE_LIST, new ObservableCollection<StockData>());
            stockDataStorage.Add(IdentifierConstants.UPPERBOLINGER, new ObservableCollection<StockData>());
            stockDataStorage.Add(IdentifierConstants.LOWERBOLINGER, new ObservableCollection<StockData>());

            Calculators = new List<ICalculator>();

            Array.ForEach(new int[] { 20, 50, 100 }, x => Calculators.Add(new CalculateMovingAverage(stockDataStorage.Get(IdentifierConstants.MOVING_AVERAGE, x), priceList, x)));
            Calculators.Add(new CalculateRSI(stockDataStorage.Get(IdentifierConstants.RSI), stockDataStorage.Get(IdentifierConstants.PRICE_LIST), 14));
            Calculators.Add(new CalculateBolingerBand(stockDataStorage.Get(IdentifierConstants.UPPERBOLINGER),
                stockDataStorage.Get(IdentifierConstants.LOWERBOLINGER), priceList, stockDataStorage.Get(IdentifierConstants.MOVING_AVERAGE, 20)));

            this.trader = trader;
        }

        public void addTestData(DataClass data) 
        {
            priceList.Add(data);

            stockDataStorage.Get(IdentifierConstants.PRICE_LIST).Add(new StockData(data.closingPrice, data.date));

            Calculators.ForEach(c => c.Calculate());

            MarketAnalyzer.signal signal = MarketAnalyzer.analyzeRSI(stockDataStorage.Get(IdentifierConstants.RSI));
            if (signal == MarketAnalyzer.signal.SELLSIGNAL || MarketAnalyzer.StopLoss(data.closingPrice) == MarketAnalyzer.signal.STOPLOSS)
            {
                trader.sellStock(1, data);
                MarketAnalyzer.ClearStopLoss();
            }
            else if (signal == MarketAnalyzer.signal.BUYSIGNAL)
            {
                trader.buyStock(1, data);
            }
        }
    }
}

