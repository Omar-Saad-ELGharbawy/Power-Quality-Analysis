clc
clear all
close all
pkg load signal

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Start Signals Simulations %%%%%%%%%%%%%%%%%%%%%%%%%
%%%%%%%%%% Generations of AC power Signal With Harmonics and Transients %%%%%%%%%%%
% Signal Parameters
sampling_rate = 32000; #32 KHz sampling frequency
start_time = 0;
end_time = 0.2;   # 200 milli second duration buffer
amplitude = 220;   # 220 Volts Amplitude
frequency = 50;        # 50 Hz Frequency
phase_shift = 0;
harmonics_num = 5;   # Nuimber of harmonics in the signal
% generate AC signal
[ac_time,ac_signal] = generateSin(start_time,end_time,amplitude,frequency,sampling_rate,phase_shift);
% generate Harmionics signal
harmoincs = generateHarmonics(ac_signal,start_time,end_time,amplitude,frequency,sampling_rate,harmonics_num);

################# Generate Multiple Sin Transients ##################
% Transient Parameters
# Transient 1
transient_time_1 = 0.005;
transient_freq_1 = 6000;
transient_amp_1 = 4000 *2.91;
# Transient 2
transient_time_2 = 0.1;
transient_freq_2 = 4500;
transient_amp_2 = 6000 *2.91;
# Transient 3
transient_time_3 = 0.15;
transient_freq_3 = 3000;
transient_amp_3 = 8000 *2.91;
% generate Transients Signals
transient_signal_1 = generateTransientSignal(ac_time,transient_amp_1,transient_freq_1,transient_time_1);
transient_signal_2 = generateTransientSignal(ac_time,transient_amp_2,transient_freq_2,transient_time_2);
transient_signal_3 = generateTransientSignal(ac_time,transient_amp_3,transient_freq_3,transient_time_3);
all_transients = transient_signal_1 + transient_signal_2 + transient_signal_3;

######### All signals ###########
all_signals = harmoincs + all_transients;

% %%%%%%%%%%%%%%%%%%%%%%%%%% Time Domain Plots %%%%%%%%%%%%%%%%
figure;
subplot(4,1,1);
plot(ac_time,ac_signal)
grid on;
title('AC Signal')

subplot(4,1,2);
plot(ac_time,harmoincs)
grid on;
title('Harmonics')

subplot(4,1,3);
plot(ac_time, all_transients);
grid on;
title('Transient');

subplot(4,1,4);
plot(ac_time,all_signals)
grid on;
title('All Signals')
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% END of Signals Simulations %%%%%%%%%%%%%%%%%%%%%%%%%

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% START of Signal Analysis %%%%%%%%%%%%%%%%%%%%%%%%%

################ Short Time Fourier Transform (specgram) ###################
window_time = 2; #Window Time in ms (Each ms contains 32 points)
window=ceil(window_time*sampling_rate/1000); #2 ms data window
step=ceil(31.25*sampling_rate/1000000);   #One spectral slice every 31.25 us
nfft = 2^nextpow2(window);  #Number of window Points
% %%%%%%%%%% Specgram Plot %%%%%%%%%%%%%%%%
figure
specgram(all_signals, nfft, sampling_rate, window, window-step);
xlabel('Time in [sec]');
ylabel('Frequency in [HZ]');
grid on
title('Power Transient STFT');
% Custom color map
colormap(jet);
colorbar;

# Calculate STFT
[all_S, all_f, all_t] = specgram(all_signals, nfft, sampling_rate, window, window-step);
all_magnitude = abs(all_S);
all_mag_mean = mean(all_magnitude, 1);

sub_f = all_f(7:12);
all_S_F3000 = all_S(7:12, :);
all_magnitude_F3000 = abs(all_S_F3000);
all_mag_mean_F3000 = mean(all_magnitude_F3000,1);

all_t = all_t - 3.125e-05;
all_t_shifted =all_t +(window_time/1000)/2;

############# STFT Transient Localization #############

% search in Mean Magnitude of (3000-6000)Hz STFT and find : - number of  transients, -index of each transient start
[transients_count , transients_stft_start_indices ] = findTransients(all_mag_mean_F3000, 11);

#get the time of start indices from stft domain to get it in the time domain
transient_sart_time = all_t_shifted(transients_stft_start_indices);
transient_sart_indices = int64((sampling_rate * transient_sart_time) +30);

# How to put the minimum threshold to calculate the transient width  ?????

#Remove fundmental effect from transients
transient_without_fundmental = all_signals - ac_signal;
# Find transients above the minimum threshold
#User Input
transient_min_threshold = 100;
[ transients_values , transients_times ] = ExtractTransients(transient_without_fundmental, ac_time, transient_sart_indices, transient_min_threshold);

######## Width Calculation ############
widths = getTransientWidths(transients_values);
######## Peak Values Calculation in time domain ######
old_peak_values = getTransientPeaks_old(transients_values);

############################# New Method For Peak Width Calculation ###############################
% ########## Low Pass Fourier Filter #####################

cutt_off_freq = 3000;
low_pass_signal = lowPassFourierFitler(all_signals,sampling_rate,cutt_off_freq);
% ######## Subtract Signals to get transient with removing all other signals #####
filtered_signal = all_signals - low_pass_signal ;

% %%%%%%%%%%%%%%%%%%%%%%%%%% Fourier Filtered Plots %%%%%%%%%%%%%%%%
figure;
subplot(2,1,1);
plot(ac_time,low_pass_signal)
grid on;
title('Low Pass Signal')

subplot(2,1,2);
plot(ac_time,filtered_signal)
grid on;
title('Filtered Signal')

% Calculate peaks from subtracted signal from low pass filtered signal to get
% the the peak from positive and negative peaks them calculate transient peak
#transient_subtracted_buffers = {};
transient_peaks = [];
for i = 1:numel(transient_sart_indices)
  tran_i_buffer = filtered_signal(transient_sart_indices(i):transient_sart_indices(i)+length(transients_values{i})+1);
  transient_peak = findPeaks(tran_i_buffer);
  #transient_subtracted_buffers = [transient_subtracted_buffers;tran_i_buffer];
  transient_peaks = [transient_peaks; transient_peak];
end


#################### 
# Inputs : Signal Buffer , Minimum Threshoild, Peak Threshold
# Outputs : Transient Peaks with 2 methods, Transient Width, Transient Buffers

