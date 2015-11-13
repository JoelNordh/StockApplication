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
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using System.Collections.ObjectModel;
using StockHandler;
using DataConverter;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Research.DynamicDataDisplay.Common;

namespace StockApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {     
        TestClass testClass = new TestClass(new CsvStockParser());
        StockTrader stockTrader = new StockTrader();
        StockHandler.StockHandler stockClass;
        StockDataStorage stockDataStorage;
        DataTestTimer testTimer; 

        PropertyClass propertyChanger = new PropertyClass();
        Collection<StockData> buyPoints = new ObservableCollection<StockData>(); //TODO::Eventuellt läggas till i StockDataStorage
        Collection<StockData> sellPoints = new ObservableCollection<StockData>();

        #region plotTools
        private void plotData(Collection<StockData> list, Brush pen, String Description, ChartPlotter graph)
        {
            IPointDataSource point = null;
            LineGraph line;
            

            EnumerableDataSource<StockData> _edsSPP;
            _edsSPP = new EnumerableDataSource<StockData>(list);
            
            
            _edsSPP.SetXMapping(p => dateAxis.ConvertToDouble(p.date));
            _edsSPP.SetYMapping(p => p.value);
            point = _edsSPP;

            line = new LineGraph(point);
            line.LinePen = new Pen(pen, 2);
            line.Description = new PenDescription(Description);

            graph.Children.Add(line);
            graph.FitToView();
        }

        private void plotSellEvent(Collection<StockData> list)
        {

            IPointDataSource point = null;
            CirclePointMarker marker = new CirclePointMarker();

            EnumerableDataSource<StockData> _edsSPP;
            _edsSPP = new EnumerableDataSource<StockData>(list);
            _edsSPP.SetXMapping(p => dateAxis.ConvertToDouble(p.date));
            _edsSPP.SetYMapping(p => p.value);
            point = _edsSPP;

            SellMarkerGraph.DataSource = point;

            marker.Size = 10;
            marker.Fill = new SolidColorBrush(Colors.Red);
            marker.Pen = new Pen(new SolidColorBrush(Colors.Black), 2.0);
            SellMarkerGraph.Marker = marker;     
        }

        private void plotBuyEvent(Collection<StockData> list)
        {
            IPointDataSource point = null;
            CirclePointMarker marker = new CirclePointMarker();

            EnumerableDataSource<StockData> _edsSPP;
            _edsSPP = new EnumerableDataSource<StockData>(list);
            _edsSPP.SetXMapping(p => dateAxis.ConvertToDouble(p.date));
            _edsSPP.SetYMapping(p => p.value);
            point = _edsSPP;

            BuyMarkerGraph.DataSource = point;

            marker.Size = 10;
            marker.Fill = new SolidColorBrush(Colors.Blue);
            marker.Pen = new Pen(new SolidColorBrush(Colors.Black), 2.0);
            BuyMarkerGraph.Marker = marker;
        }

        //private void plotSqueeze(Collection<StockData>list, Brush pen)
        //{
        //    IPointDataSource point = null;
        //    LineGraph line;


        //    EnumerableDataSource<StockData> _edsSPP;
        //    _edsSPP = new EnumerableDataSource<StockData>(list);


        //    _edsSPP.SetXMapping(p => dateAxis.ConvertToDouble(p.date));
        //    _edsSPP.SetYMapping(p => p.value);
        //    point = _edsSPP;

        //    line = new LineGraph(point);
        //    line.LinePen = new Pen(pen, 2);

        //    graph.Children.Add(line);
        //    graph.FitToView();
        //}

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            stockDataStorage = new StockDataStorage();
            stockClass = new StockHandler.StockHandler(stockTrader, stockDataStorage);

            testTimer = new DataTestTimer(5, testClass);
            testClass.StockDataAdded += GotNewStockData;

            stockTrader.StockSold += plotSoldStock;
            stockTrader.StockBought += plotBoughtStock;

            plotter.Viewport.PropertyChanged += Viewport_PropertyChanged;

            this.DataContext = propertyChanger;

