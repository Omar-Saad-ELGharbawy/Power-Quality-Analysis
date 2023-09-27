using System;
using System.Collections.Generic;
using System.Linq;
using Spectrogram;

using SignalGeneratorClass;

/*
TransientDetection Class Documentation:
----------------------------------
The TransientDetection class provides methods for detecting and analyzing transient signals in a given input signal.
Transients are short-lived, high-energy events in the signal that occur for a brief duration.
The main method, DetectTransients, takes in a signal, its corresponding time array, a minimum threshold for transient detection, and the sampling rate.
The method performs a short-time Fourier transform (specgram) on the signal to analyze its frequency content over time. 
It then locates transients by identifying sections of the transformed signal that exceed a specified threshold. 
The start time and count of transients are determined, and their characteristics, such as width and peak values, are calculated.
Additionally, a new method for peak calculation is implemented using a low-pass Fourier filter to enhance transient detection. 
The filtered signal is subtracted from the original signal to isolate the transients, and their peak values are determined.
Finally, the method returns the count of transients, their widths, start times in the time domain, and a list of transient peak values. 
 */


public class TransientDetection
{

    /*
    DetectTransients Function Description:
    This function detects transients in a given signal using the Short Time Fourier Transform (specgram) technique. It takes the following parameters:
    - signal: The input signal for transient detection.
    - time: The time array corresponding to the input signal.
    - transientMinThreshold: The minimum threshold for transient detection.
    - samplingRate: The sampling rate of the input signal.
    The function performs the following steps:
    1. Short Time Fourier Transform (specgram): It calculates the spectrogram of the input signal using a specified window size, step size, and frequency range.
    2. Mean FFT Calculation: It calculates the mean FFT value for each spectrogram slice.
    3. Transient Localization: It identifies the start indices of transients in the mean FFT array based on a specified threshold.
    4. Time Domain Localization: It converts the start indices of transients from the spectrogram domain to the time domain.
    5. Transient Extraction: It extracts the transient values and corresponding time values from the input signal based on the start indices.
    6. Width Calculation: It calculates the width of each transient.
    7. Peak Values Calculation: It calculates the peak values of each transient in the time domain.
    8. New Method for Peak Calculation: It applies a low-pass Fourier filter to the input signal, subtracts the filtered signal from the original signal, and finds the transient peaks in the resultant signal.
    The function returns the following tuple:
    - transientsCount: The total number of detected transients.
    - widths: An array containing the width of each transient.
    - time_domain_transient_start: An array containing the time values corresponding to the start of each transient in the time domain.
    - transientPeaks: A list of transient peak values in the time domain.
    */
    public static (int, double[], double[], List<double>) DetectTransients(double[] signal, double[] time, double transientMinThreshold, double samplingRate)
    {

        // ################ Short Time Fourier Transform (specgram) ###################
        (double[] acTime, double[] acSignal) = SignalGenerator.GenerateBuffer();

        int window_size = 64;
        int step_size = 1;
        int sampling_rate = 32000;

        var sg = new SpectrogramGenerator(sampling_rate, fftSize: window_size, stepSize: step_size, minFreq: 3000, maxFreq: 6500);
        sg.Add(signal);

        List<double[]> my_fft = sg.GetFFTs();

        double[] fft_time = CalculateTimePerIndex(my_fft.Count, window_size, step_size, sampling_rate);

        double[] mean_fft = new double[my_fft.Count];
        for (int i = 0; i < my_fft.Count; i++)
        {
            mean_fft[i] = my_fft[i].Average();
        }
        // Console.Write("mean_fft Max : ");
        // Console.WriteLine(mean_fft.Max());

        /*  STFT Transient Localization
        find transient start indices and number of transients */
        double threshold = 0.6;
        (int transientsCount, List<int> transients_stft_start_indices) = FindTransients(mean_fft, threshold);

        double[] transient_sart_time = new double[transients_stft_start_indices.Count];

        for (int i = 0; i < transients_stft_start_indices.Count; i++)
        {
            int index = transients_stft_start_indices[i];
            transient_sart_time[i] = fft_time[index];
        }

        int[] transient_sart_indices = new int[transient_sart_time.Length];

        for (int i = 0; i < transient_sart_time.Length; i++)
        {
            transient_sart_indices[i] = (int)(transient_sart_time[i] * sampling_rate) + 29;
        }

        double[] time_domain_transient_start = new double[transient_sart_indices.Length];
        for (int i = 0; i < transient_sart_indices.Length; i++)
        {
            time_domain_transient_start[i] = time[transient_sart_indices[i]];
        }

        double[] transientWithoutFundamental = new double[signal.Length];
        for (int i = 0; i < signal.Length; i++)
        {
            transientWithoutFundamental[i] = signal[i] - acSignal[i];
        }

        // Console.Write("transientWithoutFundamental Max : ");
        // Console.WriteLine(transientWithoutFundamental.Max());
        //Extract transients time amd values from time domain signal
        (List<double[]> transientsValues, List<double[]> transientsTimes) = ExtractTransients(transientWithoutFundamental, time, transient_sart_indices, transientMinThreshold);

        // Width Calculation
        double[] widths = GetTransientWidths(transientsValues);
        // Peak Values Calculation in time domain
        double[] peaks = GetTransientPeaksOld(transientsValues);

        // ############################# New Method For Peak Width Calculation ###############################
        // ########## Low Pass Fourier Filter #####################

        double[] low_pass_signal = FourierFilters.LowPassFourierFilter(signal, samplingRate, 3000);

        double[] filteredSignal = SignalGenerator.SubtractSignals(signal, low_pass_signal);

        List<double> transientPeaks = FindTransientPeaks(filteredSignal, transient_sart_indices, transientsValues);

        return (transientsCount, widths, time_domain_transient_start, transientPeaks);
    }

