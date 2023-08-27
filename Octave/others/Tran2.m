clc
clear all
close all


% Parameters
mu = 0;         % Mean of the Gaussian distribution
width=0.01;
sigma = (width/2) /pi;      % Standard deviation of the Gaussian distribution
duration = 0.5;   % Duration of the signal in seconds


sampling_rate = 32000;  % Sampling rate (samples per second)
amplitude = 1; % Amplitude of the sine wave
frequency = 1000;  % Frequency of the sine wave

% Generate time axis
t = -duration : 1/sampling_rate : duration;

% Generate Gaussian signal using custom normpdf function
gaussian_signal = my_normpdf(t, mu, sigma);
#gaussian_signal = gaussian_signal/(sigma*0.4);
% Generate sine wave
sine_wave = amplitude * sin(2*pi*frequency*t);

% Add sine wave to Gaussian signal
combined_signal = gaussian_signal .* sine_wave;

% Create a figure and divide it into subplots
figure;

% First subplot - Gaussian signal
subplot(3, 1, 1);
plot(t, gaussian_signal);
xlabel('Time (s)');
ylabel('Amplitude');
title('Gaussian Signal');

% Second subplot - Sine wave
subplot(3, 1, 2);
plot(t, sine_wave);
xlabel('Time (s)');
ylabel('Amplitude');
title('Sine Wave');

% Third subplot - Combined signal
subplot(3, 1, 3);
plot(t, combined_signal);
xlabel('Time (s)');
ylabel('Amplitude');
title('Combined Signal');
