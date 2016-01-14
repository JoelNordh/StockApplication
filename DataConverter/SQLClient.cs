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

        public void GetDataFrom(Collection<DataClass> data, DateTime from, int stock, int timePeriod)
        {
            MySqlCommand myCommand = new MySqlCommand(string.Format("SELECT * FROM Quote where date > \"{0}\" AND Stock = \"{1}\" ORDER BY date", from.ToString(), stock), myConnection);
            MySqlDataReader myReader = myCommand.ExecuteReader();
            DateTime fifteenMinutes = new DateTime();
            double currentLow = double.MaxValue;
            double currentHigh = double.MinValue;

            while (myReader.Read())
            {
                DateTime testTime = DateTime.Parse(myReader["date"].ToString());
                if (fifteenMinutes == new DateTime())
                {
                    fifteenMinutes = DateTime.Parse(myReader["date"].ToString());
                    fifteenMinutes = fifteenMinutes.AddMinutes(timePeriod);

                    DataClass dataClass = new DataClass();

                    dataClass.closingPrice = double.Parse(myReader["price"].ToString());
                    dataClass.highPrice = double.Parse(myReader["price"].ToString());
                    dataClass.lowPrice = double.Parse(myReader["price"].ToString());

                    dataClass.date = RoundUp(DateTime.Parse(myReader["date"].ToString()), TimeSpan.FromMinutes(timePeriod));

                    data.Add(dataClass);

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
                    fifteenMinutes = RoundUp(DateTime.Parse(myReader["date"].ToString()), TimeSpan.FromMinutes(timePeriod)); 

                    DataClass dataClass = new DataClass();

                    dataClass.closingPrice = double.Parse(myReader["price"].ToString());
                    dataClass.highPrice = currentHigh;
                    dataClass.lowPrice = currentLow;
                    dataClass.date = RoundUp(DateTime.Parse(myReader["date"].ToString()), TimeSpan.FromMinutes(timePeriod));

                    //if (data.Count >= 2)
                    //    Console.WriteLine("Date period: " + (data[data.Count - 1].date - data[data.Count - 2].date).ToString());

                    data.Add(dataClass);

                    currentHigh = double.MinValue;
                    currentLow = double.MaxValue;

                    continue;
                }
            }
            myReader.Close();

        }
    }
}
