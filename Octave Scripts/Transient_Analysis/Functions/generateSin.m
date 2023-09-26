% generateSin Function Description :
%   Generates a sine wave with the given parameters
% Input Parameters:
%   t1: Start time of the signal
%   t2: End time of the signal
%   amplitude: Amplitude of the signal
%   freq: Frequency of the signal
%   sampling_rate: Sampling rate of the signal
%   phase_shift: Phase shift of the signal
% Output Parameters:
%   time: Time vector of the signal
%   mag: Magnitude vector of the signal
function [time, mag] = generateSin(t1, t2, amplitude, freq, sampling_rate, phase_shift)
  duration = t2 - t1;
  time = linspace(t1, duration, duration * sampling_rate);
  phase = phase_shift / 360;  % Convert phase shift from degrees to radians
  mag = amplitude * sin(2 * pi * freq * time + phase);
endfunction
