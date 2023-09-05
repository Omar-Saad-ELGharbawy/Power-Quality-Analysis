function [time, mag] = generate_sin(t1, t2, amplitude, freq, sampling_rate, phase_shift)
  duration = t2 - t1;
  time = linspace(t1, duration, duration * sampling_rate);
  phase = phase_shift / 360;  % Convert phase shift from degrees to radians
  mag = amplitude * sin(2 * pi * freq * time + phase);
endfunction



#function [time,mag] = generate_sin(t1,t2,amplitude,freq,sampling_rate);
#  duration = t2-t1;
#  time = linspace(t1, duration, duration * sampling_rate);
#  mag = amplitude * sin(2 * pi * freq * time);
#endfunction




#function sin_wave = generate_sin(Amplitude,frequency)
#  sin_wave = Amplitude*sin(2*pi*frequency*time);
#endfunction

