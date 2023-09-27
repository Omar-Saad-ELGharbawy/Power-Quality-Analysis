using System;
using System.Collections.Generic;

using TransientParamsClass;

/* 
SignalGenerator Class Documentation:
----------------------------------
This class contains functions for generating signals.
The functions are:
    - GenerateSin: This function generates a sinusoidal signal.
    - GenerateHarmonics: This function generates a signal with harmonics.
    - generateWindow: This function generates a rectangular window.
    - GenerateTransientSignal: This function generates a transient signal.
    - GenerateSignals: This function generates a signal with harmonics and transients.
    - AddSignals: This function adds two arrays element-wise.
    - SubtractSignals: This function subtracts two signals element-wise.
    - linspace: This function generates a linearly spaced array.
 */
namespace SignalGeneratorClass
{
    public class SignalGenerator
    {
        /*
        GenerateSin Function Description :
            This function generates a sinusoidal signal.
            The function takes the following parameters:
                - t1: The start time of the signal.
                - t2: The end time of the signal.
                - amplitude: The amplitude of the signal.
                - freq: The frequency of the signal.
                - samplingRate: The sampling rate of the signal.
                - phaseShift: The phase shift of the signal.
            The function returns the following:
                - time: The time of the signal.
                - signal: The signal. 
         */
        public static (double[], double[]) GenerateSin(double t1, double t2, double amplitude, double freq, double samplingRate, double phaseShift)
        {
            double duration = t2 - t1;
            int numSamples = (int)(duration * samplingRate);
            double[] signal = new double[numSamples];
            double phase = phaseShift / 360.0; // Convert phase shift from degrees to radians
            double[] time = linspace(0, duration, (int)(duration * samplingRate));
            for (int i = 0; i < time.Length; i++)
            {
                signal[i] = amplitude * Math.Sin(2 * Math.PI * freq * time[i] + phase);
            }
            return (time, signal);
        }

        /*
        GenerateHarmonics Function Description :
            This function generates a signal with harmonics.
            The function takes the following parameters:
                - amp: The amplitude of the signal.
                - frequency: The frequency of the signal.
                - duration: The duration of the signal.
                - fs: The sampling rate of the signal.
                - harmsValue: The number of harmonics of the signal.
            The function returns the following:
                - time: The time of the signal.
                - signal: The signal.
         */
        public static (double[], double[]) GenerateHarmonics(double amp, double frequency, double duration, double fs, int harmsValue)
        {
            int size = (int)(fs * duration);
            double[] signal = new double[size];
            double[] time = new double[size];
            for (int ind = 0; ind < fs * duration; ind += 1)
            {
                // int ind = (int)(fs * x);
                double x = (double)ind / fs;
                signal[ind] = amp * Math.Sin(2 * x * Math.PI * frequency);
                for (int i = 2; i < 2 + harmsValue; i++)
                {
                    signal[ind] += 0.1 * amp * Math.Sin(2 * x * Math.PI * frequency * i);
                }

                time[ind] = x;
            }
            return (time, signal);
        }
        /*
        generateWindow Function Description :
            This function generates a rectangular window.
            The function takes the following parameters:
                - x: The time array.
                - t1: The start time of the window.
                - t2: The end time of the window.
                - A: The amplitude of the window.
            The function returns the following:
                - rec_window: The rectangular window. GenerateBuffer
         */
        public static double[] generateWindow(double[] x, double t1, double t2, double A)
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
        /*
        GenerateTransientSignal Function Description :
            This function generates a transient signal.
            The function takes the following parameters:
                - transientTime: The time array.
                - transientAmplitude: The amplitude of the transient.
                - transientFrequency: The frequency of the transient.
                - t1: The start time of the transient.
            The function returns the following:
                - transientSignal: The transient signal. 
         */
        public static double[] GenerateTransientSignal(double[] transientTime, double transientAmplitude, double transientFrequency, double t1)
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

            double[] recWindow = generateWindow(transientTime, t1, t2, 1);
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

