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
[time_3, signal_3] = generate_sine_wave(1000, Fs, DURATION,150, pi/2);

signal_1_2 = signal_1 + signal_2;
signals_1_2_3 = signal_1 + signal_2 + signal_3;
############################# Time Domain Plots ###########################
figure;
plot(time_1,signal_1_2)
title('Original 2 Signals')

figure;
plot(time_1,signals_1_2_3)
title('Original 3 Signals')
############################# Fourier Transform ###########################

#[frequencies,fourier_shifted] = generate_all_fourier(signals_1_2_3,Fs);

% Compute the FFT
fourier_domain = fft(signals_1_2_3);
% Shift the FFT result
fourier_shifted = fftshift(fourier_domain);
% Obtain magnitude and phase
magnitude = abs(fourier_shifted);
phase = angle(fourier_shifted);

% Compute the number of points in the FFT
N = length(fourier_shifted);
% Compute the frequency vector
frequencies = (-N/2:N/2-1) * (Fs/N);

% Plot the magnitude spectrum
figure;
plot(frequencies, abs(fourier_shifted));
xlabel('Frequency (Hz)');
ylabel('Magnitude');
title('Magnitude Spectrum');

############################ 3 Signals Inverse Fourier Transform ###############
% Perform inverse FFT
#result = ifft(filtered_power .* exp(1j * phase));\

x_reconstructed = ifft(ifftshift( magnitude .* exp(1j * phase) ));
#x_reconstructed = ifft(ifftshift(fourier_shifted));
% Get the real part of the reconstructed signal (in case of complex values)
x_reconstructed = real(x_reconstructed);

#[x_reconstructed] = get_inverse_fourier(fourier_shifted)

figure;
% Plot the reconstructed signal
plot(time_1, x_reconstructed);
xlabel('Time (s)');
ylabel('Amplitude');
title('Reconstructed Signal');

################# Find Indices ####################

% Find the indices corresponding to the desired frequencies
#index_100hz = find(frequencies == 1000);
#index_minus_100hz = find(frequencies == -1000);

index_100hz = find(frequencies >= 1000);
index_minus_100hz = find(frequencies <= -1000);

% Get the magnitudes at the desired frequencies
magnitude_100hz = abs(fourier_shifted(index_100hz));
magnitude_minus_100hz = abs(fourier_shifted(index_minus_100hz));

% Display the magnitudes
disp('Magnitude at 100 Hz:');
disp(magnitude_100hz);
disp('Magnitude at -100 Hz:');
disp(magnitude_minus_100hz);

#fourier_shifted(index_100hz) = 0;
#fourier_shifted(index_minus_100hz) = 0;

magnitude(index_100hz) = 0;
magnitude(index_minus_100hz) = 0;

% Get the magnitudes at the desired frequencies
magnitude_100hz = abs(fourier_shifted(index_100hz));
magnitude_minus_100hz = abs(fourier_shifted(index_minus_100hz));

% Display the magnitudes
disp('Cleared Magnitude at 100 Hz:');
disp(magnitude_100hz);
disp('Cleared Magnitude at -100 Hz:');
disp(magnitude_minus_100hz);

#[filtered_fourier_shifted] = fourier_fitler(fourier_shifted,frequencies,1000)

############################ Inverse Fourier Transform ###############
#[reconstructed_filtered] = get_inverse_fourier(filtered_fourier_shifted);
#[reconstructed_filtered] = get_inverse_fourier(fourier_shifted);

% Perform inverse FFT
reconstructed_signal = ifft(ifftshift( magnitude .* exp(1j * phase) ));
% Get the real part of the reconstructed signal (in case of complex values)
reconstructed_filtered = real(reconstructed_signal);

figure;
% Plot the reconstructed Filtered signal
plot(time_1, reconstructed_filtered);
xlabel('Time (s)');
ylabel('Amplitude');
title('Reconstructed Filtered');
