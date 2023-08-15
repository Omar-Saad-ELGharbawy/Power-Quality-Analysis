clc
clear all
close all

pkg load signal

############################# AC Sin Signal Generation ###########################
#Constant parameters
sampling_rate = 32000; % Sampling rate 32K Hz
[ac_signal,ac_time] = generate_power_buffer();

start_time=0.005; %t1 start duration

# RHO Transient Signal
%transient_amplitude = 7000;
%transient_amplitude = transient_amplitude + 77;
%transient_signal = generate_transient_rho(ac_time,transient_amplitude,start_time);

# Sin Transient Signal
transient_amplitude = 7000 *2.91;
transient_frequency = 6000; #transient frequency of 3.5K HZ
transient_signal = generate_transient_sin(ac_time,transient_amplitude,transient_frequency,start_time);


% AC power signal with 220 Volt, 50 Hz, Sampling Rate 32KHz, Contaning Transient at Start time
power_transient = ac_signal + transient_signal;

############################### Plots ##########################
% figure
% subplot(3,1,1);
% plot(ac_time,ac_signal)
% title('Pure 50 Hz Sine wave')

% subplot(3,1,2);
% plot(ac_time,transient_signal)
% title('Transient')

% subplot(3,1,3);
% plot(ac_time,power_transient)
% title('Power Transient')

################ Short Time Fourier Transform (specgram) ###################
window_time = 2; #  Window Time in ms (Each ms contains 32 points)

step=ceil(0.4*sampling_rate/1000);    # one spectral slice every 1 ms

#step=ceil(31.25*sampling_rate/1000000);    # one spectral slice every 31.25 us

window=ceil(window_time*sampling_rate/1000); # 1 ms data window
nfft = 2^nextpow2(window);
% #specgram (x, n, Fs, window, overlap)

%%%%%%%%%%%%%%%%%%%%%%%%%% Transient Only STFT %%%%%%%%%%%%%%%%
% figure
% specgram(transient_signal, nfft, sampling_rate, window, window-step);
% xlabel('Time in [sec]')
% ylabel('Frequency in [HZ]')
% grid on
% title('Transient Only STFT')
% % Custom color map
% colormap(jet);
% colorbar;

[tran_S, tran_f, tran_t] = specgram(transient_signal, 2^nextpow2(window), sampling_rate, window, window-step);
tran_magnitude = abs(tran_S);
tran_mag_mean = mean(tran_magnitude, 1);
tran_t2 =tran_t +(window_time/1000);

%find values in

%%%%%%%%%%%%%%%%%%%%%%%%%% Sin Only STFT %%%%%%%%%%%%%%%%
% figure
% specgram(ac_signal, nfft, sampling_rate, window, window-step);
% xlabel('Time in [sec]');
% ylabel('Frequency in [HZ]');
% grid on
% title('Sin Only STFT');
% % Custom color map
% colormap(jet);
% colorbar;

[sin_S, sin_f, sin_t] = specgram(ac_signal, 2^nextpow2(window), sampling_rate, window, window-step);
sin_magnitude = abs(sin_S);
sin_mag_mean = mean(sin_magnitude, 1);
sin_t2 =sin_t +(window_time/1000);
%%%%%%%%%%%%%%%%%%%%%%%%%% All signal STFT %%%%%%%%%%%%%%%%
% figure
% specgram(power_transient, nfft, sampling_rate, window, window-step);
% xlabel('Time in [sec]');
% ylabel('Frequency in [HZ]');
% grid on
% title('Power Transient STFT');
% % Custom color map
% colormap(jet);
% colorbar;

[all_S, all_f, all_t] = specgram(power_transient, 2^nextpow2(window), sampling_rate, window, window-step);
all_magnitude = abs(all_S);
all_mag_mean = mean(all_magnitude, 1);
all_t_shifted =all_t +(window_time/1000);

sub_f = all_f(7:end);

all_S_F3000 = all_S(7:end, :);
all_magnitude_F3000 = abs(all_S_F3000);
all_mag_mean_F3000 = mean(all_magnitude_F3000, 1);

% find index of transient elements in magnitude array
#indices = find(all_mag_mean_F3000 > 300);

indices = find_indices_greater_than(300,all_mag_mean_F3000);

#print(indices);
tran_time = all_t_shifted(indices);
tran_time_2 = floar(tran_time);
signal_index = (sampling_rate * tran_time) +1;


#indices(1);
############################

% [S2, f2, t2] = specgram(power_transient, 2^nextpow2(window), sampling_rate, window, window-step);
% magnitude = abs(S2);
% mag_mean = mean(magnitude, 1);
% t3 =t2 +(window_time/1000)/2;
% t4 =t2 +(window_time/1000);
