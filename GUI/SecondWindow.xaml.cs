using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Collections.ObjectModel;

using TransientParamsClass;
using TransientOutputClass;
using SignalGeneratorClass;


namespace Transient_GUI
{
    public partial class SecondWindow : Window
    {
        public ObservableCollection<TransientParams> TranData = new ObservableCollection<TransientParams>();
        public ObservableCollection<TransientOutput> TranOutput = new ObservableCollection<TransientOutput>();

        public double samplingRate = 32000;

        public double[] time;
        public double[] allSignals;

        List<TransientParams> transientParamsList = new List<TransientParams>();

        public SecondWindow()
        {
            InitializeComponent();
            TransientGrid.ItemsSource = TranData;
            OutputGrid.ItemsSource = TranOutput;
        }
        private void harmShow(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        // get value from textbox
        private double getValue(TextBox b1)
        {
            double value = 0;
            if (b1.Text != "")
            {
                value = double.Parse(b1.Text);
            }
            return value;
        }

        private void Draw(object sender, RoutedEventArgs e)
        {
            double tranStartTime = getValue(StartTimeBox);
            double tranFrequency = getValue(FrequencyBox);
            double tranAmplitude = getValue(AmplitudeBox);
            TranData.Add(new TransientParams { StartTime = tranStartTime, Frequency = tranFrequency, Amplitude = tranAmplitude });
            transientParamsList.Add(new TransientParams { StartTime = tranStartTime, Frequency = tranFrequency, Amplitude = tranAmplitude * 2.91 });

            StartTimeBox.Text = string.Empty;
            FrequencyBox.Text = string.Empty;
            AmplitudeBox.Text = string.Empty;

            // Define signal parameters
            double startTime = 0;
            double endTime = 0.2;
            double amplitude = 220;
            double frequency = 50;
            double phaseShift = 0;
            int harmonicsNum = 5;
            double duration = endTime - startTime;
            int numSamples = (int)(duration * samplingRate);

            (time, allSignals) = SignalGenerator.GenerateSignals(samplingRate, startTime, endTime, amplitude, frequency, phaseShift, harmonicsNum, transientParamsList);

            drawSignal(time, allSignals, TransientPlot, "TransientPlot", "Time", "SineWave", "SineWave");
           
        }
        private void drawSignal(double[] firstItem, double[] secondItem, PlotView p, string title = "Plot", string xtitle = "X-axis", string ytitle = "Y-axis", string graphtitle = "signal")
        {
            var plotModel = new PlotModel { Title = title };
            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = xtitle };
            var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = ytitle };
            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);
            var sineWaveSeries = new LineSeries { Title = graphtitle };
            for (int i = 0; i < firstItem.Length; i++)
            {
                sineWaveSeries.Points.Add(new DataPoint(firstItem[i], secondItem[i]));
            }
            plotModel.Series.Add(sineWaveSeries);
            p.Model = plotModel;
        }


        private void analyseTransient(object sender, RoutedEventArgs e)
        {
            // double transientMinThreshold = 350;
            double transientThreshold = getValue(TransientThresholdBox);

            (int transientsCount, double[] widths, double[] transientStartTime, List<double> transientPeaks) = TransientDetection.DetectTransients(allSignals, time, transientThreshold, samplingRate);

            TranOutput.Clear();
            for (int i = 0; i < transientsCount; i++)
            {
                TranOutput.Add(new TransientOutput { StartTime = transientStartTime[i], Width = widths[i], Peak = transientPeaks[i] });
            }
        }

    }
}