using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public interface StockParser
    {
        ObservableCollection<DataClass> Parse(StreamReader stream);

        
    }
}
