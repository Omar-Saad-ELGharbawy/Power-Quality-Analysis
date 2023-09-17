using ScottPlot;
using System;
using Spectrogram;

using TransientParamsClass;

// using MathNet.Numerics.IntegralTransforms;


class Program
{
    static void Main()
    {
        /* ################################################### 
        *                 Generatte Signals
        ################################################### */

        // Define signal parameters
        double samplingRate = 32000;
        double startTime = 0;
        double endTime = 0.2;
        double amplitude = 220;
        double frequency = 50;
        double phaseShift = 0;
        int harmonicsNum = 5;
        double duration = endTime - startTime;
        int numSamples = (int)(duration * samplingRate);

        // Buffer Signal
        SignalGenerator.GenerateSin(startTime, endTime, amplitude, frequency, samplingRate, phaseShift, out double[] acTime, out double[] acSignal);

        // Transients Parameters 
        List<TransientParams> transientParams = new List<TransientParams>
        {
            new TransientParams { StartTime = 0.005, Frequency = 6000, Amplitude = 4000 * 2.91 },
            new TransientParams { StartTime = 0.1, Frequency = 4500, Amplitude = 6000 * 2.91 },
            new TransientParams { StartTime = 0.15, Frequency = 3000, Amplitude = 8000 * 2.91 }
        };

        // Pass parameters to GenerateSignals function
        SignalGenerator.GenerateSignals(samplingRate, startTime, endTime, amplitude, frequency, phaseShift, harmonicsNum, transientParams, out double[] time, out double[] allSignals);

        /* ************************************************************** PLOT ***************************************** */
        // var plt = new ScottPlot.Plot(800, 600);
        // // var scatter = plt.AddScatter(acTime, power_transient);
        // var scatter = plt.AddScatter(time, allSignals);
        // scatter.MarkerSize = 1;
        // scatter.MarkerShape = MarkerShape.none;
        // scatter.Color = System.Drawing.Color.Blue;
        // plt.Title("Sine Wave");
        // plt.XLabel("Time (s)");
        // plt.YLabel("Amplitude");
        // plt.Grid(true);
        // plt.AxisAuto(0, 0.1);  // Adjust the y-axis limits as needed
        // // Save the plot to an image file
        // plt.SaveFig("Images/Signals Function.png");

        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% END of Signals Simulations %%%%%%%%%%%%%%%%%%%%%%%%%

        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% START of Signal Analysis %%%%%%%%%%%%%%%%%%%%%%%%%

        // TRANSIENT Analysis Function
        
        // Find transients above the minimum threshold
        double transientMinThreshold = 100;

        (int transientsCount, double[] widths, double[] peaks, List<double> transientPeaks) =  TransientDetection.DetectTransients(allSignals, acTime, acSignal, transientMinThreshold, samplingRate);

        Console.WriteLine("transientsCount");
        Console.WriteLine(transientsCount);


        Console.WriteLine("transientPeaks");
        for (int i = 0; i < transientPeaks.Count; i++)
        {
            Console.WriteLine(transientPeaks[i]);
        }


        // ################ Short Time Fourier Transform (specgram) ###################

        // int window_size = 64;
        // int step_size = 1;
        // int sampling_rate = 32000;

        // var sg = new SpectrogramGenerator(sampling_rate, fftSize: window_size, stepSize: step_size, minFreq: 3000, maxFreq: 6500);
        // sg.Add(allSignals);

        // List<double[]> my_fft = sg.GetFFTs();
        // double[] fft_time = CalculateTimePerIndex(my_fft.Count, window_size, step_size, sampling_rate);

        // double[] mean_fft = new double[my_fft.Count];
        // for (int i = 0; i < my_fft.Count; i++)
        // {
        //     mean_fft[i] = my_fft[i].Average();
        // }

        // // ############# STFT Transient Localization #############

        // //find transient start indices and number of transients
        // double threshold = 0.5;
        // FindTransients(mean_fft, threshold, out int transientsCount, out List<int> transients_stft_start_indices);

        // Console.WriteLine(transientsCount);
        // // Console.WriteLine(transientsStartIndices.Count);
        // // Console.WriteLine(transients_stft_start_indices[0]);
        // // Console.WriteLine(transients_stft_start_indices[1]);
        // // Console.WriteLine(transients_stft_start_indices[2]);
        // for (int i = 0; i < transients_stft_start_indices.Count; i++)
        // {
        //     Console.WriteLine(transients_stft_start_indices[i]);
        // }
        // Console.WriteLine(" ######################## ");

        // double[] transient_sart_time = new double[transients_stft_start_indices.Count];

        // for (int i = 0; i < transients_stft_start_indices.Count; i++)
        // {
        //     int index = transients_stft_start_indices[i];
        //     transient_sart_time[i] = fft_time[index];
        // }

        // int[] transient_sart_indices = new int[transient_sart_time.Length];

        // for (int i = 0; i < transient_sart_time.Length; i++)
        // {
        //     transient_sart_indices[i] = (int)(transient_sart_time[i] * sampling_rate) + 29;
        // }

        // //  print transient_sart_time and transient_sart_indices

        // Console.WriteLine("Time ***********");
        // for (int i = 0; i < transient_sart_time.Length; i++)
        // {
        //     Console.WriteLine(transient_sart_time[i]);
        // }

        // Console.WriteLine("Time Index *********");
        // for (int i = 0; i < transient_sart_indices.Length; i++)
        // {
        //     Console.WriteLine(transient_sart_indices[i]);
        // }

        // // Remove fundamental effect from transients
        // double[] transientWithoutFundamental = new double[allSignals.Length];
        // for (int i = 0; i < allSignals.Length; i++)
        // {
        //     transientWithoutFundamental[i] = allSignals[i] - acSignal[i];
        // }

        // // Find transients above the minimum threshold
        // double transientMinThreshold = 100;

        // // ExtractTransients(transientWithoutFundamental, acTime, transient_sart_indices, transientMinThreshold, out List<double[]> transientsValues, out List<double[]> transientsTimes);

        // //Extract transients time amd values from time domain signal
        // List<double[]> transientsValues;
        // List<double[]> transientsTimes;
        // ExtractTransients(transientWithoutFundamental, acTime, transient_sart_indices, transientMinThreshold, out transientsValues, out transientsTimes);

        // Console.WriteLine("\nTransients Values:\n");
        // // Console.WriteLine(transientsValues.Count);

        // // for (int i = 0; i < transientsValues.Count; i++)
        // // {
        // //     Console.Write("\nTransients Array : ");
        // //     Console.WriteLine(i);
        // //     Console.Write("\n");
        // //     for (int j = 0; j < transientsValues[i].Length; j++)
        // //     {
        // //         Console.WriteLine(transientsValues[i][j]);
        // //     }
        // // }

        // // Width Calculation
        // double[] widths = GetTransientWidths(transientsValues);
        // // Peak Values Calculation in time domain
        // double[] peaks = GetTransientPeaksOld(transientsValues);

        // for (int i = 0; i < widths.Length; i++)
        // {
        //     Console.WriteLine(widths[i]);
        // }

        // for (int i = 0; i < peaks.Length; i++)
        // {
        //     Console.WriteLine(peaks[i]);
        // }

        // // ############################# New Method For Peak Width Calculation ###############################
        // // % ########## Low Pass Fourier Filter #####################

        // double[] low_pass_signal = FourierFilters.LowPassFourierFilter(allSignals, samplingRate, 3000);

        // double[] filteredSignal = SubtractSignals(allSignals, low_pass_signal);


        // List<double> transientPeaks = TransientDetection.FindTransientPeaks(filteredSignal, transient_sart_indices, transientsValues);

        
        /*  Calculate peaks from subtracted signal from low pass filtered signal to get 
        the the peak from positive and negative peaks them calculate transient peak */




        /* ************************************************************** PLOT ***************************************** */
        // var plt = new ScottPlot.Plot(800, 600);
        // // var scatter = plt.AddScatter(acTime, power_transient);
        // var scatter = plt.AddScatter(time, filteredSignal);
        // // var scatter = plt.AddScatter(time, low_pass_signal);
        // scatter.MarkerSize = 1;
        // scatter.MarkerShape = MarkerShape.none;
        // scatter.Color = System.Drawing.Color.Blue;
        // plt.Title("Sine Wave");
        // plt.XLabel("Time (s)");
        // plt.YLabel("Amplitude");
        // plt.Grid(true);
        // plt.AxisAuto(0, 0.1);  // Adjust the y-axis limits as needed
        // // Save the plot to an image file
        // // plt.SaveFig("Images/Signals Function.png");
        // // plt.SaveFig("low_pass_signal.png");
        // plt.SaveFig("filteredSignal.png");



    }
    // ################################# Functions ################################


    

