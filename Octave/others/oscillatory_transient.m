clc
clear all
close all

% Parameters
#Common Parameters

#gaussian parameter
mu = 0;         % Mean of the Gaussian distribution
width=0.2;     % Width of transient signal
g_duration = 1;   % Duration of the signal in seconds
g_sampling_rate = 32000;  % Sampling rate (samples per second)
g_amplitude = 1; % Amplitude of the sine wave
g_frequency = 50;  % Frequency of the sine wave

% generate time signal
#g_time = -g_duration : 1/g_sampling_rate : g_duration;
g_time = linspace(-g_duration, g_duration, g_duration * g_sampling_rate);
#g_time = linspace(0, g_duration, g_duration * g_sampling_rate);

% Generate sine wave
sine_wave = g_amplitude * sin(2*pi*g_frequency*g_time);

% First subplot - sine wave
#subplot(5, 1, 1);
#plot(g_time, sine_wave);
#title('Sine Wave');


% Generate Gaussian signal
gaussian_signal = generate_gaussian(g_time, mu, width, 0);
% Second subplot -  gaussian_signal
#subplot(5, 1, 2);
#plot(g_time,gaussian_signal );
#title('Gaussian Signal');

% Add sine wave to Gaussian signal
transient_signal = gaussian_signal .* sine_wave;
% Third subplot -  transient_signal
subplot(3, 1, 1);
plot(g_time, transient_signal);
title('Transient Signal');

# Original signal parameters
frequency = 50; % Frequency in Hz
amplitude = 3; % Amplitude of the signal

duration = 1; % Duration of the signal in seconds
sampling_rate = 32000; % Sampling rate 32K Hz
t = linspace(-duration, duration, duration * sampling_rate);

% Generate the sine signal
original_signal = amplitude * sin(2 * pi * frequency * t);
% Fourth subplot -  original_signal
subplot(3, 1, 2);
plot(t,original_signal );
title('original_signal');

combined_signal = original_signal + transient_signal
% Fifth subplot -  combined_signal
subplot(3, 1, 3);
plot(t,combined_signal );
title('combined_signal');

