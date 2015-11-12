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
        public enum signal { NOSIGNAL, BUYSIGNAL, SELLSIGNAL, STOPLOSS};

        public static signal analyzeMA(Collection<StockData> movingAvrage20, Collection<StockData> movingAvrage50, Collection<StockData> movingAvrage100)
        {
            //if (movingAvrage100.Count >= 2)
            //{
            //    if(movingAvrage20.Last().value > movingAvrage50.Last().value && movingAvrage50.Last().value > movingAvrage100.Last().value)
            //    {
            //        return signal.MOVINGAVRAGEUP;
            //    }
            //    else if (movingAvrage20.Last().value < movingAvrage50.Last().value && movingAvrage50.Last().value < movingAvrage100.Last().value)
            //    {
            //        return signal.MOVINGAVRAGEDOWN;
            //    }
            //}
            return signal.NOSIGNAL;
        }

        static double highest;
        public static signal StopLoss(double current) //Gör om gör rätt
        {
            
            if(highest == 0)
            {
                highest = current;
                return signal.NOSIGNAL;
            }

            if(current > highest)
            {
                highest = current;
                return signal.NOSIGNAL;
            }

            if(current < highest*0.93)
            {
                return signal.STOPLOSS;
            }
            return signal.NOSIGNAL;
        }

        public static void ClearStopLoss()
        {
            highest = 0;
        }

        private enum state { BELOW30, ABOVE70, ABOVE50, BELOW50}
        private static state currentState = new state();

        const int lowerRSI = 27;
        const int higherRSI = 73;

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
                    if(RSI.Last().value > lowerRSI)
                    {
                        currentState = state.BELOW50;
                    }
                    if (RSI.Last().value > RSI[RSI.Count - 2].value)
                    {
                        currentSignal = signal.BUYSIGNAL;
                    }
                    break;
                case state.BELOW50:
                    if(RSI.Last().value < lowerRSI)
                    {
                        currentState = state.BELOW30;
                    }
                    else if(RSI.Last().value > 50)
                    {
                        currentState = state.ABOVE50;
                    }
                    break;
                case state.ABOVE50:
                    if (RSI.Last().value > higherRSI)
                    {
                        currentState = state.ABOVE70;
                    }
                    else if(RSI.Last().value < 50)
                    {
                        currentState = state.BELOW50;
                    }
                    break;
                case state.ABOVE70:
                    if(RSI.Last().value < higherRSI)
                    {
                        currentSignal = signal.SELLSIGNAL;
                        currentState = state.ABOVE50;
                    }
                    else
                    {
                        if(RSI.Last().value < RSI[RSI.Count-2].value)
                        {
                            currentSignal = signal.SELLSIGNAL;
                        }
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
