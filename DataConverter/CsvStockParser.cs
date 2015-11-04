using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public sealed class CsvStockParser : StockParser
    {
        public ObservableCollection<DataClass> Parse(StreamReader stream)
        {
            ObservableCollection<DataClass> list = new ObservableCollection<DataClass>();
            using (TextFieldParser parser = new TextFieldParser(stream))
            {
                parser.CommentTokens = new string[] { "#" };
                parser.SetDelimiters(new string[] { ";" });

                // Skip over header line.

                while (!parser.EndOfData)
                {
                    DataClass data = new DataClass();
                    string[] fields = parser.ReadFields();
                    if (!DateTime.TryParse(fields[0], out data.date))
                    {
                        continue;
                    }
                    data.highPrice = parseDouble(fields[1]);
                    data.lowPrice = parseDouble(fields[2]);
                    data.closingPrice = parseDouble(fields[3]);
                    data.avragePrice = parseDouble(fields[4]);
                    data.volume = parseInt(fields[5]);
                    data.turnover = parseDouble(fields[6]);
                    data.trades = parseInt(fields[7]);

                    list.Add(data);
                }
            }
            return list;
        }

        private static double parseDouble(string sValue)
        {
            if (sValue == "")
            {
                return -1;
            }
            try
            {
                return double.Parse(sValue);
            }
            catch
            {
                return -1;
            }
        }
        private static int parseInt(string sValue)
        {
            try
            {
                return int.Parse(sValue);
            }
            catch
            {
                return -1;
            }
        }
    }
}
