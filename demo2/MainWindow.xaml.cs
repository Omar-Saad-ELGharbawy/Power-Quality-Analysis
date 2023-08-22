using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using MathNet;
using MathNet.Numerics.IntegralTransforms;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace demo2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<DataHarm> HarmData = new ObservableCollection<DataHarm>();
        private ObservableCollection<DataTime> TimeData = new ObservableCollection<DataTime>();
        public MainWindow()
        {
            InitializeComponent();
            DataGrid1.ItemsSource = HarmData;
            DataGrid2.ItemsSource = TimeData;
        }
        // Switch to transient Tab
        private void transShow(object sender,RoutedEventArgs e){
            SecondWindow secondWindow = new SecondWindow();
            secondWindow.Show();
            this.Close();
        }
        // get value from textbox
        private int getValue(TextBox b1){
            int value = 0;
            if(b1.Text != ""){
                value = int.Parse(b1.Text);
            }
            return value;
        }
        //Plot button function
        private void Plot(object sender,RoutedEventArgs e){
            // int Fs = ;
            int Fs = getValue(FSBox);
            double []time=new double[Fs];
            double []signal =new double[Fs];
            double []freqs=new double[(int)(Fs/2)];
            double []mags =new double[Fs];
            double sinTime = 0;
            double fftTime = 0;
            double harmTime = 0;
            int amp = 1;
            int freq = 1;       
            amp = getValue(AmpBox);
            freq = getValue(FreqBox);
            
            if (HarmBox.SelectedItem != null)
            {
                string selectedText = ((ComboBoxItem)HarmBox.SelectedItem).Content.ToString();
                int selectedNum = int.Parse(selectedText);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                (time,signal) = generateSin(amp,freq,1,Fs,true,selectedNum);
                stopwatch.Stop();
                sinTime = stopwatch.Elapsed.TotalMilliseconds;
            }
            else{
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                (time,signal) = generateSin(amp,freq,1,Fs);
                stopwatch.Stop();
            }
            drawSignal(time,signal,SinePlot,"sine plot","Time","SineWave","SineWave");
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            (freqs,mags) = getFFT(signal,Fs,1);
            stopwatch2.Stop();
            fftTime = stopwatch2.Elapsed.TotalMilliseconds;
            drawSignal(freqs,mags,FouPlot,"Fourier plot","Frequency","Magnitude","Fou");
            HarmData.Clear();
            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();
            getHarmonics(freqs,mags,freq,Fs);
            stopwatch3.Stop();
            harmTime = stopwatch3.Elapsed.TotalMilliseconds;
            TimeData.Add(new DataTime { SineTime = sinTime, FFTTime = fftTime, HarmTime = harmTime });
        }
        //Generate Pure Sine wave
        private (double [],double[]) generateSin(double amp=1,int frequency=1,double duration=1,int fs=100,bool harmonics=false,int harmsValue=1){
            int size = (int) (fs*duration);
            double[] signal = new double[size];
            double[] time = new double[size];
            for(int ind=0;ind<fs*duration;ind+=1){
                // int ind = (int)(fs * x);
                double x = (double)ind/fs;
                signal[ind] = amp*Math.Sin(2*x*Math.PI*frequency);
                if(harmonics){
                    for(int i=2;i<2+harmsValue;i++){
                        signal[ind] += 0.1*amp*Math.Sin(2*x*Math.PI*frequency*i);
                    }
                }
                time[ind] = x;
            }
            return (time,signal);
        }
        // Get fourier of signal : this function takesignal and frequency of sampling and return frequency vector and magnitude in frequency domain
        private (double [],double[]) getFFT(double[] signal,int fs,double duration=1){
            // create variables of freq and mag
            // int size = (int) (fs*duration);
            double []frequencies = new double[(int)(fs/2)];
            Complex [] mag = new Complex[fs];
            // convert signal array to complex one to preform fft on it
            for(int i=0;i<fs;i++){
                mag[i] = new Complex((float)signal[i],0);
            }
            // declare new array to store mag of fft result(half of it only)
            double [] lastfou = new double[fs];
            // preform fourier
            Fourier.Forward(mag, FourierOptions.Matlab);
            // normalize of signal
            for(int i=0;i<mag.Length;i++){
                lastfou[i] = 2*mag[i].Magnitude/signal.Length;
            }
    
            // create frequency array to plot fourier by good representation
            for(int i=0;i<fs/2;i++){
                frequencies[i] = (double)i*fs/fs;
            }
            return(frequencies,lastfou);
        }
        // Draw signal to the UI
        private void drawSignal(double []firstItem,double[] secondItem,PlotView p,string title ="Plot",string xtitle="X-axis",string ytitle="Y-axis",string graphtitle="signal"){
            var plotModel = new PlotModel { Title = title };
            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = xtitle };
            var yAxis = new LinearAxis { Position = AxisPosition.Left, Title = ytitle };
            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);
            var sineWaveSeries = new LineSeries { Title = graphtitle };
            for(int i=0;i<firstItem.Length;i++){
                sineWaveSeries.Points.Add(new DataPoint(firstItem[i], secondItem[i]));
            }
            plotModel.Series.Add(sineWaveSeries);
            p.Model = plotModel;
        }
        private void getHarmonics(double[] frequencies,double[] signal,int fundmentalFreq,int fs)
        {
            int j =0;
            string [] orders = {"1st","2nd","3rd","4th","5th","6th","7th","8th","9th","10th"};
            for(int i=fundmentalFreq;i<fs;i+=fundmentalFreq){
                int ind = FindNearestIndex(frequencies,i);
                if(signal[ind]>0.01){
                    HarmData.Add(new DataHarm { Index = ind, Order=orders[j],mag = Math.Round(signal[ind],5), freq = Math.Round(frequencies[ind],5) });
                    j++;
                }
            }
        }
        private int FindNearestIndex(double[] array, double target)
        {
            int nearestIndex = 0;
            double minDifference = Math.Abs(array[0] - target);

            for (int i = 1; i < array.Length; i++)
            {
                double difference = Math.Abs(array[i] - target);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    nearestIndex = i;
                }
            }

            return nearestIndex;
            }
    }
}
