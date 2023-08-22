using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FftSharp;
using Spectrogram;



namespace Transient
{
    // ##########################***************************************


    internal class Program
    {
        static void Main(string[] args)
        {

            // ####################### Sin Wave ######################################
            double[] acSignal;
            double[] acTime;
            // get 200 ms buffer of AC Power sin signal of 50 Hz and 220 Volt
            generate_power_buffer(out acSignal, out acTime);

            // ######################### Transient Signal #########################
            double transient_amplitude = 4000 * 2.91;
            double transient_frequency = 6000;
            double start_time = 0.005; //t1 start duration

            double[] transientSignal = GenerateTransientSin(acTime, transient_amplitude, transient_frequency, start_time);

            // All Signal
            double[] power_transient = AddArrays(transientSignal, acSignal);

            // ############################ Short Time Fourier Transform ############## 
            //var sg = new SpectrogramGenerator(samplingRate, fftSize: 64, stepSize: 1, maxFreq: 16000);

            int window_size = 64;
            int step_size = 1;
            int sampling_rate = 32000;

            var sg = new SpectrogramGenerator(sampling_rate, fftSize: window_size, stepSize: step_size, minFreq: 3000 , maxFreq: 6500);

            sg.Add(power_transient);

            List<double[]> my_fft = sg.GetFFTs();

            double[] fft_time = CalculateTimePerIndex(my_fft.Count, window_size, step_size, sampling_rate);

            double[] mean_fft = new double[my_fft.Count];
            for (int i = 0; i < my_fft.Count; i++)
            {
                mean_fft[i] = my_fft[i].Average();
            }

            List<int> indices = FindIndicesGreaterThan(0.1, mean_fft);

            List<double> time_occurs = new List<double>();
            foreach (int index in indices)
            {
                time_occurs.Add(fft_time[index]); 
            }

            List<int> signal_index = new List<int>();
            foreach (double time in time_occurs)
            {
                //int index = (int)(sampling_rate * time) + 30; 
                int index = (int)(sampling_rate * time) + 29; 
                signal_index.Add(index);
            }

            // ########################## Peak & Width Calculations ######################
            List<double> transient_values = new List<double>();
            for (int i = signal_index[0]; i <= signal_index[signal_index.Count - 1]; i++)
            {
                transient_values.Add(power_transient[i]); 
            }
            //Debugged 

            List<double> transient__time = new List<double>();
            for (int i = signal_index[0]; i <= signal_index[signal_index.Count - 1]; i++)
            {
                transient__time.Add(acTime[i]); 
            }
            // Debugged

            List<double> transient_values_only = new List<double>();
            for (int i = 0; i < transient_values.Count; i++)
            {
                transient_values_only.Add(transient_values[i] - 220 * Math.Sin(2 * Math.PI * 50 * transient__time[i]));
            }
            // Debugged

            List<double> filtered_transient_values = new List<double>();
            foreach (double value in transient_values_only)
            {
                if (value > 220)
                {
                    filtered_transient_values.Add(value);
                }
            }

            double peak_value = filtered_transient_values.Max();
            int length = filtered_transient_values.Count;

            double width = length * 3.125e-05;

            int xr = 5;
            Console.WriteLine("Done");

        }
        // ################################# Functions ################################

        static double[] CalculateTimePerIndex(int stftLength, int windowSize, int step_size, double sampleRate)
        {
            double frameDuration = windowSize / sampleRate;
            double frameShift = step_size / sampleRate;
            int numFrames = stftLength;

            double[] timePerIndex = new double[numFrames];
            for (int i = 0; i < numFrames; i++)
            {
                timePerIndex[i] = i * frameShift + frameDuration / 2.0;
            }

            return timePerIndex;
        }

        static List<int> FindIndicesGreaterThan(double threshold, double[] values)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > threshold)
                {
                    indices.Add(i);
                }
            }
            return indices;
        }


        public static double[] AddArrays(double[] array1, double[] array2)
        {
            if (array1.Length != array2.Length)
            {
                throw new ArgumentException("Arrays must have the same length");
            }

            double[] result = new double[array1.Length];

            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = array1[i] + array2[i];
            }

            return result;
        }

        public static double[] GenerateTransientSin(double[] transientTime, double transientAmplitude, double transientFrequency, double t1)
        {
            double period = 1 / transientFrequency;

            double[] halfSine = new double[transientTime.Length];
            for (int i = 0; i < transientTime.Length; i++)
            {
                halfSine[i] = transientAmplitude * Math.Sin(2 * Math.PI * transientFrequency * transientTime[i]);
            }

            double halfPeriod = 1 / (transientFrequency * 2);
            double t2 = t1 + halfPeriod;
            double ty = (t1 + t2) / 2;

            double[] recWindow = Window(transientTime, t1, t2, 1);
            double[] expSignal = new double[transientTime.Length];
            for (int i = 0; i < transientTime.Length; i++)
            {
                expSignal[i] = Math.Exp(-transientTime[i] / ty);
            }

            double[] transientSignal = new double[transientTime.Length];
            for (int i = 0; i < transientTime.Length; i++)
            {
                transientSignal[i] = recWindow[i] * expSignal[i] * halfSine[i];
            }

            return transientSignal;
        }

        static void generate_power_buffer(out double[] acSignal, out double[] acTime)
        {
            // Constant parameters
            int samplingRate = 32000; // Sampling rate 32K Hz

            // AC Power Supply Signal
            double acAmplitude = 220;
            double acFrequency = 50; // Frequency in Hz
            double acDuration = 0.2;

            acTime = linspace(0, acDuration, (int)(acDuration * samplingRate));
            acSignal = new double[acTime.Length];

            for (int i = 0; i < acTime.Length; i++)
            {
                acSignal[i] = acAmplitude * Math.Sin(2 * Math.PI * acFrequency * acTime[i]);
            }
        }

        static double[] linspace(double start, double stop, int num)
        {
            double[] result = new double[num];
            double step = (stop - start) / (num - 1);
            for (int i = 0; i < num; i++)
            {
                result[i] = start + i * step;
            }
            return result;
        }

        public static double[] Window(double[] x, double t1, double t2, double A)
        {
            double[] rec_window = new double[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] >= t1 && x[i] < t2)
                {
                    rec_window[i] = A;
                }
                else
                {
                    rec_window[i] = 0;
                }
            }
            return rec_window;
        }

        // ##########################***************************************
    }
}