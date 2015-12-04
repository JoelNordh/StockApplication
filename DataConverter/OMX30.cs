using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class OMX30
    {
        public List<OMX30IdDate> Omx30;
        
        public OMX30()
        {
            Omx30 = new List<OMX30IdDate>();

            Omx30.Add(new OMX30IdDate("ABB Ltd", "ABB", 2.55));
            Omx30.Add(new OMX30IdDate("Alfa Laval", "ALFA", 1.61));
            Omx30.Add(new OMX30IdDate("Assa Abloy", "ASSA-B", 4.19));
            Omx30.Add(new OMX30IdDate("AstraZeneca", "AZN", 2.09));
            Omx30.Add(new OMX30IdDate("Atlas Copco A", "ATCO-A", 5.68));
            Omx30.Add(new OMX30IdDate("Atlas Copco B", "ATCO-B", 2.33));
            Omx30.Add(new OMX30IdDate("Boliden", "BOL", 1.22));
            Omx30.Add(new OMX30IdDate("Electrolux", "ELUX-B", 1.85));
            Omx30.Add(new OMX30IdDate("Ericsson Telefonab L M", "ERIC-B", 6.85));
            Omx30.Add(new OMX30IdDate("Getinge", "GETI-B", 1.11));
            Omx30.Add(new OMX30IdDate("Hennes & Mauritz", "HM-B", 11.94));
            Omx30.Add(new OMX30IdDate("Investor", "INVE-B", 3.81));
            Omx30.Add(new OMX30IdDate("Kinnevik", "KINV-B", 1.67));
            Omx30.Add(new OMX30IdDate("Lundin Petroleum", "LUPE", 1.03));
            Omx30.Add(new OMX30IdDate("Modern Times Group", "MTG-B", 0.42));
            Omx30.Add(new OMX30IdDate("Nordea", "NDA-SEK", 10.58));
            Omx30.Add(new OMX30IdDate("Sandvik", "SAND", 3.24));
            Omx30.Add(new OMX30IdDate("SCA", "SCA-B", 3.22));
            Omx30.Add(new OMX30IdDate("SEB", "SEB-A", 5.65));
            Omx30.Add(new OMX30IdDate("Securitas", "SECU-B", 1.07));
            Omx30.Add(new OMX30IdDate("Skanska", "SKA-B", 1.55));
            Omx30.Add(new OMX30IdDate("SKF", "SKF-B", 2.10));
            Omx30.Add(new OMX30IdDate("SSAB", "SSAB-A", 0.38));
            Omx30.Add(new OMX30IdDate("Swedbank", "SWED-A", 5.41));
            Omx30.Add(new OMX30IdDate("Swedish Match", "SWMA", 1.27));
            Omx30.Add(new OMX30IdDate("Svenska Handelsbanken", "SHB-A", 5.91));
            Omx30.Add(new OMX30IdDate("Tele2", "TEL2-B", 1.17));
            Omx30.Add(new OMX30IdDate("Telia Sonera", "TLSN", 5.54));
            Omx30.Add(new OMX30IdDate("Volvo Group", "VOLV-B", 4.56));
        }
        public class OMX30IdDate
        {
            public string Name { get; set; }
            public string symbol { get; set; }
            string type;
            public double weight;
            public int StockID { get; set; }

            public OMX30IdDate(string Name, string symbol, double weight)
            {
                this.Name = Name;
                this.symbol = symbol;
                this.weight = weight;
            }

            public void SetStockID(int ID)
            {
                StockID = ID;
            }

            public void SetType(string type)
            {
                this.type = type;
            }
        }
    }
}
