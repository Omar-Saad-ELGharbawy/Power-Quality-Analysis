clc
clear all
close all

pkg load signal

#Constant parameters
sampling_rate = 32000; % Sampling rate 32K Hz

########################  AC Power Supply Signal #########################################
%Pure Normal 50 Hz AC sine wave
ac_amplitude =220;
ac_frequency = 50; % Frequency in Hz
#ac_duration = 2;
ac_duration = 0.2;

ac_time = linspace(0, ac_duration, ac_duration * sampling_rate);
ac_signal = ac_amplitude * sin(2 * pi * ac_frequency * ac_time);

########################## Transient ####################################
% Half sin signal with high frequency for transient signal
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
transient_amplitude = 5000;
transient_frequency = 3000; #transient frequency of 3.5K HZ
period = 1/transient_frequency; % period of one sine signal

transient_time = linspace(0, period, period * sampling_rate);
half_sine = transient_amplitude * sin(2 * pi * transient_frequency * ac_time);

% Transient Window
half_period = 1 / (transient_frequency*2);

t1=0.105; %t1 start duration

t2= t1 + half_period; %t2 end duration
ty= (t1+t2)/2; %ty decay time

rec_window = window(ac_time,t1,t2, 1);
exp_signal = exp(-ac_time/ty);

transient_signal= rec_window .*exp_signal .*half_sine;
#transient_signal= rec_window .*half_sine ;

final= ac_signal + transient_signal;

samples_num = 20;

#figure
#specgram(ac_signal,samples_num,sampling_rate)
#xlabel('Time in [sec]')
#ylabel('Frequency in [HZ]')
#grid on
#title('AC STFT')

figure
specgram(final,samples_num,sampling_rate);
#S = specgram(final,samples_num,sampling_rate);
xlabel('Time in [sec]');
ylabel('Frequency in [HZ]');
grid on
title('Transient STFT');

[S, f, t] = specgram(final,80,sampling_rate);

#[S2, f2, t2] = specgram(final,100,sampling_rate);

%%dshgs

