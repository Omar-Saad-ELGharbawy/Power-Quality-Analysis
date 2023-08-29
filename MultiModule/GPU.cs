
using System;
using OpenCL.Net;
using OpenCL.Net.Extensions;

namespace GPUoptimization
{
    class GPUModule
    {
        // public static Context 
        public static void InitializeGPU(int platFormIndex,int deviceIndex,out Context context, out CommandQueue commandQueue, out Device device)
        {
            Platform[] platforms= Cl.GetPlatformIDs(out _);         
            if(platFormIndex>platforms.Length)
                throw new Exception("Device Index is out of range");
            Device[] devices=Cl.GetDeviceIDs(platforms[platFormIndex], DeviceType.Gpu, out _);
            device = devices[deviceIndex];
            
            context = Cl.CreateContext(null, (uint)devices.Length, devices, null, IntPtr.Zero, out _);
            commandQueue = Cl.CreateCommandQueue(context, device, CommandQueueProperties.None, out _);
        }
        public static IMem addBuffer(Context context,CommandQueue commandQueue,float []data){
            IMem buffer ;
            // if(ReadOnly){
                buffer = Cl.CreateBuffer(context, MemFlags.ReadWrite, (IntPtr)(data.Length * sizeof(float)), IntPtr.Zero, out _);
                Cl.EnqueueWriteBuffer(commandQueue, buffer, Bool.True, IntPtr.Zero, (IntPtr)( data.Length* sizeof(float)), data, 0, null, out _);
            // }else{
                // buffer = Cl.CreateBuffer(context, MemFlags.WriteOnly, (IntPtr)(data.Length * sizeof(float)), IntPtr.Zero, out _);
            // }
            return buffer;
        }
        public static Program addProgram(Context context,string kernelSource,Device device){
            Program program = Cl.CreateProgramWithSource(context, 1, new[] { kernelSource }, null, out _);
            Cl.BuildProgram(program, 1,new[] {device} , string.Empty, null, IntPtr.Zero);
            return program;
        }
        public static Kernel addKernel(Program program,string functionName,IMem []buffers){
            Kernel kernel = Cl.CreateKernel(program, functionName, out _);
            // Set kernel arguments
            for(uint i=0;i<buffers.Length;i++){
                Cl.SetKernelArg(kernel, i, buffers[i]);
            }
            return kernel;
        }
        public static void executeKernel(CommandQueue commandQueue,Kernel kernel,int ArraySize){
            Event KernelEvent;
            var globalWorkSize = new IntPtr(ArraySize);
            Cl.EnqueueNDRangeKernel(commandQueue, kernel, 1, null, new IntPtr[] { globalWorkSize },null, 0, null, out KernelEvent);
            Cl.WaitForEvents(1, new Event[] { KernelEvent });
        }
        public static float [] readData(CommandQueue commandQueue,IMem buffer,int dataSize){
            float [] data = new float[dataSize];
            Cl.EnqueueReadBuffer(commandQueue, buffer, Bool.True, IntPtr.Zero, (IntPtr)( dataSize * sizeof(float)), data, 0, null, out _);
            return data;
        }
    }
}