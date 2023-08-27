
using System;
using OpenCL.Net;
namespace GPUoptimization
{
    class GPUModule
    {
        // public static void InitializeGPU(out Context context, out CommandQueue commandQueue, out Device device)
        // {
        //     Platform[] platforms= Cl.GetPlatformIDs(out _);
        //     Device[] devices = Cl.GetDeviceIDs(platforms[0], DeviceType.Gpu, out _);
        //     device = devices[0];
        //     context = Cl.CreateContext(null, (uint)devices.Length, devices, null, IntPtr.Zero, out _);
        //     commandQueue = Cl.CreateCommandQueue(context, device, CommandQueueProperties.None, out _);
        // }
        // public static int GetDeviceCount()
        // {
        //     Platform[] platforms= Cl.GetPlatformIDs(out _);
        //     Device[] devices=Cl.GetDeviceIDs(platforms[0], DeviceType.Gpu, out _);
        //     return devices.Length;
        // }

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
    }
}