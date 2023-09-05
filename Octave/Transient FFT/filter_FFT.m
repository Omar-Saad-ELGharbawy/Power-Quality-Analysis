clc
clear all
close all
pkg load signal

% Generate a real-valued input signal
Fs = 32000;  % Sampling frequency
DURATION = 0.2;

#[time_1, signal_1] = generate_sine_wave(500, Fs, 0.2 ,220, 0);

[time_1, signal_1] = generate_sine_wave(50, Fs, DURATION,50, 0);
[time_2, signal_2] = generate_sine_wave(200, Fs, DURATION,100, pi/4);
[time_3, signal_3] = generate_sine_wave(500, Fs, DURATION,150, pi/2);

signal_1_2 = signal_1 + signal_2;
signals_1_2_3 = signal_1 + signal_2 + signal_3;
############################# Time Domain Plots ###########################
figure;
plot(time_1,signal_3)
title('Signal 3')

figure;
plot(time_1,signal_1_2)
title('Original 2 Signals')

figure;
plot(time_1,signals_1_2_3)
title('Original 3 Signals')
############################ Filter Signal #########################

[filtered_signal] = fourier_fitler(signals_1_2_3,Fs,500);

figure;
% Plot the reconstructed Filtered signal
plot(time_1, filtered_signal);
xlabel('Time (s)');
ylabel('Amplitude');
title('Reconstructed Filtered');

subtracted = signals_1_2_3 - filtered_signal ;

figure;
% Plot the reconstructed Filtered signal
plot(time_1, subtracted);
xlabel('Time (s)');
ylabel('Amplitude');
title('Subtracted');


