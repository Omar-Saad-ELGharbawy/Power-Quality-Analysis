function [ac_signal,ac_time] = generate_power_buffer()

#Constant parameters
sampling_rate = 32000; % Sampling rate 32K Hz

########################  AC Power Supply Signal #########################################
%Pure Normal 50 Hz AC sine wave
ac_amplitude =220;
ac_frequency = 50; % Frequency in Hz
ac_duration = 0.2;

ac_time = linspace(0, ac_duration, ac_duration * sampling_rate);
ac_signal = ac_amplitude * sin(2 * pi * ac_frequency * ac_time);

endfunction

