clc
clear all
close all
pkg load signal

############################# AC Sin Signal Generation ###########################
sampling_rate = 32000;
% 1st time start 2nd time end 3rd amplitude 4th frequency 5th Sampling freq
[ac_time,ac_signal] = generate_sin(0,0.2,220,50,sampling_rate,0);
% 0th magnitude of sin 1st time start 2nd time end 3rd amplitude 4th frequency 5th Sampling freq 6th harmonics number
harmoincs = generateharmonics(ac_signal,0,0.2,220,50,sampling_rate,5);

%##################  Transient Generation ######################################
start_time=0.005; %t1 start duration
transient_frequency = 6000;

# Sin Transient Signal
transient_amplitude = 4000 *2.91;
transient_signal = generate_transient_sin(ac_time,transient_amplitude,transient_frequency,start_time);

## RHO Transient Signal
#transient_amplitude = 2000;
#transient_amplitude = transient_amplitude + 77;
#transient_signal_rho = generate_transient_rho(ac_time,transient_amplitude,start_time);

all_signals = harmoincs + transient_signal;
############################# Plots ###########################
figure;
plot(ac_time,ac_signal)
title('Original AC Signal')

figure;
plot(ac_time,harmoincs)
title('Harmonics')

figure;
plot(ac_time,transient_signal)
title('Transient')

figure;
plot(ac_time,all_signals)
title('Transient + Harmonics ')

############################# Low Pass Fourier Filter ###########################
[filtered_signal] = fourier_fitler(all_signals,sampling_rate,3000);

figure;
% Plot the reconstructed Filtered signal
plot(ac_time, filtered_signal);
xlabel('Time (s)');
ylabel('Amplitude');
title('Reconstructed Filtered');

########################### Remove Filtered Signal ################
subtracted = all_signals - filtered_signal ;

figure;
subplot(2,1,1)
% Plot the transient Filtered signal
plot(ac_time, transient_signal);
xlabel('Time (s)');
ylabel('Amplitude');
title('transient Signal');

subplot(2,1,2)
% Plot the reconstructed Filtered signal
plot(ac_time, subtracted);
xlabel('Time (s)');
ylabel('Amplitude');
title('Subtracted Signal');



