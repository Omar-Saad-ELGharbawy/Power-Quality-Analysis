using System;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;

/* 
FourierFilters Class Documentation:
----------------------------------
This class contains functions for filtering signals in the frequency domain.
The functions are based on the MathNet.Numerics.IntegralTransforms library.
    The functions are:
        - FourierTransform: This function takes a signal and returns the magnitude and phase of the signal in the frequency domain.
        - inverseFourierTransform: This function takes the magnitude and phase of a signal in the frequency domain and returns the signal in the time domain.
        - LowPassFourierFilter: This function takes a signal and returns the filtered signal in the time domain.
 */
public class FourierFilters
{

    /* FourierTransform Function Description :
        This function takes a signal and returns the magnitude and phase of the signal in the frequency domain.
        The function takes the following parameters:
            - signal: The signal in the time domain.
            - samplingRate: The sampling rate of the signal.
        The function returns the following:
            - frequencies: The frequencies of the signal in the frequency domain.
            - magnitude: The magnitude of the signal in the frequency domain.
            - phase: The phase of the signal in the frequency domain.
     */
    public static (double[], double[], double[]) FourierTransform(double[] signal, double samplingRate)
    {
        Complex[] complexSignal = new Complex[signal.Length];
        for (int i = 0; i < signal.Length; i++)
        {
            complexSignal[i] = new Complex(signal[i], 0);
        }

        Complex[] result = new Complex[complexSignal.Length];
        Array.Copy(complexSignal, result, complexSignal.Length);

        // forward fourier transform
        Fourier.Forward(result);
        double[] magnitude = new double[result.Length];
        double[] phase = new double[result.Length];

        for (int i = 0; i < result.Length; i++)
        {
            magnitude[i] = result[i].Magnitude;
            phase[i] = result[i].Phase;
        }

        // double deltaF = 5;
        double deltaF = samplingRate / signal.Length;
        double startFrequency = 0;
        double[] frequencies = new double[signal.Length];

        for (int i = 0; i < signal.Length; i++)
        {
            frequencies[i] = startFrequency + i * deltaF;
        }
        return (frequencies, magnitude, phase);
    }

    /*
    inverseFourierTransform Function Description :
        This function takes the magnitude and phase of a signal in the frequency domain and returns the signal in the time domain.
        The function takes the following parameters:
            - magnitudes: The magnitudes of the signal in the frequency domain.
            - phase: The phase of the signal in the frequency domain.
        The function returns the following:
            - inversefourier: The signal in the time domain. 
     */
    public static double[] inverseFourierTransform(double[] magnitudes, double[] phase)
    {
        Complex[] fourier_signal = new Complex[magnitudes.Length];

        for (int i = 0; i < magnitudes.Length; i++)
        {
            double magnitude = magnitudes[i];
            double angle = phase[i];

            Complex mag_phase_num = magnitude * Complex.Exp(Complex.ImaginaryOne * angle);
            fourier_signal[i] = mag_phase_num;
        }

        Fourier.Inverse(fourier_signal);

        double[] inversefourier = new double[magnitudes.Length];
        for (int i = 0; i < magnitudes.Length; i++)
        {
            inversefourier[i] = fourier_signal[i].Real;
        }

        return inversefourier;
    }

    /*
    LowPassFourierFilter Function Description :
        This function takes a signal and returns the filtered signal in the time domain.
        The function takes the following parameters:
            - signal: The signal in the time domain.
            - samplingRate: The sampling rate of the signal.
            - cutOff: The cut off frequency of the filter.
        The function returns the following:
            - filteredSignal: The filtered signal in the time domain. 
     */
    public static double[] LowPassFourierFilter(double[] signal, double samplingRate, double cutOff)
    {

        (double[] frequencies, double[] magnitudes, double[] phase) = FourierTransform(signal, samplingRate);

        // ###################### Low pass filter #######################

        int deltaF = (int)samplingRate / signal.Length;

        int cutOff_idx_start = (int)cutOff / deltaF;
        int cutOff_idx_end = (int)(samplingRate - cutOff) / deltaF;

        // int idx_middle = 16000 / deltaF;
        // int idx_end = 32000 / deltaF;

        int x = 100 * deltaF;

        for (int i = cutOff_idx_start - x; i < cutOff_idx_start; i++)
        {
            magnitudes[i] = 0;
        }

        for (int i = cutOff_idx_start; i < cutOff_idx_end; i++)
        {
            magnitudes[i] = 0;
        }

        for (int i = cutOff_idx_end; i < cutOff_idx_end + x; i++)
        {
            magnitudes[i] = 0;
        }

        double[] filteredSignal = inverseFourierTransform(magnitudes, phase);

        return filteredSignal;
    }

}