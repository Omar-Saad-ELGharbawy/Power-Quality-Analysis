using System;

namespace OneDConvolutionExample
{
    class ConvolutionCPU
    {
        public static void conv(float[] signal, float[] filter)
        {
            // Initialize the result array
            double[] result = new double[signal.Length - filter.Length + 1];
            // Perform convolution
            for (int i = 0; i < result.Length; i++)
            {
                float sum = 0;
                for (int j = 0; j < filter.Length; j++)
                {
                    sum += signal[i + j] * filter[j];
                }
                result[i] = sum;
            }

            // Print the result
            Console.WriteLine("Result of 1D Convolution CPU of First element :"+result[0]);
            Console.WriteLine("Result of 1D Convolution CPU of Last element :"+result[signal.Length-filter.Length]);
            // Console.WriteLine("Result of 500th number ");
            // Console.WriteLine(" ");
            // foreach (float value in result)
            // {
            //     Console.Write(value + " ");
            // }
        }
    }
}
