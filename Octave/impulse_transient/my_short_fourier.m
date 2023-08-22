clc
clear all
close all

pkg load signal


% Load the signal

% Define parameters
#Constant parameters
fs = 32000; % Sampling rate 32K Hz

############## Time Signal #################
% Define time vector
duration = 0.2
t = linspace(0, duration, duration*fs);

###########  AC 50 HZ Power Supply Signal ########################
ac_amplitude =1;
ac_frequency = 50; % Frequency of sinewave in Hz
theta_A = 0;  % Phase angle of sinewave

% Calculate the sinewave component
y = ac_amplitude*sin(2*pi*ac_frequency*t + theta_A);

###################################################################################################


% Parameters for the STFT
windowSize = 100;    % Size of the window (in samples)
overlap = 0.75;      % Overlap between consecutive windows (as a fraction of windowSize)
nfft = 1024;         % Number of FFT points (optional, can be set to windowSize for default)

% Compute the STFT
#[S, F, T] = spectrogram(y, windowSize, round(overlap*windowSize), nfft, fs);
tran_stft = stft (y, windowSize);
#S = stft (y)


% Plot the spectrogram
#figure;
#imagesc(T, F, 20*log10(abs(S)));   % Convert to decibels for better visualization
#axis xy;
#xlabel('Time (s)');
#ylabel('Frequency (Hz)');
#title('Short-Time Fourier Transform');
#colorbar;
