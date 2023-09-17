using System;



using TransientParamsClass;

// namespace SignalGenerator

public class SignalGenerator
{
        // ################################# Functions ################################
    public static void GenerateSin(double t1, double t2, double amplitude, double freq, double samplingRate, double phaseShift, out double[] time, out double[] signal)
    {
        double duration = t2 - t1;
        int numSamples = (int)(duration * samplingRate);
        signal = new double[numSamples];
        double phase = phaseShift / 360.0; // Convert phase shift from degrees to radians
        time = linspace(0, duration, (int)(duration * samplingRate));
        for (int i = 0; i < time.Length; i++)
        {
            signal[i] = amplitude * Math.Sin(2 * Math.PI * freq * time[i] + phase);
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

    public static double[] GenerateHarmonics(in double[] signal, double start, double last, double factor, double frequency, double fs, int iter)
    {
        double[] harmonics = signal.ToArray();

        double phaseShift = 0; // Phase shift in degrees
        for (int i = 2; i <= iter; i++)
        {
            GenerateSin(start, last, factor, frequency * i, fs, phaseShift * i, out double[] timeTemp, out double[] magTemp);
            for (int j = 0; j < signal.Length; j++)
            {
                harmonics[j] += (0.9 / i) * magTemp[j];
            }
        }
        return harmonics;
    }

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

    public static void GenerateSignals(double samplingRate, double startTime, double endTime, double amplitude, double frequency, double phaseShift, int harmonicsNum, List<TransientParams> transientParams, out double[] time, out double[] allSignals)
    {
        double duration = endTime - startTime;
        int numSamples = (int)(duration * samplingRate);
        // Generate Sin Signal 
        // double[] time;
        GenerateSin(startTime, endTime, amplitude, frequency, samplingRate, phaseShift, out time, out double[] acSignal);

        // Create the harmonics signal
        double[] harmonicsSignal = GenerateHarmonics(in acSignal, startTime, endTime, amplitude, frequency, samplingRate, harmonicsNum);

        // ######################### Generate Transient Signal #########################
        double transient_amplitude = 4000 * 2.91;
        double transient_frequency = 6000;
        double start_time = 0.005; //t1 start duration

        double[] transientSignal = GenerateTransientSignal(time, transient_amplitude, transient_frequency, start_time);
        // AC Signal + Transient
        double[] ac_transient = AddArrays(transientSignal, acSignal);

        // ############# Transients Generation #########################

        Console.WriteLine(transientParams[0].StartTime);

        double[] allTransients = new double[numSamples];

        for (int i = 0; i < transientParams.Count; i++)
        {
            double transientStartTime = transientParams[i].StartTime;
            double transientFreq = transientParams[i].Frequency;
            double transientAmp = transientParams[i].Amplitude;

            double[] generatedTransient = GenerateTransientSignal(time, transientAmp, transientFreq, transientStartTime);

            allTransients = AddArrays(allTransients, generatedTransient);
        }

        // All Transients + Harmonics
        // double[] allSignals = AddArrays(allTransients, harmonicsSignal);
        allSignals = AddArrays(allTransients, harmonicsSignal);
    }


}