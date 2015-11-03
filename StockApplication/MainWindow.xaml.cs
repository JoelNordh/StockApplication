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
        private void plotData(List<DataClass> list)
        {
            IPointDataSource point = null;
            LineGraph line;

            EnumerableDataSource<DataClass> _edsSPP;
            _edsSPP = new EnumerableDataSource<DataClass>(list);
            _edsSPP.SetXMapping(p => dateAxis.ConvertToDouble(p.date));
            _edsSPP.SetYMapping(p => p.highPrice);
            point = _edsSPP;

            line = new LineGraph(point);
            line.LinePen = new Pen(Brushes.Black, 2);
            line.Description = new PenDescription("Price");
            plotter.Children.Add(line);
            plotter.FitToView();
        }

        public MainWindow()
        {
            InitializeComponent();
            List<DataClass> list = DataConverter.DataConverter.ParseCSV(new System.IO.StreamReader("history.csv"));

            //LineChart1.DataContext = list;

            plotData(list);

            string test = "";
        }
    }
}
