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
        public SQLClient(string username, string password, string server, string database)
        {
            myConnection = new MySqlConnection();
            

            myConnection.ConnectionString = string.Format("Address={2};UID={0};PWD={1};Database={3}", username, password, server, database);

            try
            {
                myConnection.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            OMX30 omx30 = new OMX30();

            omx30.Omx30.ForEach(getStockId);
        }

        private void getStockId(OMX30IdDate obj)
        {
            Console.WriteLine(string.Format("SELECT id FROM Stock where symbol = \"{0}\"", obj.symbol));
            MySqlCommand command = new MySqlCommand(string.Format("SELECT id FROM Stock where symbol = \"{0}\"", obj.symbol), myConnection);

            MySqlDataReader myReader;
            myReader = command.ExecuteReader();
            myReader.Read();
            int result;

            Console.WriteLine(myReader["id"].ToString());

            int.TryParse(myReader["id"].ToString(), out result);

            obj.SetStockID(result);

            myReader.Close();

        }

        public void connect()
        {
            myConnection.Open();
        }

        public void GetDataFrom(Collection<DataClass> data, DateTime from)
        {
            MySqlCommand myCommand = new MySqlCommand(string.Format("SELECT * FROM Quote where date > \"{0}\"", from.ToString()), myConnection);
            MySqlDataReader myReader = myCommand.ExecuteReader();

            while(myReader.Read())
            {

            }

        }
    }
}
