using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataConverter;

namespace StockHandler
{
    public class StockTrader
    {
        List<DataConverter.DataClass> possesion;
        double balance;

        public StockTrader()
        {
            possesion = new List<DataConverter.DataClass>();
            balance = 0;
        }

        public void addMoney(double amount)
        {
            balance += amount;
        }

        public void buyStock(int amount, DataClass buyDetails)
        {
            DataClass purchase = buyDetails;

            possesion.Add(buyDetails);
        }

        public void sellStock(int amount, DataClass sellDetails)
        {
            possesion.Remove(sellDetails);
        }
    }
}
