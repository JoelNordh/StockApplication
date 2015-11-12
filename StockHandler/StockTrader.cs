using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataConverter;

namespace StockHandler
{
    public class TradeEventArgs : EventArgs
    {
        public DataClass Data { get; }
        public double Balance { get; }

        public TradeEventArgs(double balance, DataClass data)
        {
            Data = data;
            Balance = balance;
        }
    }

    public delegate void TradeDelegate(object sender, TradeEventArgs args);

    public class StockTrader
    {
        private List<DataClass> possesion;
        double balance;
        public TradeDelegate StockSold;
        public TradeDelegate StockBought;

        public StockTrader()
        {
            possesion = new List<DataClass>();
            balance = 10000;
        }

        public void addMoney(double amount)
        {
            balance += amount;
        }

        public void buyStock(int amount, DataClass buyDetails)
        {
            if(possesion.Count > 0)
            {
                return;
            }

            DataClass purchase = buyDetails;

            possesion.Add(buyDetails);

            balance = balance * 0.99;

            OnStockBought(new TradeEventArgs(balance, buyDetails));
        }

        public StockData getLastBuy()
        {
            if (possesion.Count > 0)
            {
                return new StockData(possesion.Last().closingPrice, possesion.Last().date);
            }

            return null;
        }

        public void sellStock(int amount, DataClass sellDetails)
        {
            if (possesion.Count > 0)
            {
                balance = balance * sellDetails.closingPrice / possesion.Last().closingPrice;
                Console.WriteLine("Current balance " + balance.ToString());

                possesion.RemoveAt(0);

                OnStockSold(new TradeEventArgs(balance, sellDetails));
            }
        }

        protected virtual void OnStockSold(TradeEventArgs args)
        {
            if (StockSold != null)
            {
                StockSold(this, args);
            }
        }

        protected virtual void OnStockBought(TradeEventArgs args)
        {
            if (StockBought != null)
            {
                StockBought(this, args);
            }
        }

    }
}
