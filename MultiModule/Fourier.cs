
using System;
using OpenCL.Net;
using OpenCL.Net.Extensions;
using GPUoptimization;
using Generate;

namespace FourierTransform
{
    class Fourier
    {
        public static void pre(CommandQueue commandQueue1,Context context1,Device device1){
            float [] FS = {32000};
            float [] Mags = new float[32000];
            float [] Freqs = new float[32000];
            float [] result2 = new float[32000];
            IMem bufferFS = GPUModule.addBuffer(context1,commandQueue1,FS);
            IMem bufferMags = GPUModule.addBuffer(context1,commandQueue1,Mags);
            IMem bufferFreqs = GPUModule.addBuffer(context1,commandQueue1,Freqs);
            const string KernelFFT = @"
        __kernel void FFT(__global float2* Signal,__global float* numSamples, __global float2* Result) {
            int gid = get_global_id(0);

            float2 sum = (float2)(0.0f, 0.0f);

            for (int k = 0; k < numSamples[0]; ++k) {
                float angle = -2.0f * M_PI * gid * k / numSamples[0];
                float2 exponent = (float2)(cos(angle), sin(angle));
                sum += Signal[k] * exponent;
            }

            Result[gid] = sum;
        }";  


            
            Program program2 =  GPUModule.addProgram(context1,KernelFFT,device1);
            // result2 = GPUModule.readData(commandQueue1,bufferMags,2000);
            // Console.WriteLine($" Result of reading magbuffer in fou class {result2[500]}");
            Kernel kernel2 = GPUModule.addKernel(program2,"FFT",new[] {Master.BufferSin,bufferFS,bufferMags});
            GPUModule.executeKernel(commandQueue1,kernel2,Freqs.Length);
            Master.BufferFou = bufferMags;
            // result2 = GPUModule.readData(commandQueue1,Master.BufferRes,2000);
            // Console.WriteLine($" Result of reading magbuffer in fou class {result2[500]}");
            
        }
    }
}