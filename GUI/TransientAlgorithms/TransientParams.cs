using System;

/*
TransientParams Class Documentation:
----------------------------------
This class contains the parameters for the transient signal.
    The parameters are:
        - StartTime: The start time of the transient signal.
        - Frequency: The frequency of the transient signal.
        - Amplitude: The amplitude of the transient signal. 
 */
namespace TransientParamsClass
{
    public class TransientParams
    {
        public double StartTime { get; set; }
        public double Frequency { get; set; }
        public double Amplitude { get; set; }
    }

}