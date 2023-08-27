clc
clear all
close all

# x(t) = A * sin(ωt + φ) * e^(-α * t)

% Parameters
sampling_rate = 10000;   % Sampling rate (samples per second)
phi = pi/4;      % Phase angle (in radians)

A = 1;           % Amplitude of the transient
alpha = 0.001;     % Decay coefficient
duration = 1;    % Duration of the signal in seconds

frequency = 50;  % frequency (in Hz)
omega = 2*pi*frequency;      % Angular frequency


% Generate time axis
t = 0 : 1/sampling_rate : duration/2;
t = 0 : 1/sampling_rate : duration/2;

% Generate transient signal
transient_signal = A * exp(-alpha * t) .* sin(omega*t + 0);

% Plot the transient signal
plot(t, transient_signal);
xlabel('Time (s)');
ylabel('Amplitude');
title('Transient Signal of Power Lines');
