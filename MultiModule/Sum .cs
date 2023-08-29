
using System;
using OpenCL.Net;
using OpenCL.Net.Extensions;
using GPUoptimization;
using Generate;

namespace Summation
{
    class Summation
    {
        public static void ApplySum(CommandQueue commandQueue1,Context context1,Device device1){
            float [] iter = new float[32000];
            IMem buffer = GPUModule.addBuffer(context1,commandQueue1,iter);
            const string Summation = @"
                __kernel void Sum(__global float* Signal,__global float* Fourier, __global float* Result) {
                    int gid = get_global_id(0);
                    Result[gid] = Signal[gid] + Fourier[gid];
                }";  
            Program program3 =  GPUModule.addProgram(context1,Summation,device1);
            Kernel kernel3 = GPUModule.addKernel(program3,"Sum",new[] {Master.BufferSin,Master.BufferSin,buffer});
            GPUModule.executeKernel(commandQueue1,kernel3,iter.Length);
            // iter = GPUModule.readData(commandQueue1,buffer,iter.Length);
            Master.BufferSum = buffer;
        }
    }
}