using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataHandler;
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
        //TEST
        SQLClient sqlClient;

        protected virtual void OnStockDataAdded(NewDataEventArgs args)
        {
            if(StockDataAdded != null)
            {
                StockDataAdded(this, args);
            }
        }

        public TestClass(StockParser parser)
        {
            testList = new ObservableCollection<DataClass>();
            sqlClient = new SQLClient("finance", "financePass", "localhost", "finance"); //axelnordh.ddns.net
            sqlClient.GetDataFrom(testList, new DateTime(2015, 11, 10));

            testList = new ObservableCollection<DataClass>(testList.Reverse()); 
            i = 0;
        }

        public void nextData()
        {
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
