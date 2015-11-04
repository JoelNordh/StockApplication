using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataConverter;

namespace StockHandler
{
    public class NewDataEventArgs : EventArgs
    {
        public DataClass Data { get; }

        public NewDataEventArgs(DataClass data)
        {
            Data = data;
        }
    }

    public delegate void NewDataDelegate(object sender, NewDataEventArgs args);
    
    public class TestClass
    {
        List<DataClass> testList;
        int i;
        public NewDataDelegate StockDataAdded;

        

        protected virtual void OnStockDataAdded(NewDataEventArgs args)
        {
            if(StockDataAdded != null)
            {
                StockDataAdded(this, args);
            }
        }

        public TestClass(StockParser parser)
        {
            testList = parser.Parse(new System.IO.StreamReader("history.csv"));
            testList.Reverse(); 
            i = 0;
        }

        public void nextData()
        {
            OnStockDataAdded(new NewDataEventArgs(testList[i++]));
        }

        
    }
}
