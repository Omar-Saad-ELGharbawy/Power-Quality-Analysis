clc
clear all
close all
pkg load signal

############################# AC Sin Signal Generation ###########################
sampling_rate = 32000; % Sampling rate 32K Hz
[ac_signal,ac_time] = generate_power_buffer();

############################# Generate Multiple Sin Transients ###########################
transient_time_1 = 0.005;
transient_freq_1 = 6000;
transient_amp_1 = 4000 *2.91;

transient_time_2 = 0.1;
transient_freq_2 = 4500;
transient_amp_2 = 6000 *2.91;

transient_time_3 = 0.15;
transient_freq_3 = 3000;
transient_amp_3 = 8000 *2.91;

transient_signal_1 = generate_transient_sin(ac_time,transient_amp_1,transient_freq_1,transient_time_1);
transient_signal_2 = generate_transient_sin(ac_time,transient_amp_2,transient_freq_2,transient_time_2);
transient_signal_3 = generate_transient_sin(ac_time,transient_amp_3,transient_freq_3,transient_time_3);

all_transients = transient_signal_1 + transient_signal_2 + transient_signal_3;

% AC power signal with 220 Volt, 50 Hz, Sampling Rate 32KHz, Contaning Multipe Transients
power_transient = ac_signal + all_transients;

############################### Plots ##########################
% figure
% subplot(3,1,1);
% plot(ac_time,ac_signal)
% title('Pure 50 Hz Sine wave')

% subplot(3,1,2);
% plot(ac_time,all_transients)
% title('Transient')

% subplot(3,1,3);
% plot(ac_time,power_transient)
% title('Power Transient')

################ Short Time Fourier Transform (specgram) ###################
window_time = 2; #  Window Time in ms (Each ms contains 32 points)

step=ceil(31.25*sampling_rate/1000000);    # one spectral slice every 31.25 us
window=ceil(window_time*sampling_rate/1000); # 1 ms data window
nfft = 2^nextpow2(window);

%%%%%%%%%%%%%%%%%%%%%%%%%% Transient Only STFT %%%%%%%%%%%%%%%%
% figure
% specgram(all_transients, nfft, sampling_rate, window, window-step);
% xlabel('Time in [sec]')
% ylabel('Frequency in [HZ]')
% grid on
% title('Transient Only STFT')
% % Custom color map
% colormap(jet);
% colorbar;

[tran_S, tran_f, tran_t] = specgram(all_transients, nfft, sampling_rate, window, window-step);
tran_magnitude = abs(tran_S);
tran_mag_mean = mean(tran_magnitude, 1);
tran_t2 =tran_t +(window_time/1000);

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

[sin_S, sin_f, sin_t] = specgram(ac_signal, nfft, sampling_rate, window, window-step);
sin_magnitude = abs(sin_S);
sin_mag_mean = mean(sin_magnitude, 1);
sin_t2 =sin_t +(window_time/1000);
%%%%%%%%%%%%%%%%%%%%%%%%% All signal STFT %%%%%%%%%%%%%%%%
% figure
% specgram(power_transient, nfft, sampling_rate, window, window-step);
% xlabel('Time in [sec]');
% ylabel('Frequency in [HZ]');
% grid on
% title('Power Transient STFT');
% % Custom color map
% colormap(jet);
% colorbar;

[all_S, all_f, all_t] = specgram(power_transient, nfft, sampling_rate, window, window-step);
all_magnitude = abs(all_S);
all_mag_mean = mean(all_magnitude, 1);

sub_f = all_f(7:end);
all_S_F3000 = all_S(7:12, :);
all_magnitude_F3000 = abs(all_S_F3000);
all_mag_mean_F3000 = mean(all_magnitude_F3000, 1);

all_t = all_t - 3.125e-05;
all_t_shifted =all_t +(window_time/1000)/2;

################################# Processing ###############################

% search inn all_mag_mean_F3000 and find (number of  transients, index of each transient start)
% #find number of transients in the signal
% transientCount = CalculateTransientCount(transient_values, 220)

[transients_count , transients_start_indices ] = find_transients(all_mag_mean_F3000, 2);

time_occurs = all_t_shifted(transients_start_indices);
signal_index_start = (sampling_rate * time_occurs) +30;

% % loop over signal index of transient start indices to make a 2d array of transients_count * number of transient values of each transient that exceeds 200
% for i = 1:length(signal_index)
%     transient_values(i,:) = power_transient(signal_index(i):signal_index(i)+transients_count-1);
% endfor

[ transients_values , transients_times ] = ExtractTransients(power_transient, ac_time, signal_index_start, 220);

[filtered_transient_values,filtered_transient_times ]= get_elements_bigger_than_cell(220, transients_values,transients_times);

transient_values_only = cell(size(filtered_transient_values));

for i = 1:numel(transient_values_only)
    sub_transient_values = filtered_transient_values{i};
    sub_transient_times = filtered_transient_times{i};
    transient_values_only{i} = sub_transient_values - 220 * sin(2 * pi * 50 * sub_transient_times);
end


widths = [];
for i = 1:numel(transient_values_only)
    values = transient_values_only{i};
    row_width = length(values) * 3.125e-05;
    widths = [widths; row_width];
end


peak_values = [];
for i = 1:numel(transient_values_only)
    values = transient_values_only{i};
    if ~isempty(values)
        row_peak = max(values);
    else
        row_peak = 0;
    end
    peak_values = [peak_values; row_peak];
end



