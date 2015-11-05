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
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StockApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {     
        StockHandler.TestClass testClass = new TestClass(new CsvStockParser());
        StockHandler.StockHandler stockClass = new StockHandler.StockHandler();
        StockHandler.StockTrader stockTrader = new StockTrader();
        PropertyClass propertyChanger = new PropertyClass();



        private void plotData(ref ObservableCollection<StockClass> list, Brush pen, String Description)
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
            
            //testClass.StockDataAdded += GotNewStockData;

            stockTrader.StockTraded += plotTradedStock;

            this.DataContext = propertyChanger;

            plotData(ref stockClass.priceList2, Brushes.Black, "Current Price");

            //plotData(stockClass.movingAvrage50, Brushes.Red, "MA 50");
            //plotData(stockClass.movingAvrage100, Brushes.Green, "MA 100");
            //plotData(stockClass.UpperBolinger, Brushes.Pink, "Upper Boliger");
            //plotData(stockClass.LowerBolinger, Brushes.Pink, "Upper Boliger");
        }

        private void plotTradedStock(object sender, NewTradeEventArgs args)
        {
            //propertyChanger.CurrentBalance = args.Data.ToString();
        }

        bool testRunning; 
        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (testRunning == false)
                {
                    testRunning = true;
                    new Task(() =>
                    {
                        testClass.StockDataAdded += GotNewStockData;
                        while (testClass.HasMoreData())
                        {

                            testClass.nextData();
                        }
                    }).Start();
                }
                //Fire new event with new data
                //testClass.nextData();
            }
        }

        public void GotNewStockData(object sender, NewDataEventArgs args)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                stockClass.addTestData(args.Data);


                if(stockClass.priceList.Count == 14)
                {
                    plotData(ref stockClass.RSI, Brushes.BurlyWood, "RSI");
                }
                else if (stockClass.priceList.Count == 20)
                {
                    plotData(ref stockClass.movingAvrage20, Brushes.BlueViolet, "MA 20");
                    plotData(ref stockClass.UpperBolinger, Brushes.Pink, "Upper Bolinger");
                    plotData(ref stockClass.LowerBolinger, Brushes.Pink, "Lower Bolinger");
                }
                else if (stockClass.priceList.Count == 50)
                {
                    plotData(ref stockClass.movingAvrage50, Brushes.Red, "MA 50");
                }
                else if (stockClass.priceList.Count == 100)
                {
                    plotData(ref stockClass.movingAvrage100, Brushes.Green, "MA 100");
                }
            }));

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

    public class PropertyClass : INotifyPropertyChanged
    {
        private string balance;
        public string CurrentBalance
        {
            get
            {
                return "Current balance: " + balance;
            }
            set
            {
                balance = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
