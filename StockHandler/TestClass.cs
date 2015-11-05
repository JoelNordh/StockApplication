using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataConverter;
using System.Collections.ObjectModel;
using System.Threading;

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
        public ObservableCollection<DataClass> testList;
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
            testList = new ObservableCollection<DataClass>(testList.Reverse()); 
            i = 0;
        }

        public void nextData()
        {
            Thread.Sleep(1);
            OnStockDataAdded(new NewDataEventArgs(testList[i++]));

        }

        public bool HasMoreData()
        {
            if (i < testList.Count)
                return true;
            else
                return false;
        }
    }
}
