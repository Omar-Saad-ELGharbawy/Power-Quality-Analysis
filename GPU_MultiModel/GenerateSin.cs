
using System;
using OpenCL.Net;
using OpenCL.Net.Extensions;
using GPUoptimization;
using System.Diagnostics;
using MainClass;
namespace Generate
{
    class GenerateSignal
    {
        float [] amplitude = new float[1];
        float [] frequency = new float[1];
        float [] Samplingfrequency = new float[1];
        float [] result = new float[32000];
        public void SetParameters(float amp,float freq,float FS){  
            amplitude[0] = amp;
            frequency[0] = freq;
            Samplingfrequency[0] = FS;
        }
        public void Generate(CommandQueue commandQueue1,Context context1,Device device1)
        {
            IMem bufferAmp = GPUModule.addBuffer(context1,commandQueue1,amplitude);
            IMem bufferFreq = GPUModule.addBuffer(context1,commandQueue1,frequency);
            IMem bufferFS = GPUModule.addBuffer(context1,commandQueue1,Samplingfrequency);
            IMem bufferresult = GPUModule.addBuffer(context1,commandQueue1,result);
            const string kernelSin = @"
                __kernel void Generate_Sin(__global float* amp, __global float* freq,__global float* FS, __global float* result) {
                    int gid = get_global_id(0);
                    result[gid] = amp[0]*sin(2*3.14*freq[0]*gid/FS[0]);
                }";
            Program program1 =  GPUModule.addProgram(context1,kernelSin,device1);
            Kernel kernel1 = GPUModule.addKernel(program1,"Generate_Sin",new[] {bufferAmp,bufferFreq,bufferFS,bufferresult});
            GPUModule.executeKernel(commandQueue1,kernel1,result.Length);
            MainClass.MainClass.bufferresult = bufferresult;
        }
    }
}