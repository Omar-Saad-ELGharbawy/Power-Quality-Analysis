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
namespace demo2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Switch to transient Tab
        private void transShow(object sender,RoutedEventArgs e){
            SecondWindow secondWindow = new SecondWindow();
            secondWindow.Show();
            this.Close();
        }
        //Plot button function
        private void Plot(object sender,RoutedEventArgs e){
            int Fs = 1000;
            double []time=new double[Fs];
            double []signal =new double[Fs];
            double []freqs=new double[(int)(Fs/2)];
            double []mags =new double[Fs];
            double []harmfreqs =new double[1000];
            double []harmmags =new double[1000];
            int amp = 1;
            int freq = 1;
            if(AmpBox.Text != ""){
                amp = int.Parse(AmpBox.Text);
            }
            if(FreqBox.Text != ""){
                freq = int.Parse(FreqBox.Text);
            }         
            if (HarmBox.SelectedItem != null)
            {
                string selectedText = ((ComboBoxItem)HarmBox.SelectedItem).Content.ToString();
                int selectedNum = int.Parse(selectedText);
                (time,signal) = generateSin(amp,freq,1,1000,true,selectedNum);
            }
            else{
                (time,signal) = generateSin(amp,freq,1,1000);
            }
            drawSignal(time,signal,SinePlot,"sine plot","Time","SineWave","SineWave");
            (freqs,mags) = getFFT(signal,Fs);
            drawSignal(freqs,mags,FouPlot,"Fourier plot","Frequency","Magnitude","Fou");
            // (harmfreqs,harmmags)=getHarmonics(freqs,mags,freq,Fs);
            // displayHarms(harmfreqs,harmmags);
            // Harm1Box.Text = harmfreqs.Length.ToString();
        }
        //Generate Pure Sine wave
        private (double [],double[]) generateSin(double amp=1,int frequency=1,double duration=1,int fs=100,bool harmonics=false,int harmsValue=1){
            int size = (int) (fs*duration);
            double[] signal = new double[size];
            double[] time = new double[size];
            for(double x=0;x<duration;x+=1.0/fs){
                int ind = (int)(fs * x);
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
        private (double [],double[]) getFFT(double[] signal,int fs){
            // create variables of freq and mag
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
        // detect harmonics in signal
        private (double[],double[]) getHarmonics(double[] frequencies,double[] signal,int fundmentalFreq,int fs){
            double [] mags = new double[10];
            double [] freqs = new double[10];
            int j=0;
            for(int i=0;i<fs;i++){
                if(i%fundmentalFreq==0){
                    // int ind = getIndex(freqs,i,0.3);
                    Harm2Box.Text = i.ToString();
                    double target = (double)i;
                    int ind = FindNearestIndex(freqs,target);
                    mags[j] = ind;
                    freqs[j] = frequencies[ind];
                    j++;
                }
            }
            Array.Resize(ref mags, j);   // Resize arrays to actual number of elements
            Array.Resize(ref freqs, j);
            return(freqs,mags);
        }
        private void displayHarms(double []freqs,double[] mags){
            TextBlock [] boxes = {Harm1Box,Harm3Box};
            for(int i=0;i<freqs.Length;i++){
                boxes[i].Text = mags[i].ToString();
                if(i==2){
                    break;
                }
            }
        }
        private int getIndex(double []array,int target,double diff){
            for(int i=0;i<array.Length;i++){
                if((array[i]-target)<=diff){
                    return i;
                }
            }
            return 0;
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
