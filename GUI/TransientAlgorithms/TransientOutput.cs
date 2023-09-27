using System;

/*
TransientOutput Class Documentation:
----------------------------------
This class contains the parameters of a transient analysis output.
    The parameters are:
        - StartTime: The time at which the transient starts.
        - Width: The width of the transient.
        - Peak: The peak of the transient.
 */
namespace TransientOutputClass
{
    public class TransientOutput
    {
        public double StartTime { get; set; }
        public double Width { get; set; }
        public double Peak { get; set; }
    }

}