        /*
        GenerateSignals Function Description :
        This function generates a signal with harmonics and transients
        The function takes the following parameters:
            - samplingRate: The sampling rate of the signal.
            - startTime: The start time of the signal.
            - endTime: The end time of the signal.
            - amplitude: The amplitude of the signal.
            - frequency: The frequency of the signal.
            - phaseShift: The phase shift of the signal.
            - harmonicsNum: The number of harmonics of the signal.
            - transientParamsList: The list of transient parameters.
        The function returns the following:
        - signal: A generated signal with specified properties.
         */
        public static (double[], double[]) GenerateSignals(double samplingRate, double startTime, double endTime, double amplitude, double frequency, double phaseShift, int harmonicsNum, List<TransientParams> transientParamsList)
        {
            double duration = endTime - startTime;
            int numSamples = (int)(duration * samplingRate);
            // Create the harmonics signal
            (double[] time, double[] harmonicsSignal) = GenerateHarmonics(amplitude, frequency, duration, samplingRate, harmonicsNum);

            double[] allTransients = new double[numSamples];

            for (int i = 0; i < transientParamsList.Count; i++)
            {
                double transientStartTime = transientParamsList[i].StartTime;
                double transientFreq = transientParamsList[i].Frequency;
                double transientAmp = transientParamsList[i].Amplitude;

                double[] generatedTransient = GenerateTransientSignal(time, transientAmp, transientFreq, transientStartTime);

                allTransients = AddSignals(allTransients, generatedTransient);
            }

            // All Transients + Harmonics
            double[] allSignals = AddSignals(allTransients, harmonicsSignal);

            return (time, allSignals);
        }

        /*
AddArrays Function Description :
    This function adds two arrays element-wise.
    The function takes the following parameters:
        - array1: The first array.
        - array2: The second array.
    The function returns the following:
        - result: The sum of the two arrays.
  */
        public static double[] AddSignals(double[] array1, double[] array2)
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

        /*
            SubtractSignals Function Description :
                This function subtracts two signals element-wise.
                The function takes the following parameters:
                    - signal1: The first signal.
                    - signal2: The second signal.
                The function returns the following:
                    - result: The difference of the two signals. 
         */

        public static double[] SubtractSignals(double[] signal1, double[] signal2)
        {
            if (signal1.Length != signal2.Length)
            {
                throw new ArgumentException("Signal lengths must be equal.");
            }

            double[] result = new double[signal1.Length];
            for (int i = 0; i < signal1.Length; i++)
            {
                result[i] = signal1[i] - signal2[i];
            }

            return result;
        }

        /*
linspace Function Description :
    This function generates a linearly spaced array.
    It i commonly used to generate time array
    The function takes the following parameters:
        - start: The start value of the array.
        - stop: The end value of the array.
        - num: The number of elements in the array.
    The function returns the following:
        - result: The array. 
 */
        public static double[] linspace(double start, double stop, int num)
        {
            double[] result = new double[num];
            double step = (stop - start) / (num - 1);
            for (int i = 0; i < num; i++)
            {
                result[i] = start + i * step;
            }
            return result;
        }

        /*
        GenerateBuffer Function Description :
            This function generates a buffer of AC powe signal with 200 ms time, 50 Hz Frequency and 32KHz Sampling Rate.
            The function takes the following parameters:
                - None
            The function returns the following:
                - time: The time array.
                - signal: The signal.  
        */

        public static (double[], double[]) GenerateBuffer()
        {

            // Define signal parameters
            double samplingRate = 32000;
            double startTime = 0;
            double endTime = 0.2;
            double amplitude = 220;
            double frequency = 50;
            double duration = endTime - startTime;
            int numSamples = (int)(duration * samplingRate);

            double[] signal = new double[numSamples];

            double[] time = SignalGenerator.linspace(0, duration, numSamples);

            for (int i = 0; i < time.Length; i++)
            {
                signal[i] = amplitude * Math.Sin(2 * Math.PI * frequency * time[i]);
            }
            return (time, signal);
        }

    }
}