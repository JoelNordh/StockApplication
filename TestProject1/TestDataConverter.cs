using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using DataConverter;
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
                //List<DataConverter.DataClass> list = DataConverter.CsvStockParser(stream);

                //Assert.IsNotNull(list);
            }
        }
    }
}
