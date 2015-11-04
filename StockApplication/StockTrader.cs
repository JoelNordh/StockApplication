using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApplication
{
    class StockTrader
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

        public void buyStock(int amount)
        {

        }

        public void sellStock(int amount)
        {

        }
    }
}
