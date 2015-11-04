using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using System.Collections.ObjectModel;
using StockHandler;
using DataConverter;

namespace StockApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {     
        StockHandler.TestClass testClass = new TestClass(new CsvStockParser());
        StockHandler.StockHandler stockClass = new StockHandler.StockHandler();

        private void plotData(ObservableCollection<StockClass> list, Brush pen, String Description)
        {
            IPointDataSource point = null;
            LineGraph line;
            

            EnumerableDataSource<StockClass> _edsSPP;
            _edsSPP = new EnumerableDataSource<StockClass>(list);
            _edsSPP.SetXMapping(p => dateAxis.ConvertToDouble(p.date));
            _edsSPP.SetYMapping(p => p.value);
            point = _edsSPP;

            line = new LineGraph(point);
            line.LinePen = new Pen(pen, 2);
            line.Description = new PenDescription(Description);
            
            plotter.Children.Add(line);
            plotter.FitToView();
        }

        public MainWindow()
        {
            InitializeComponent();

            testClass.StockDataAdded += GotNewStockData;

            plotData(toStockClass(stockClass.priceList, DataClass.priceChooser.CLOSINGPRICE), Brushes.Black, "Current Price");
            //plotData(stockClass.movingAvrage20, Brushes.BlueViolet, "MA 20");
            //plotData(stockClass.movingAvrage50, Brushes.Red, "MA 50");
            //plotData(stockClass.movingAvrage100, Brushes.Green, "MA 100");
            //plotData(stockClass.UpperBolinger, Brushes.Pink, "Upper Boliger");
            //plotData(stockClass.LowerBolinger, Brushes.Pink, "Upper Boliger");
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                //Fire new event with new data
                  testClass.nextData();
            }
        }

        public void GotNewStockData(object sender, NewDataEventArgs args)
        {
            stockClass.addTestData(args.Data);
            Console.WriteLine(args.Data.closingPrice.ToString());
        }


        public static ObservableCollection<StockClass> toStockClass(ObservableCollection<DataClass> data, DataClass.priceChooser choice)
        {
            ObservableCollection<StockClass> toPlot = new ObservableCollection<StockClass>();
            foreach (DataClass Item in data)
            {
                StockClass plotClass;
                switch (choice)
                {
                    case DataClass.priceChooser.AVRAGEPRICE:
                        plotClass = new StockClass(Item.avragePrice, Item.date);
                        break;
                    case DataClass.priceChooser.HIGHPRICE:
                        plotClass = new StockClass(Item.highPrice, Item.date);
                        break;
                    case DataClass.priceChooser.LOWPRICE:
                        plotClass = new StockClass(Item.lowPrice, Item.date);
                        break;
                    case DataClass.priceChooser.CLOSINGPRICE:
                        plotClass = new StockClass(Item.closingPrice, Item.date);
                        break;
                    default:
                        plotClass = new StockClass();
                        break;
                }
                toPlot.Add(plotClass);
            }
            return toPlot;
        }
    }
}
