using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class SQLClient
    {
        SqlConnection myConnection;
        public SQLClient(string username, string password, string server, string database)
        {
            myConnection = new SqlConnection();
            

            myConnection.ConnectionString = string.Format("UID={0},3306;PWD={1};Address={2};Database={3}", username, password, server, database);

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
            SqlCommand command = new SqlCommand(string.Format("SELECT [id] FROM Stock where symbol = {0}", obj.symbol));
            SqlDataReader myReader = command.ExecuteReader();

            int result;

            int.TryParse((string)myReader["id"], out result);

            obj.SetStockID(result);

        }

        public void connect()
        {
            myConnection.Open();
        }

        public void GetDataFrom(Collection<DataClass> data, DateTime from)
        {
            SqlCommand myCommand = new SqlCommand(string.Format("SELECT * FROM Quote where date > {0}", from.ToString()), myConnection);
            SqlDataReader myReader = myCommand.ExecuteReader();

            while(myReader.Read())
            {

            }

        }
    }
}
