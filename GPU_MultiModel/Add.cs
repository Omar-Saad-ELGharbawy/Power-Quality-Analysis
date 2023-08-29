
using System;
using OpenCL.Net;
using OpenCL.Net.Extensions;
using GPUoptimization;
using System.Diagnostics;
using MainClass;
namespace Summation
{
    class Summation
    {

        IMem bufferA;
        IMem bufferB;
        public void SetParameters(IMem dataA,IMem dataB){
             bufferA= dataA;
             bufferB = dataB;  
        }
        public void Sum(CommandQueue commandQueue1,Context context1,Device device1)
        {
            IMem bufferresult = GPUModule.addBuffer(context1,commandQueue1,new float[32000]);
            const string kernelSin = @"
                __kernel void add(__global float* ArrayA, __global float* ArrayB, __global float* result) {
                    int gid = get_global_id(0);
                    result[gid] = ArrayA[gid]+ArrayB[gid];
                }";
            Program program2 =  GPUModule.addProgram(context1,kernelSin,device1);
            Kernel kernel2 = GPUModule.addKernel(program2,"add",new[] {bufferA,bufferB,bufferresult});
            GPUModule.executeKernel(commandQueue1,kernel2,32000);
            MainClass.MainClass.bufferresult = bufferresult;
        }
    }
}