            plotData(stockDataStorage.Get(IdentifierConstants.PRICE_LIST), Brushes.Black, "Current Price", plotter);
            plotSellEvent(sellPoints);
            plotBuyEvent(buyPoints);
            
        }

        private void plotBoughtStock(object sender, TradeEventArgs args)
        {
            buyPoints.Add(new StockData(args.Data.closingPrice, args.Data.date));
        }

        private void plotSoldStock(object sender, TradeEventArgs args)
        {
            propertyChanger.CurrentBalance = args.Balance.ToString();
            sellPoints.Add(new StockData(args.Data.closingPrice, args.Data.date));
        }

        bool testRunning; 
        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (testRunning == false)
                {
                    testRunning = true;
                    testTimer.StartTimer();
                }
                else
                {
                    testRunning = false;
                    testTimer.StopTimer();
                }
            }
        }

        public void GotNewStockData(object sender, NewDataEventArgs args)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                stockClass.addTestData(args.Data);

                if (stockClass.priceList.Count == 14)
                {
                    plotData(stockDataStorage.Get(IdentifierConstants.RSI), Brushes.BurlyWood, "RSI", RSIPlotter);
                    plotData(stockDataStorage.Get(IdentifierConstants.ATR), Brushes.DarkSlateGray, "ATR", RSIPlotter);
                }
                else if (stockClass.priceList.Count == 20)
                {
                    plotData(stockDataStorage.Get(IdentifierConstants.UPPERKELTNER), Brushes.Purple, "Upper Keltner", plotter);
                    plotData(stockDataStorage.Get(IdentifierConstants.LOWERKELTNER), Brushes.Purple, "Lower Keltner", plotter);
                    plotData(stockDataStorage.Get(IdentifierConstants.EXPONENTIAL_MOVING_AVERAGE, 20), Brushes.Purple, "Upper Keltner", plotter);
                    //plotData(stockDataStorage.Get(IdentifierConstants.SIMPLE_MOVING_AVERAGE, 20), Brushes.BlueViolet, "MA 20", plotter);
                    //plotData(stockDataStorage.Get(IdentifierConstants.UPPERBOLINGER), Brushes.Pink, "Upper Bolinger", plotter);
                    //plotData(stockDataStorage.Get(IdentifierConstants.LOWERBOLINGER), Brushes.Pink, "Lower Bolinger", plotter);
                }
                else if (stockClass.priceList.Count == 50)
                {
                    //plotData(stockDataStorage.Get(IdentifierConstants.SIMPLE_MOVING_AVERAGE, 50), Brushes.Red, "MA 50", plotter);
                }
                else if (stockClass.priceList.Count == 100)
                {
                    //plotData(stockDataStorage.Get(IdentifierConstants.SIMPLE_MOVING_AVERAGE, 100), Brushes.Green, "MA 100", plotter);
                }
            }));

        }



        public static ObservableCollection<StockData> toStockClass(ObservableCollection<DataClass> data, DataClass.priceChooser choice)
        {
            ObservableCollection<StockData> toPlot = new ObservableCollection<StockData>();
            foreach (DataClass Item in data)
            {
                StockData plotClass;
                switch (choice)
                {
                    case DataClass.priceChooser.AVRAGEPRICE:
                        plotClass = new StockData(Item.avragePrice, Item.date);
                        break;
                    case DataClass.priceChooser.HIGHPRICE:
                        plotClass = new StockData(Item.highPrice, Item.date);
                        break;
                    case DataClass.priceChooser.LOWPRICE:
                        plotClass = new StockData(Item.lowPrice, Item.date);
                        break;
                    case DataClass.priceChooser.CLOSINGPRICE:
                        plotClass = new StockData(Item.closingPrice, Item.date);
                        break;
                    default:
                        plotClass = new StockData();
                        break;
                }
                toPlot.Add(plotClass);
            }
            return toPlot;
        }


        private void Viewport_PropertyChanged(object sender, ExtendedPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Visible")
            {
                RSIPlotter.Viewport.Visible = new DataRect(plotter.Viewport.Visible.X, RSIPlotter.Viewport.Visible.Y, plotter.Viewport.Visible.Width, RSIPlotter.Viewport.Visible.Height);
            }
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
