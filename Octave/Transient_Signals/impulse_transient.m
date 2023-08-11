clc
clear all
close all

#Constant parameters
sampling_rate = 32000; % Sampling rate 32K Hz

########################  AC Power Supply Signal #########################################
%Pure Normal 50 Hz AC sine wave
ac_amplitude =220;
ac_frequency = 50; % Frequency in Hz
ac_duration = 4;

ac_time = linspace(0, ac_duration, ac_duration * sampling_rate);
ac_signal = ac_amplitude * sin(2 * pi * ac_frequency * ac_time);

############ 1
########################## Transient ####################################

###########  2
% Half sin signal with high frequency for transient signal
transient_amplitude = 5000;
transient_frequency = 6000; #transient frequency of 3.5K HZ
period = 1/transient_frequency; % period of one sine signal

transient_time = linspace(0, period, period * sampling_rate);
half_sine = transient_amplitude * sin(2 * pi * transient_frequency * ac_time);
###########  3
% Transient Window
half_period = 1 / (transient_frequency*2);
#t1=0; %t1 start duration
t1 = (1/200)+half_period; %time at peak of first ac signal
t2= t1 + half_period; %t2 end duration
ty= (t1+t2)/2; %ty decay time
ty = half_period/2

rec_window = window(ac_time,t1,t2, 1);
############  4
exp_signal = exp(-ac_time/ty);
#.*exp(-t/ty)
############  5
window_EXP= rec_window .*exp_signal;
############  6
sin_EXP= half_sine .*exp_signal;
############  7
#transient_signal= rec_window .*half_sine .*exp_signal;
transient_signal= rec_window .*half_sine ;
############  8
final= ac_signal + transient_signal;

[freq,mag] = generatefou(half_sine,sampling_rate);
subplot(3,2,1);
plot(freq,mag);
title('Half Sin')

[freq,mag] = generatefou(transient_signal,sampling_rate);
subplot(3,2,2);
plot(freq,mag);
title('Transient')

[freq,mag] = generatefou(final,sampling_rate);
subplot(3,2,3);
plot(freq,mag);
title('Full')



% subplot(4,2,1);
% plot(ac_time,ac_signal)
% title('Pure 50 Hz Sine wave')
% subplot(4,2,2);
% plot(ac_time,half_sine)
% title('Transient Sine')
% subplot(4,2,3);
% plot(ac_time,rec_window)
% title('Window')
% subplot(4,2,4);
% plot(ac_time,exp_signal)
% title('EXP')
% subplot(4,2,5);
% plot(ac_time,window_EXP)
% title('window_EXP')
% subplot(4,2,6);
% plot(ac_time,sin_EXP)
% title('sin_EXP')
% subplot(4,2,7);
% plot(ac_time,transient_signal)
% title('Transient')
% subplot(4,2,8);
% plot(ac_time,final)
% title('Final')

% % Adjust the spacing between subplots
% for i = 1:8
%     subplot(4, 2, i);
%     pos = get(gca, 'Position');
%     pos(3) = pos(3) + 0.05;
%     set(gca, 'Position', pos);
% end

% % Save the figure (optional)
% saveas(gcf, 'eight_graphs.png');