    // public static double[] SubtractSignals(double[] signal1, double[] signal2)
    // {
    //     if (signal1.Length != signal2.Length)
    //     {
    //         throw new ArgumentException("Signal lengths must be equal.");
    //     }

    //     double[] result = new double[signal1.Length];
    //     for (int i = 0; i < signal1.Length; i++)
    //     {
    //         result[i] = signal1[i] - signal2[i];
    //     }

    //     return result;
    // }
    // public static void FindTransients(double[] signalBuffer, double threshold, out int transientsCount, out List<int> transientsStartIndices)
    // {
    //     transientsCount = 0;
    //     // transientsStartIndices.Clear();
    //     transientsStartIndices = new List<int>();
    //     bool isTransient = false;

    //     for (int i = 0; i < signalBuffer.Length; i++)
    //     {
    //         if (signalBuffer[i] > threshold)
    //         {
    //             if (!isTransient)
    //             {
    //                 // Start of a new transient
    //                 isTransient = true;
    //                 // Add the start index to the list
    //                 transientsStartIndices.Add(i);
    //                 transientsCount++;
    //             }
    //         }
    //         else
    //         {
    //             isTransient = false;
    //         }
    //     }
    // }
    // static double[] CalculateTimePerIndex(int stftLength, int windowSize, int step_size, double sampleRate)
    // {
    //     double frameDuration = windowSize / sampleRate;
    //     double frameShift = step_size / sampleRate;
    //     int numFrames = stftLength;

