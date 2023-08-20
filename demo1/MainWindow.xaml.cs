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
using System.Xaml;
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
        double []time = new double[320*3];
        double []signal = new double[320*3];
        int amp=1;
        int freq=1;
        public MainWindow()
        {
            InitializeComponent();
        }
        // When we click on the button
        private void GeneratePlot(object sender,RoutedEventArgs e){
            // get the value of amp and freq
            if(!(ampBox.Text=="")){
                amp = int.Parse(ampBox.Text);
            }
            if(!(freqBox.Text==""))
            {
               freq = int.Parse(freqBox.Text); 
            }
            // in if put condtion if radio button is checked
            if(harmonicsButton.IsChecked == false){
                    (time,signal) = generateSin(amp,freq,320,true);
                    signalPlot(time,signal);
                    fouPlot(time,signal);
                }
            else{
                (time,signal) = generateSin(amp,freq,320);
                signalPlot(time,signal);
            }
                
            }
            // generate the signal with harmonics or without 
        private (double [],double[]) generateSin(int amp,int freq,int Fs,bool harmonics=false){
            double [] sig = new double[Fs*3];
            double [] time = new double[Fs*3];
            for (int x = 0; x < 3*Fs; x += 1)
            {
                double t = (double)x / Fs;
                double y = amp*Math.Sin(2*t*Math.PI*freq);
                time[x] = t;
                if(harmonics){
                    y = y + 0.1*amp*Math.Sin(2*t*Math.PI*freq*2);
                    y = y + 0.1*amp*Math.Sin(2*t*Math.PI*freq*3);
                }
                sig[x] = y;
            }
            return (time,sig);
        }
        private void signalPlot(double []time, double []signal){
            var plotModel = new PlotModel { Title = "Sine Wave Plot" };

            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" };
            var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Value" };

            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            var sineWaveSeries = new LineSeries { Title = "Sine Wave" };
            for(int i=0;i<time.Length;i++){
                sineWaveSeries.Points.Add(new DataPoint(time[i], signal[i]));
            }
            plotModel.Series.Add(sineWaveSeries);
            plotView.Model = plotModel;
        }
        private void fouPlot(double []time, double []signal){
            var plotModel = new PlotModel { Title = "Sine Wave Plot" };

            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" };
            var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Value" };

            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            var sineWaveSeries = new LineSeries { Title = "Sine Wave" };
            for(int i=0;i<time.Length;i++){
                sineWaveSeries.Points.Add(new DataPoint(time[i], signal[i]));
            }
            plotModel.Series.Add(sineWaveSeries);
            plotView2.Model = plotModel;
        }
        // private void InitializePlot(int amp,int freq,int Fs)
        // {
        //     var plotModel = new PlotModel { Title = "Sine Wave Plot" };

        //     var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" };
        //     var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Value" };

        //     plotModel.Axes.Add(xAxis);
        //     plotModel.Axes.Add(yAxis);

        //     var sineWaveSeries = new LineSeries { Title = "Sine Wave" };
        //     for (int x = 0; x < 3*Fs; x += 1)
        //     {
        //         double time = (double)x / Fs;
        //         double y = amp*Math.Sin(2*time*Math.PI*freq);
        //         sig[x] = y;
        //         sineWaveSeries.Points.Add(new DataPoint(time, y));
        //     }

        //     plotModel.Series.Add(sineWaveSeries);
        //     plotView.Model = plotModel;
        // }
    }
}
