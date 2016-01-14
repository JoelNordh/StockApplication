using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHandler
{
    public class StockDataStorage
    {
        private Dictionary<string, Dictionary<int, Collection<StockData>>> data;

        public StockDataStorage()
        {
            this.data = new Dictionary<string, Dictionary<int, Collection<StockData>>>();
        }

        public void Add(string name, Collection<StockData> collection)
        {
            Add(name, 0, collection);
        }

        public void Add(string name, int value, Collection<StockData> collection)
        {
            Dictionary<int, Collection<StockData>> collectionData;
            if (data.ContainsKey(name))
            {
                collectionData = data[name];
            }
            else
            {
                collectionData = new Dictionary<int, Collection<StockData>>();
                
                data.Add(name, collectionData);
            }
            collectionData.Add(value, collection);
        }

        public Collection<StockData> Get(string name)
        {
            return Get(name, 0);
        }

        public Collection<StockData> Get(string name, int value)
        {
            return data[name][value];
        }

        public List<Collection<StockData>> GetRange(string name)
        {
            return data[name].Values.ToList();
        }

        public void Clear(string name)
        {
            Clear(name, 0);
        }

        public void Clear(string name, int value)
        {
            data[name][value].Clear();
        }

        public void ResetAll()
        {
            foreach(string key in data.Keys)
            {
                foreach(int numberKey in data[key].Keys)
                {
                    Clear(key, numberKey);
                }
            }
        }

    }
}
