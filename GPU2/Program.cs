using System;
using System.Diagnostics;
using GPUoptimization;
using OpenCL.Net;


namespace OpenCLDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get available platforms and devices
            Context context;
            CommandQueue commandQueue;
            Device device;
            int platFormIndex = 0;
            int DeviceIndexInThisPlatForm = 0;
            GPUModule.InitializeGPU(platFormIndex, DeviceIndexInThisPlatForm, out context, out commandQueue, out device);
            //     Platform[] platforms = Cl.GetPlatformIDs(out _);
            // Device[] devices = Cl.GetDeviceIDs(platforms[0], DeviceType.Gpu, out _);

            // // Create a context and command queue
            // var context = Cl.CreateContext(null, (uint)devices.Length, devices, null, IntPtr.Zero, out _);
            // var commandQueue = Cl.CreateCommandQueue(context, devices[0], CommandQueueProperties.None, out _);

            // Create input data
            const int ArraySize = 90000000;
            int loopsize = 30;
            var arrayA = new float[ArraySize];
            var arrayB = new float[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                arrayA[i] = 3;
                arrayB[i] = 3;
            }
            var ctimearr = new float[loopsize];
            var gtimearr = new float[loopsize];
            for (int K = 0;K<loopsize;K++)
            {

            
            Stopwatch Cpu = new Stopwatch();
            var resultArrayCpu = new float[ArraySize];
            Cpu.Start();
            for (int i = 0; i < ArraySize; i++)
            {
                resultArrayCpu[i] = arrayA[i] * arrayB[i];
            }
            Cpu.Stop();
            Console.WriteLine($"the time of cpu in ms : {Cpu.ElapsedMilliseconds}");
            ctimearr[K] = Cpu.ElapsedMilliseconds;

            // Create OpenCL buffers
            var bufferA = Cl.CreateBuffer(context, MemFlags.ReadOnly, (IntPtr)(ArraySize * sizeof(float)), IntPtr.Zero, out _);
            var bufferB = Cl.CreateBuffer(context, MemFlags.ReadOnly, (IntPtr)(ArraySize * sizeof(float)), IntPtr.Zero, out _);
            var bufferC = Cl.CreateBuffer(context, MemFlags.WriteOnly, (IntPtr)(ArraySize * sizeof(float)), IntPtr.Zero, out _);
            // Write data to buffers
            Cl.EnqueueWriteBuffer(commandQueue, bufferA, Bool.True, IntPtr.Zero, (IntPtr)(ArraySize * sizeof(float)), arrayA, 0, null, out _);
            Cl.EnqueueWriteBuffer(commandQueue, bufferB, Bool.True, IntPtr.Zero, (IntPtr)(ArraySize * sizeof(float)), arrayB, 0, null, out _);

            // Create and build the OpenCL kernel
            const string kernelSource = @"
                __kernel void add_arrays(__global const float* a, __global const float* b, __global float* c) {
                    int gid = get_global_id(0);
                    c[gid] = a[gid] * b[gid];

                }";

            var program = Cl.CreateProgramWithSource(context, 1, new[] { kernelSource }, null, out _);
            Cl.BuildProgram(program, 1,new[] {device} , string.Empty, null, IntPtr.Zero);
            //uildProgram(Program program, uint numDevices, Device[] deviceList, string options, ProgramNotify pfnNotify, nint userData);

            var kernel = Cl.CreateKernel(program, "add_arrays", out _);

            // Set kernel arguments
            Cl.SetKernelArg(kernel, 0, bufferA);
            Cl.SetKernelArg(kernel, 1, bufferB);
            Cl.SetKernelArg(kernel, 2, bufferC);

            // Execute the kernel
            var globalWorkSize = new IntPtr(ArraySize);
            Stopwatch Gpu = new Stopwatch();
            Gpu.Start();
            Cl.EnqueueNDRangeKernel(commandQueue, kernel, 1, null, new IntPtr[] { globalWorkSize }, null, 0, null, out _);
            Gpu.Stop();
            // Read the results back to host
            var resultArray = new float[ArraySize];
            Stopwatch read = new Stopwatch();
            read.Start();
            Cl.EnqueueReadBuffer(commandQueue, bufferC, Bool.True, IntPtr.Zero, (IntPtr)(300 * sizeof(float)), resultArray, 0, null, out _);
            read.Stop();
            Console.WriteLine($"the time of read in ms : {read.ElapsedMilliseconds}");
            // Print the result
            Console.WriteLine($"the time of gpu in ms : {Gpu.ElapsedMilliseconds}");
            gtimearr[K] = Gpu.ElapsedMilliseconds;
            // Release resources
            Cl.ReleaseMemObject(bufferA);
            Cl.ReleaseMemObject(bufferB);
            Cl.ReleaseMemObject(bufferC);
            Cl.ReleaseKernel(kernel);
            Cl.ReleaseProgram(program);
            Cl.ReleaseCommandQueue(commandQueue);
            Cl.ReleaseContext(context);
            Console.WriteLine(resultArray[400]);
            }
            float r1=0;
            float r2=0;
            for(int i=0;i<loopsize;i++){
                r1 += ctimearr[i];
                r2 += gtimearr[i];
            }
            r1 = r1/loopsize;
            r2 = r2/loopsize;
            Console.WriteLine($"CPU : {r1}");
            Console.WriteLine($"GPU : {r2}");
        }
    }
}
