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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace demo1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int amp=1;
        int freq=1;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void GeneratePlot(object sender,RoutedEventArgs e){
            if(!(ampBox.Text=="")){
                amp = int.Parse(ampBox.Text);
            }
            if(!(freqBox.Text==""))
            {
               freq = int.Parse(freqBox.Text); 
            }
            InitializePlot(amp,freq,10);
        }
        private void InitializePlot(int amp,int freq,int Fs)
        {
            var plotModel = new PlotModel { Title = "Sine Wave Plot" };

            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" };
            var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Value" };

            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            var sineWaveSeries = new LineSeries { Title = "Sine Wave" };
            for (double x = 0; x < 3; x += 0.01 )
            {
                double y = amp*Math.Sin(2*x*Math.PI*freq);
                sineWaveSeries.Points.Add(new DataPoint(x, y));
            }

            plotModel.Series.Add(sineWaveSeries);
            plotView.Model = plotModel;
        }
    }
}