    /*
    FindTransients Function Description:
    This function finds transients in a given signal based on a specified threshold. It takes the following parameters:
    - signalBuffer: The input signal for transient detection.
    - threshold: The minimum threshold for transient detection.
    The function returns the following:
    - Transient count and indices of detected transients.
     */
    private static (int, List<int>) FindTransients(double[] signalBuffer, double threshold)
    {
        int transientsCount = 0;
        List<int> transientsStartIndices = new List<int>();
        bool isTransient = false;

        for (int i = 0; i < signalBuffer.Length; i++)
        {
            if (signalBuffer[i] > threshold)
            {
                if (!isTransient)
                {
                    // Start of a new transient
                    isTransient = true;
                    // Add the start index to the list
                    transientsStartIndices.Add(i);
                    transientsCount++;
                }
            }
            else
            {
                isTransient = false;
            }
        }
        return (transientsCount, transientsStartIndices);
    }

    /*
    ExtractTransients Function Description:
    This function extracts transients from a given signal based on the start indices of the transients. It takes the following parameters:
    - valuesBuffer: The input signal for transient detection.
    - timeBuffer: The time array corresponding to the input signal.
    - startIndices: The start indices of the transients in the input signal.
    - threshold: The minimum threshold for transient detection.
    The function returns the following tuple:
    - A vector containing all extracted transients' values.
    - A vector containing all extracted transients' time values.
     */
    private static (List<double[]>, List<double[]>) ExtractTransients(double[] valuesBuffer, double[] timeBuffer, int[] startIndices, double threshold)
    {
        List<double[]> transientsValues = new List<double[]>();
        List<double[]> transientsTimes = new List<double[]>();

        for (int i = 0; i < startIndices.Length; i++)
        {
            List<double> valueList = new List<double>();
            List<double> timeList = new List<double>();
            int j = startIndices[i];
            int shift = 0;
            int tran_count = 0;

            // Find the first value above the threshold
            while (valuesBuffer[j] <= threshold)
            {
                j++;
                shift++;
                if (shift > 10)
                {
                    break;
                }
            }

            while (valuesBuffer[j] > threshold)
            {
                valueList.Add(valuesBuffer[j]);
                timeList.Add(timeBuffer[j]);
                j++;
                //tran_count++;
                //if (tran_count> 15)
                //{
                //    throw new ArgumentException("Check Signals Mean STFT Without Transient and Threshold Value");
                //}
            }

            if (valueList.Count > 0)
            {
                transientsValues.Add(valueList.ToArray());
            }

            if (timeList.Count > 0)
            {
                transientsTimes.Add(timeList.ToArray());
            }
        }

        return (transientsValues, transientsTimes);
    }

    private static double[] GetTransientPeaksOld(List<double[]> transientsValues)
    {
        List<double> peakValues = new List<double>();

        foreach (double[] values in transientsValues)
        {
            if (values.Length > 0)
            {
                double rowPeak = values.Max();
                peakValues.Add(rowPeak);
            }
            else
            {
                peakValues.Add(0);
            }
        }

        return peakValues.ToArray();
    }

    /*
    GetTransientWidths Function Description:
    This function takes the list of transient peaks and returns a vector containing widths for each transient.

     */

    private static double[] GetTransientWidths(List<double[]> transientsValues)
    {
        List<double> widths = new List<double>();

        foreach (double[] values in transientsValues)
        {
            double rowWidth = values.Length * 3.125e-05;
            widths.Add(rowWidth);
        }

        return widths.ToArray();
    }

    /*
    FindTransientPeaks Function Description:
    This function takes the filtered signal, transient start indices, and transient values and returns a list of transient peak values.
    This function finds the peaks of each transient and returns them in a list
     */
    private static List<double> FindTransientPeaks(double[] filteredSignal, int[] transientStartIndices, List<double[]> transientValues)
    {
        List<double> transientPeaks = new List<double>();

        for (int i = 0; i < transientStartIndices.Length; i++)
        {
            int startIndex = transientStartIndices[i];
            int endIndex = startIndex + transientValues[i].Length + 1;

            double[] tranIBuffer = new double[endIndex - startIndex + 1];
            Array.Copy(filteredSignal, startIndex, tranIBuffer, 0, tranIBuffer.Length);

            double transientPeak = FindPeaks(tranIBuffer);
            transientPeaks.Add(transientPeak);
        }
        return transientPeaks;
    }

    /*
    FindPeaks Function Description:
    This function takes a signal and returns the transient peak value of the signal. 
     */

    private static double FindPeaks(double[] signal)
    {
        double positivePeak = signal.Max();
        double negativePeak = signal.Min();

        double transientPeak = positivePeak - negativePeak;
        return transientPeak;
    }

    /*
    CalculateTimePerIndex Function Description:
    This function takes: 
    - stftLength: The length of the spectrogram.
    - windowSize: The window size used for the spectrogram.
    - step_size: The step size used for the spectrogram.
    - sampleRate: The sampling rate of the signal.
    This function returns:
    - timePerIndex: The time values corresponding to each spectrogram slice. 
     */
    private static double[] CalculateTimePerIndex(int stftLength, int windowSize, int step_size, double sampleRate)
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

}