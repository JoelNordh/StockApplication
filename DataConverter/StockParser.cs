using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public interface StockParser
    {
        List<DataClass> Parse(StreamReader stream);

        
    }
}
