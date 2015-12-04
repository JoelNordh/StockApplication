using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DataHandler
{
    public class SQLClient
    {
        MySqlConnection myConnection;
        public OMX30 omx30 = new OMX30();
        public SQLClient(string username, string password, string server, string database)
        {
            myConnection = new MySqlConnection();
            

            myConnection.ConnectionString = string.Format("Address={2};UID={0};PWD={1};Database={3}", username, password, server, database);
            Console.WriteLine(myConnection.ConnectionString);
            try
            {
                myConnection.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            

            omx30.Omx30.ForEach(getStockId);
        }

        private void getStockId(OMX30.OMX30IdDate obj)
        {
            MySqlCommand command = new MySqlCommand(string.Format("SELECT id FROM Stock where symbol = \"{0}\"", obj.symbol), myConnection);

            MySqlDataReader myReader;
            myReader = command.ExecuteReader();
            myReader.Read();
            int result;
            
            int.TryParse(myReader["id"].ToString(), out result);

            obj.SetStockID(result);

            myReader.Close();

        }

        public void connect()
        {
            myConnection.Open();
        }

        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        public void GetDataFrom(Collection<DataClass> data, DateTime from, string stock)
        {
            List<DataClass> temporaryList = new List<DataClass>();
            foreach (OMX30.OMX30IdDate item in omx30.Omx30)
            {
                MySqlCommand myCommand = new MySqlCommand(string.Format("SELECT * FROM Quote where date > \"{0}\" AND Stock = \"{1}\" ORDER BY date", from.ToString(), item.StockID), myConnection);
                MySqlDataReader myReader = myCommand.ExecuteReader();
                DateTime fifteenMinutes = new DateTime();
                double currentLow = double.MaxValue;
                double currentHigh = double.MinValue;

                while (myReader.Read())
                {
                    if(fifteenMinutes == new DateTime())
                    {
                        fifteenMinutes = DateTime.Parse(myReader["date"].ToString());
                        fifteenMinutes = fifteenMinutes.AddMinutes(15);

                        DataClass dataClass = new DataClass();

                        dataClass.closingPrice = double.Parse(myReader["price"].ToString()) * (item.weight / 100);
                        dataClass.highPrice = double.Parse(myReader["price"].ToString()) * (item.weight / 100);
                        dataClass.lowPrice = double.Parse(myReader["price"].ToString()) * (item.weight / 100);

                        dataClass.date = RoundUp(DateTime.Parse(myReader["date"].ToString()), TimeSpan.FromMinutes(15));

                        temporaryList.Add(dataClass);

                        continue;
                    }

                    if(double.Parse(myReader["price"].ToString()) < currentLow)
                    {
                        currentLow = double.Parse(myReader["price"].ToString());
                    }
                    if(double.Parse(myReader["price"].ToString()) > currentHigh)
                    {
                        currentHigh = double.Parse(myReader["price"].ToString());
                    }

                    if(DateTime.Parse(myReader["date"].ToString()) >= fifteenMinutes)
                    {
                        fifteenMinutes = RoundUp(DateTime.Parse(myReader["date"].ToString()).AddMinutes(15), TimeSpan.FromMinutes(15)); 

                        DataClass dataClass = new DataClass();

                        dataClass.closingPrice = double.Parse(myReader["price"].ToString()) * (item.weight / 100);
                        dataClass.highPrice = currentHigh * (item.weight / 100);
                        dataClass.lowPrice = currentLow * (item.weight / 100);
                        dataClass.date = RoundUp(DateTime.Parse(myReader["date"].ToString()), TimeSpan.FromMinutes(15));

                        temporaryList.Add(dataClass);

                        currentHigh = double.MinValue;
                        currentLow = double.MaxValue;

                        continue;
                    }
                }
                myReader.Close();
            }

            temporaryList = temporaryList.OrderBy(x => x.date).ToList();
            DataClass previousItem = temporaryList[0];
            DataClass addItem = new DataClass();
            foreach (DataClass item in temporaryList)
            {
                if(item.date == previousItem.date)
                {
                    addItem.highPrice += item.highPrice;
                    addItem.lowPrice += item.lowPrice;
                    addItem.closingPrice += item.closingPrice;
                    addItem.date = item.date;
                }
                else
                {
                    data.Add(addItem);

                    addItem = new DataClass();
                }
                previousItem = item;
            }

        }
    }
}
