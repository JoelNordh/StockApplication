using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataConverter;

namespace StockHandler
{
    public class NewTradeEventArgs : EventArgs
    {
        public DataClass Data { get; }
        public enum TRADE { BUY, SELL }
        TRADE CurrentTrade { get; }
        double Balance;

        public NewTradeEventArgs(double balance, DataClass data, TRADE trade)
        {
            Data = data;
            CurrentTrade = trade;
            Balance = balance;
        }
    }

    public delegate void NewTradeDelegate(object sender, NewTradeEventArgs args);

    public class StockTrader
    {
        List<DataConverter.DataClass> possesion;
        double balance;
        public NewTradeDelegate StockTraded;
        public enum TRADE { BUY, SELL }

        public StockTrader()
        {
            possesion = new List<DataConverter.DataClass>();
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

            balance = balance * 1;

            OnStockTrade(new NewTradeEventArgs(balance, buyDetails, NewTradeEventArgs.TRADE.BUY));
        }

        public void sellStock(int amount, DataClass sellDetails)
        {
            if (possesion.Count > 0)
            {
                balance = balance * sellDetails.closingPrice / possesion.Last().closingPrice;
                Console.WriteLine("Current balance " + balance.ToString());

                possesion.RemoveAt(0);

                OnStockTrade(new NewTradeEventArgs(balance, sellDetails, NewTradeEventArgs.TRADE.SELL));
            }
        }

        protected virtual void OnStockTrade(NewTradeEventArgs args)
        {
            if (StockTraded != null)
            {
                StockTraded(this, args);
            }
        }

    }
}
