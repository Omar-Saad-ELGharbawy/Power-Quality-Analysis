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

# Generate Harmonics
harmoincs = generateharmonics(ac_signal,0,0.2,220,50,sampling_rate,5);

% AC power signal with 220 Volt, 50 Hz, Sampling Rate 32KHz, Contaning Multipe Transients
all_signals = ac_signal + all_transients;
#all_signals = harmoincs + all_transients;

############################# Low Pass Fourier Filter ###########################
[filtered_signal] = fourier_fitler(all_signals,sampling_rate,3000);

############################# Subtract Signals ###########################
subtracted = all_signals - filtered_signal ;

############################# Plots ###########################
figure;
subplot(2,1,1);
plot(ac_time,all_signals)
grid on;
title('Original All Signals Signal')

% Plot the reconstructed Filtered signal
subplot(2,1,2);
plot(ac_time, filtered_signal);
grid on;
xlabel('Time (s)');
ylabel('Amplitude');
title('Reconstructed Filtered');

%%%%%%%%%%%%%%%%%%%%%%%%%% 3 Plots %%%%%%%%%%%%%%%%
figure;
subplot(3,1,1);
plot(ac_time,all_transients)
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
################ Short Time Fourier Transform (specgram) ###################
window_time = 2; #  Window Time in ms (Each ms contains 32 points)

step=ceil(31.25*sampling_rate/1000000);    # one spectral slice every 31.25 us
window=ceil(window_time*sampling_rate/1000); # 1 ms data window
nfft = 2^nextpow2(window);
%%%%%%%%%%%%%%%%%%%%%%%%% All signal STFT %%%%%%%%%%%%%%%%
figure
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

sub_f = all_f(7:12);
all_S_F3000 = all_S(7:12, :);
all_magnitude_F3000 = abs(all_S_F3000);
all_mag_mean_F3000 = mean(all_magnitude_F3000,1);

all_t = all_t - 3.125e-05;
all_t_shifted =all_t +(window_time/1000)/2;
################################# Processing ###############################

% search inn all_mag_mean_F3000 and find (number of  transients, index of each transient start)
[transients_count , transients_start_indices ] = find_transients(all_mag_mean_F3000, 2);

time_occurs = all_t_shifted(transients_start_indices);
signal_index_start = (sampling_rate * time_occurs) +30;

[ transients_values , transients_times ] = ExtractTransients(all_signals, ac_time, signal_index_start, 220);
############################# New Method For Peak Width Calculation ###############################

# TO DO :
#  - Make This with Loop to get all peaks
#  - Document All Codes
#  - Convert new function to C#
#  - Make C# Big Module


#take sub transient from subtracted
sub_tran_1 = all_signals(161:161+2);

sub_tran_1_subtracted = subtracted(161:161+2+1);

# Calculate Positive and Negative Peaks of the subtracted filtered signal
[positivePeak, negativePeak] = findPeaks(sub_tran_1_subtracted);
my_tran_peak = positivePeak - negativePeak;



############################# Old Method For Peak Width Calculation ###############################
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