    //     double[] timePerIndex = new double[numFrames];
    //     for (int i = 0; i < numFrames; i++)
    //     {
    //         timePerIndex[i] = i * frameShift + frameDuration / 2.0;
    //     }

    //     return timePerIndex;
    // }


    // public static void ExtractTransients(double[] valuesBuffer, double[] timeBuffer, int[] startIndices, double threshold, out List<double[]> transientsValues, out List<double[]> transientsTimes)
    // {
    //     transientsValues = new List<double[]>();
    //     transientsTimes = new List<double[]>();

    //     for (int i = 0; i < startIndices.Length; i++)
    //     {
    //         List<double> valueList = new List<double>();
    //         List<double> timeList = new List<double>();
    //         int j = startIndices[i];

    //         // Find the first value above the threshold
    //         while (valuesBuffer[j] <= threshold)
    //         {
    //             j++;
    //         }

    //         while (valuesBuffer[j] > threshold)
    //         {
    //             valueList.Add(valuesBuffer[j]);
    //             timeList.Add(timeBuffer[j]);
    //             j++;
    //         }

    //         if (valueList.Count > 0)
    //         {
    //             transientsValues.Add(valueList.ToArray());
    //         }

    //         if (timeList.Count > 0)
    //         {
    //             transientsTimes.Add(timeList.ToArray());
    //         }
    //     }
    // }

    // public static double[] GetTransientPeaksOld(List<double[]> transientsValues)
    // {
    //     List<double> peakValues = new List<double>();

    //     foreach (double[] values in transientsValues)
    //     {
    //         if (values.Length > 0)
    //         {
    //             double rowPeak = values.Max();
    //             peakValues.Add(rowPeak);
    //         }
    //         else
    //         {
    //             peakValues.Add(0);
    //         }
    //     }

    //     return peakValues.ToArray();
    // }

    // public static double[] GetTransientWidths(List<double[]> transientsValues)
    // {
    //     List<double> widths = new List<double>();

    //     foreach (double[] values in transientsValues)
    //     {
    //         double rowWidth = values.Length * 3.125e-05;
    //         widths.Add(rowWidth);
    //     }

    //     return widths.ToArray();
    // }

}


