using OpenCL.Net;
using GPUoptimization;
using FourierTransform;
using Generate;
using System.Diagnostics;
using Summation;
class Master
    {
        public static CommandQueue commandQueue1;
        public static Context context1;
        public static Device device1;
        public static IMem BufferSin ;
        public static IMem BufferFou;
        public static IMem BufferSum;
        static void Main(){
            int platFormIndex = 0;
            int DeviceIndexInThisPlatForm = 0;
            float [] hello = new float[32000];
            Stopwatch all = new Stopwatch();
            
            GPUModule.InitializeGPU(platFormIndex, DeviceIndexInThisPlatForm, out context1, out commandQueue1, out device1);
            all.Start();
            GenerateSignal.Excute(commandQueue1,context1,device1);
            // Fourier.pre(commandQueue1,context1,device1);
            Summation.Summation.ApplySum(commandQueue1,context1,device1);
            all.Stop();
            Console.WriteLine($"time of all codes : {all.ElapsedMilliseconds}");
            hello = GPUModule.readData(commandQueue1,BufferSin,hello.Length);
            // // for(int i=70;i<90;i++){
            Console.WriteLine($" in Main Bufferres after Gene class {hello[500]}");
            // }
            // hello = GPUModule.readData(commandQueue1,BufferFou,hello.Length);
            // for(int i=70;i<90;i++){
            // Console.WriteLine($" in Main Bufferres after Fou class {hello[500]}");
            hello = GPUModule.readData(commandQueue1,BufferSum,hello.Length);
            // // for(int i=70;i<90;i++){
            Console.WriteLine($" in Main Bufferres after Gene class {hello[500]}");
            // }
            
            // }
        }
    }