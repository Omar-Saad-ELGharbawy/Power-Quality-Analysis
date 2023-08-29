using System;
using GPUoptimization;
using OpenCL.Net;
using Generate;
using Summation;
using System.Diagnostics;

namespace MainClass{
    class MainClass{
        public static IMem bufferresult;
        static void Main(){
            Stopwatch alltime = new Stopwatch();
            GenerateSignal GeneSignal = new GenerateSignal();
            Summation.Summation SumSignal = new Summation.Summation();
            Context context;
            CommandQueue commandQueue;
            Device device;
            GPUModule.InitializeGPU(0,0,out context,out commandQueue,out device);
            alltime.Start();
            GeneSignal.SetParameters(10,1,32000);
            GeneSignal.Generate(commandQueue,context,device);
            SumSignal.SetParameters(bufferresult,bufferresult);
            SumSignal.Sum(commandQueue,context,device);
            float [] result = GPUModule.readData(commandQueue,bufferresult,32000);
            // Console.WriteLine(result[8000]);
            alltime.Stop();
            Console.WriteLine("Time taken: {0}ms",alltime.Elapsed.TotalMilliseconds);
            // for(int i=0;i<result.Length;i++){
                
            // }
        }
    }
}