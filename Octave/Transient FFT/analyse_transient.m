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

# $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ TEST CASES $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

% Test Case 1  # Real Width = 83.3 us
start_time=0.005; %t1 start duration
transient_frequency = 6000;
% Peak Value = 4010 & Width = 62.5 us

% Test Case 2  # Real Width = 166.6 us
#start_time=0.005; %t1 start duration
#transient_frequency = 3000;
% Peak Value = 4177.6 & Width = 156.25 us

% Test Case 3 # Real Width = 83.3 us
#start_time=0.06;
#transient_frequency = 6000;
% Peak Value = 4278  & Width = 93.75 us

% Test Case 4 # Real Width = 166.6 us
#start_time=0.06;
#transient_frequency = 3000;
% Peak Value = 4183  & Width = 156.25 us

# Sin Transient Signal
transient_amplitude = 2200 *2.91;
transient_signal = generate_transient_sin(ac_time,transient_amplitude,transient_frequency,start_time);

## RHO Transient Signal
#transient_amplitude = 2000;
#transient_amplitude = transient_amplitude + 77;
#transient_signal_rho = generate_transient_rho(ac_time,transient_amplitude,start_time);

all_signals = harmoincs + transient_signal;
#all_signals = ac_signal + transient_signal;

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

% Plot the reconstructed Filtered signal
figure;
plot(ac_time, filtered_signal);
xlabel('Time (s)');
ylabel('Amplitude');
title('Reconstructed Filtered');

############################# Subtract Signals ###########################
subtracted = all_signals - filtered_signal ;

# Calculate Positive and Negative Peaks of the subtracted filtered signal
[positivePeak, negativePeak] = findPeaks(subtracted);

my_tran_peak = positivePeak - negativePeak;

figure;
plot(ac_time, subtracted);
xlabel('Time (s)');
ylabel('Amplitude');
title('Subtracted Signal');

################ STFT ###################
window_time = 2; #  Window Time in ms (Each ms contains 32 points)
step=ceil(31.25*sampling_rate/1000000);    # one spectral slice every 31.25 us
window=ceil(window_time*sampling_rate/1000); # 1 ms data window
nfft = 2^nextpow2(window);
% #specgram (x, n, Fs, window, overlap)

%%%%%%%%%%%%%%%%%%%%%%%%%% 3 Plots %%%%%%%%%%%%%%%%
figure;
subplot(3,1,1);
plot(ac_time,transient_signal)
grid on;
title('Transient')

subplot(3,1,2);
plot(ac_time,all_signals)
grid on;
title('All Signals')

subplot(3,1,3);
plot(ac_time, subtracted);
grid on;
title('Subtracted Signal');
%%%%%%%%%%%%%%%%%%%%%%%%%% All signal STFT %%%%%%%%%%%%%%%%
figure;
specgram(all_signals, nfft, sampling_rate, window, window-step);
xlabel('Time in [sec]');
ylabel('Frequency in [HZ]');
grid on
title('Power Transient STFT');
% Custom color map
colormap(jet);
colorbar;

[all_S, all_f, all_t] = specgram(all_signals, nfft, sampling_rate, window, window-step);
all_magnitude = abs(all_S);
all_mag_mean = mean(all_magnitude, 1);

all_S_F3000 = all_S(7:13, :);
all_magnitude_F3000 = abs(all_S_F3000);
all_mag_mean_F3000 = mean(all_magnitude_F3000, 1);

all_t = all_t - 3.125e-05;
all_t_shifted =all_t +(window_time/1000)/2;

% find index of transient elements in magnitude array
indices = find_indices_greater_than(5,all_mag_mean_F3000) ;

time_occurs = all_t_shifted(indices);
signal_index = (sampling_rate * time_occurs) +30;

transient_values = all_signals(signal_index(1):signal_index(end));
transient__time = ac_time(signal_index(1):signal_index(end));
############################## Subtract Filtered Signal ###################

% user_transient_threshold = 220;

% transient_values_only = transient_values -  220 * sin(2 * pi * 50 * transient__time );

#filtered_transient_values = get_elements_bigger_than(220,transient_values_only);

% peak_value = max(filtered_transient_values);

% length(filtered_transient_values);

% width = length(filtered_transient_values) * 3.125e-05;


