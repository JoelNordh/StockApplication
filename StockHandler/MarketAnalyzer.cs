using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHandler
{
    public class MarketAnalyzer
    {
        public enum signal { NOSIGNAL, BUYSIGNAL, SELLSIGNAL };

        public static signal analyzeMA(Collection<StockData> movingAvrageShort, Collection<StockData> movingAvrageLong)
        {
            //if (movingAvrageLong.Count >= 2)
            //{
            //    if (movingAvrageShort[movingAvrageShort.Count - 2].value < movingAvrageLong[movingAvrageLong.Count - 2].value && movingAvrageShort.Last().value >= movingAvrageLong.Last().value)
            //    {
            //        return signal.BUYSIGNAL;
            //    }
            //    else if (movingAvrageShort[movingAvrageShort.Count - 2].value > movingAvrageLong[movingAvrageLong.Count - 2].value && movingAvrageShort.Last().value <= movingAvrageLong.Last().value)
            //    {
            //        return signal.SELLSIGNAL;
            //    }
            //}
            return signal.NOSIGNAL;
        }


        private enum state { BELOW30, ABOVE70, ABOVE50, BELOW50}
        private static state currentState = new state();

        public static signal analyzeRSI(Collection<StockData> RSI)
        {
            if (RSI.Count < 1)
            {
                currentState = state.BELOW50;
                return signal.NOSIGNAL;
            }
            signal currentSignal = signal.NOSIGNAL;
            switch(currentState)
            {

                case state.BELOW30:
                    if(RSI.Last().value > 30)
                    {
                        currentSignal = signal.BUYSIGNAL;
                        currentState = state.BELOW50;
                    }
                    break;
                case state.BELOW50:
                    if(RSI.Last().value < 30)
                    {
                        currentState = state.BELOW30;
                    }
                    else if(RSI.Last().value > 50)
                    {
                        currentState = state.ABOVE50;
                    }
                    break;
                case state.ABOVE50:
                    if (RSI.Last().value > 70)
                    {
                        currentSignal = signal.SELLSIGNAL;
                        currentState = state.ABOVE70;
                    }
                    else if(RSI.Last().value < 50)
                    {
                        currentState = state.BELOW50;
                    }
                    break;
                case state.ABOVE70:
                    if(RSI.Last().value < 70)
                    {
                        currentState = state.ABOVE50;
                    }
                    break;
                default:
                    currentState = state.ABOVE50;
                    break;

            }

            return currentSignal;
        }

    }
}
