using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using StockHandler;
using DataConverter;

namespace StockApplication
{
    class DataTestTimer
    {
        private Timer time;
        private int period;
        StockHandler.TestClass testClass;

        public DataTestTimer(int period, TestClass testClass)
        {
            time = new Timer(new TimerCallback(TimerTick));
            this.period = period;
            this.testClass = testClass;
        }

        public void StartTimer()
        {
            time.Change(0, period);
        }

        public void StopTimer()
        {
            time.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void TimerTick(object state)
        {
            if (testClass.HasMoreData())
            {
                testClass.nextData();
            }
        }
    }
}
