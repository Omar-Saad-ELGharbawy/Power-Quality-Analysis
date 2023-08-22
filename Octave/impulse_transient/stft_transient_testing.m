clc
clear all
close all

pkg load signal

############################# Transient Sin Signal Generation ###########################
#Constant parameters
sampling_rate = 32000; % Sampling rate 32K Hz
[ac_signal,ac_time] = generate_power_buffer();

start_time=0.005; %t1 start duration

transient_amplitude = 7000 *2.91;
transient_frequency = 6000; #transient frequency of 3.5K HZ
transient_signal = generate_transient_sin(ac_time,transient_amplitude,transient_frequency,start_time);

% AC power signal with 220 Volt, 50 Hz, Sampling Rate 32KHz, Contaning Transient at Start time
power_transient = ac_signal + transient_signal;

############################### Plots ##########################
figure
subplot(3,1,1);
plot(ac_time,ac_signal)
title('Pure 50 Hz Sine wave')

subplot(3,1,2);
plot(ac_time,transient_signal)
title('Transient')

subplot(3,1,3);
plot(ac_time,power_transient)
title('Power Transient')


################ Short Time Fourier Transform (specgram) ###################
#samples_num = 80;

#figure
#specgram(transient_signal,samples_num,sampling_rate)
#xlabel('Time in [sec]')
#ylabel('Frequency in [HZ]')
#grid on
#title('Tran STFT')


####################################
#figure
#specgram(power_transient,samples_num,sampling_rate);
##S = specgram(power_transient,samples_num,sampling_rate);
#xlabel('Time in [sec]');
#ylabel('Frequency in [HZ]');
#grid on
#title('Power Transient STFT');


% figure
% #Fs=1000;
% #x = chirp([0:1/Fs:2],0,2,500);  # freq. sweep from 0-500 over 2 sec.
% #power_transient

% window_time = 2
% step=ceil(31.25*sampling_rate/1000000);    # one spectral slice every 1 us
% window=ceil(window_time*sampling_rate/1000); # 1 ms data window

% ## test of automatic plot
% #specgram (x, n, Fs, window, overlap)
% #[S2, f2, t2] = specgram(power_transient);
% specgram(power_transient, 2^nextpow2(window), sampling_rate, window, window-step);


% #[S1, f1, t1] = specgram(power_transient,80,sampling_rate);
% [S2, f2, t2] = specgram(power_transient, 2^nextpow2(window), sampling_rate, window, window-step);
% magnitude = abs(S2);
% mag_mean = mean(magnitude, 1);

% t3 =t2 +(window_time/1000)/2;
% t4 =t2 +(window_time/1000);
