using System;
using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;

// namespace FourierFilter

public class FourierFilters
{
    // ################################# Functions ################################

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

        // Fourier.Forward(result, FourierOptions.Matlab);

        double[] magnitude = new double[result.Length];
        double[] phase = new double[result.Length];

        for (int i = 0; i < result.Length; i++)
        {
            // mag_fft_array[i] = Complex.Abs(result[i]) ;
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

    public static double[] LowPassFourierFilter(double[] signal, double samplingRate, double cutOff)
    {
        // Get Fourier Domain

        // ################################# Fourier Function #########################################
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

        // ################################# Inverse Fourier Function #########################################

        double[] filteredSignal = inverseFourierTransform(magnitudes, phase);

        return filteredSignal;
    }

}