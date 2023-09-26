using System;
using OneDConvolutionExample;
using OpenCL.Net;
using OpenCL.Net.Extensions;
using GPUoptimization;
using OneDConvolutionGPU;
using System.Diagnostics;

class MainClass{
    
    public static void Main(){
        Stopwatch GPUInit = new Stopwatch();
        GPUInit.Start();
        int platformIndex = 0;
        int deviceIndex = 0;
        CommandQueue commandQueue;
        Context context;
        Device device;
        GPUModule.InitializeGPU(platformIndex,deviceIndex,out context,out commandQueue,out device);
        GPUInit.Stop();
        for(int j=1;j<=10;j++){
        int signal_length = 1000000*j;
        int kernel_length = 100;
        float[] signal = new float[signal_length];
        float[] kernel = new float [kernel_length];
        for(int i=0;i<signal.Length;i++){
            signal[i]=0.0000095f*i;
        }
        for(int i=0;i<kernel.Length;i++){
            kernel[i]=0.1f;
        }
        Stopwatch cpu = new Stopwatch();
        cpu.Start();
        ConvolutionCPU.conv(signal,kernel);
        cpu.Stop();
        // Console.WriteLine(" ");
        Console.WriteLine("CPU Time: "+cpu.ElapsedMilliseconds+" ms");
        Stopwatch gpu = new Stopwatch();
        gpu.Start();
        ConvolutionGPU.conv(context,commandQueue,device,signal,kernel);
        gpu.Stop();
        // Console.WriteLine(" ");
        Console.WriteLine("GPU Time: "+(gpu.ElapsedMilliseconds+(GPUInit.ElapsedMilliseconds/10))+" ms");
        // Console.WriteLine(" Filter Size  of "+j*100);
        Console.WriteLine(" Signal Size  of "+j*100000);
        Console.WriteLine("====================================");
        }
        // Cl.ReleaseContext(context);
        // Cl.ReleaseCommandQueue(commandQueue);
        }
}