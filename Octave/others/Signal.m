clc
clear all
close all

x= 0:0.001:1;
y=sin(2*pi*x);

#subplot(2,1,1)

% Set the parameters
frequency = 50; % Frequency in Hz
amplitude = 220; % Amplitude of the signal
duration = 1; % Duration of the signal in seconds
sampling_rate = 32000; % Sampling rate 32K Hz
t = linspace(0, duration, duration * sampling_rate);

% Generate the sine signal
signal = amplitude * sin(2 * pi * frequency * t);

% Plot the signal
plot(t, signal);
xlabel('Time (s)');
ylabel('Amplitude');
title('Sine Signal with 32K Hz Frequency');
