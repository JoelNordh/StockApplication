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

        private void plotData(ObservableCollection<PlotClass> list, Brush pen, String Description)
        {
            IPointDataSource point = null;
            LineGraph line;
            

            EnumerableDataSource<PlotClass> _edsSPP;
            _edsSPP = new EnumerableDataSource<PlotClass>(list);
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

            //movingAvrage20 = ;

            plotData(toPlotClass(stockClass.getPriceList(), DataClass.priceChooser.CLOSINGPRICE), Brushes.Black, "Current Price");
            //plotData(movingAvrage20, Brushes.BlueViolet, "MA 20");
            //plotData(CalculateMovingAvrage(50), Brushes.Red, "MA 50");
            //plotData(CalculateMovingAvrage(100), Brushes.Green, "MA 100");

            //CalculateBolinger();

        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                //Fire new event with new data
                //testClass.nextData();
            }
        }

        public static ObservableCollection<PlotClass> toPlotClass(ObservableCollection<DataClass> data, DataClass.priceChooser choice)
        {
            //List<PlotClass> toPlot = new List<PlotClass>();
            ObservableCollection<PlotClass> toPlot = new ObservableCollection<PlotClass>();
            foreach (DataClass Item in data)
            {
                PlotClass plotClass;
                switch (choice)
                {
                    case DataClass.priceChooser.AVRAGEPRICE:
                        plotClass = new PlotClass(Item.avragePrice, Item.date);
                        break;
                    case DataClass.priceChooser.HIGHPRICE:
                        plotClass = new PlotClass(Item.highPrice, Item.date);
                        break;
                    case DataClass.priceChooser.LOWPRICE:
                        plotClass = new PlotClass(Item.lowPrice, Item.date);
                        break;
                    case DataClass.priceChooser.CLOSINGPRICE:
                        plotClass = new PlotClass(Item.closingPrice, Item.date);
                        break;
                    default:
                        plotClass = new PlotClass();
                        break;
                }
                toPlot.Add(plotClass);
            }
            return toPlot;
        }
    }
}
