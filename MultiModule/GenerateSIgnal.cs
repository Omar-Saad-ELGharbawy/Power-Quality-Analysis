
using System;
using OpenCL.Net;
using OpenCL.Net.Extensions;
using GPUoptimization;
using System.Diagnostics;
using System.Net.Http.Headers;
using FourierTransform;
namespace Generate
{
    class GenerateSignal
    {
        // public static
        public static void Excute(CommandQueue commandQueue1,Context context1,Device device1)
        {
            // CommandQueue commandQueue1;
            // Context context1;
            // Device device1;
            // int platFormIndex = 0;
            // int DeviceIndexInThisPlatForm = 0;
            // // Stopwatch all = new Stopwatch();
            // // all.Start();
            // GPUModule.InitializeGPU(platFormIndex, DeviceIndexInThisPlatForm, out context1, out commandQueue1, out device1);
            float [] amp = {7};
            float [] freq = {10};
            float [] FS = {32000};
            float [] result = new float[32000];
            float [] result2 = new float[32000];
            IMem bufferAmp = GPUModule.addBuffer(context1,commandQueue1,amp);
            IMem bufferFreq = GPUModule.addBuffer(context1,commandQueue1,freq);
            IMem bufferFS = GPUModule.addBuffer(context1,commandQueue1,FS);
            IMem bufferresult = GPUModule.addBuffer(context1,commandQueue1,result);
            const string kernelSin = @"
                __kernel void add(__global float* amp, __global float* freq,__global float* FS, __global float* result) {
                    int gid = get_global_id(0);
                    result[gid] = amp[0]*sin(2*3.14*freq[0]*gid/FS[0]);
                }";
            Program program1 =  GPUModule.addProgram(context1,kernelSin,device1);
            Kernel kernel1 = GPUModule.addKernel(program1,"add",new[] {bufferAmp,bufferFreq,bufferFS,bufferresult});
            GPUModule.executeKernel(commandQueue1,kernel1,result.Length);
            // result = GPUModule.readData(commandQueue1,bufferresult,result.Length);
            Master.BufferSin = bufferresult;
            //  Console.WriteLine($"Class Gen  {result[500]}");    
            // float [] FS = {2000};
            // float [] Mags = new float[2000];
            // float [] Freqs = new float[2000];
            // IMem bufferFS = GPUModule.addBuffer(context1,commandQueue1,FS);
            // IMem bufferMags = GPUModule.addBuffer(context1,commandQueue1,Mags);
            // IMem bufferFreqs = GPUModule.addBuffer(context1,commandQueue1,Freqs);
            // const string KernelFFt = @"
            //     __kernel void FFT(__global float* Signal,__global float* FS, __global float* Mags,__global float* Freqs) {
            //         int gid = get_global_id(0);
            //         Mags[gid] = Signal[gid]*FS[0]/1000;
            //     }";
            // Program program2 =  GPUModule.addProgram(context1,KernelFFt,device1);
            // Kernel kernel2 = GPUModule.addKernel(program2,"FFT",new[] {bufferresult,bufferFS,bufferMags,bufferFreqs});
            // GPUModule.executeKernel(commandQueue1,kernel2,Freqs.Length);
            // Master.BufferRes = bufferMags;
            // result2 = GPUModule.readData(commandQueue1,bufferMags,2000);
            // Console.WriteLine("Data Here");
            // // 
            // all.Stop();
            // Console.WriteLine("Time taken: {0}ms",all.Elapsed.TotalMilliseconds);
            // Cl.ReleaseMemObject(bufferAmp);
            // Cl.ReleaseMemObject(bufferFreq);
            // Cl.ReleaseMemObject(bufferresult);
            // Cl.ReleaseKernel(kernel1);
            // Cl.ReleaseProgram(program1);
            // Cl.ReleaseCommandQueue(commandQueue1);
            // Cl.ReleaseContext(context1);
            // Console.WriteLine("Done class1");
            // // for(int i=0;i<100;i++){
           
            // }
        }
    }
}