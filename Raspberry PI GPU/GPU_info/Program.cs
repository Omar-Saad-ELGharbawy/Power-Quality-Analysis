using OpenCL.Net;

namespace GPUInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the available platforms
            ErrorCode error;
            Platform[] platforms = Cl.GetPlatformIDs(out error);
            if (error != ErrorCode.Success)
            {
                Console.WriteLine("Error getting platform IDs: " + error);
                return;
            }

            Console.WriteLine("Available GPUs:");

            foreach (var platform in platforms)
            {
                // Get devices for the platform
                Device[] devices = Cl.GetDeviceIDs(platform, DeviceType.Gpu, out error);
                if (error != ErrorCode.Success)
                {
                    Console.WriteLine("Error getting GPU device IDs: " + error);
                    continue;
                }

                foreach (var device in devices)
                {
                    // Get device name
                    string deviceName = Cl.GetDeviceInfo(device, DeviceInfo.Name, out error).ToString();

                    if (error != ErrorCode.Success)
                    {
                        Console.WriteLine("Error getting device info: " + error);
                        continue;
                    }
                    string platname = Cl.GetPlatformInfo(platform, PlatformInfo.Name, out error).ToString();
                    Console.WriteLine("Platform  Name: "+platname);
                    Console.WriteLine("Platform: " + platform + ", GPU: " + deviceName);
                }
            }
        }
    }
}
