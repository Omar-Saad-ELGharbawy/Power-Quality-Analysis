using System;
using OpenCL.Net;
using OpenCL.Net.Extensions;
using GPUoptimization;
namespace OneDConvolutionGPU
{
    class ConvolutionGPU
    {
        public static void conv(Context context,CommandQueue commandQueue,Device device,float[] signal,float[] filter)
        {
            // float[] signal = { 1, 2, 3, 4, 5, 6, 7};
            // float[] filter = { -1, 0, 1 };
            float [] result = new float[signal.Length- filter.Length + 1];
            
            IMem signalBuffer = GPUModule.addBuffer(context,commandQueue,signal);
            IMem kernelBuffer = GPUModule.addBuffer(context,commandQueue,filter);
            IMem lengthBuffer = GPUModule.addBuffer(context,commandQueue, new float [] {filter.Length});
            IMem resultBuffer = GPUModule.addBuffer(context,commandQueue,result);
             string kernelSource = @"
            __kernel void conv(__global float* ArrayA, __global float* ArrayB, __global float* result,__global float* len)
            {
                int i = get_global_id(0);
                float sum = 0;
    
                for (int j = 0; j < len[0]; j++)
                {
                    sum += ArrayA[i + j] * ArrayB[j];
                }
                result[i] = sum;
            }";
            Program program = GPUModule.addProgram(context,kernelSource,device);
            Kernel kernel = GPUModule.addKernel(program,"conv",new IMem[]{signalBuffer,kernelBuffer,resultBuffer,lengthBuffer});
            GPUModule.executeKernel(commandQueue,kernel,signal.Length);
            result = GPUModule.readData(commandQueue,resultBuffer,signal.Length- filter.Length + 1);
            // Console.WriteLine(" ");
            Console.WriteLine("Result of 1D Convolution GPU of First Element :" + result[0]);
            Console.WriteLine("Result of 1D Convolution GPU of Last Element :" + result[signal.Length-filter.Length]);
            // Console.WriteLine("Result of 500th number "); 
            // foreach (float value in result)
            // {
            //     Console.Write(value + " ");
            // }


    }
}
}
