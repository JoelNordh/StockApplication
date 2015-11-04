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
using DataConverter;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;

namespace StockApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {     
        List<DataClass> priceList;
        List<PlotClass> movingAvrage20;
        List<PlotClass> UpperBolinger = new List<PlotClass>();
        List<PlotClass> LowerBolinger = new List<PlotClass>();

        private void plotData(List<PlotClass> list, Brush pen, String Description)
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

        private List<PlotClass> CalculateMovingAvrage(int avrage)
        {
            List<PlotClass> plotData = new List<PlotClass>();
            double lastAvrage = 0;
            for (int i = avrage; i < priceList.Count; i++)
            {
                lastAvrage = 0;
                for (int y = i - avrage; y < i; y++)
                {
                    lastAvrage += priceList[y].closingPrice;
                }
                lastAvrage = lastAvrage / avrage;

                plotData.Add(new PlotClass(lastAvrage, priceList[i].date));
            }
            return plotData;
        }

        private void CalculateBolinger()
        {
            for(int i =0; i<movingAvrage20.Count; i++)
            {
                double deviationSquare = 0;
                for(int y = 20+i; y > i; y--)
                {
                    deviationSquare += Math.Pow(movingAvrage20[i].value - priceList[y].closingPrice, 2);
                }
                deviationSquare = Math.Sqrt(deviationSquare / 20);

                UpperBolinger.Add(new PlotClass(movingAvrage20[i].value + (deviationSquare*2), movingAvrage20[i].date));
                LowerBolinger.Add(new PlotClass(movingAvrage20[i].value - (deviationSquare*2), movingAvrage20[i].date));
            }

            plotData(UpperBolinger, Brushes.Pink, "Upper bolinger");
            plotData(LowerBolinger, Brushes.Pink, "Lower bolinger");
        }



        public MainWindow()
        {
            InitializeComponent();

            movingAvrage20 = CalculateMovingAvrage(20);

            plotData(DataConverter.DataConverter.toPlotClass(priceList, DataClass.priceChooser.CLOSINGPRICE), Brushes.Black, "Current Price");
            plotData(movingAvrage20, Brushes.BlueViolet, "MA 20");
            plotData(CalculateMovingAvrage(50), Brushes.Red, "MA 50");
            plotData(CalculateMovingAvrage(100), Brushes.Green, "MA 100");

            CalculateBolinger();

        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                //Fire new event with new data
            }
        }
    }
}
