using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using DataHandler;
using System.Collections.Generic;

namespace TestProject1
{
    [TestClass]
    public class TestDataConverter
    {
        [TestMethod]
        public void TestReadCsv()
        {
            using (StreamReader stream = new StreamReader(@"historical.csv"))
            {
                //List<DataHandler.DataClass> list = DataHandler.CsvStockParser(stream);

                //Assert.IsNotNull(list);
            }
        }
    }
}
