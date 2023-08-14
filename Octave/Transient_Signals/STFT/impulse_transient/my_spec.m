clc
clear all
close all

pkg load signal


Fs = 80e3;
Fstart = 0;
Fstop = 30e3;
Tchirp =1;

t= 0:1/Fs:Tchirp-1/Fs;

x =sin (2* pi* (Fstart*t + (Fstop-Fstart) / (2*Tchirp)*t.^2));
Nfft = length(x);

f = (-Nfft/2:Nfft/2-1)*Fs/Nfft;
X = fft(x,Nfft);

figure
plot (f, 20* log10 (abs (fftshift(X) )))
xlabel ('Frequency in (Hz]')
ylabel ('Amplitude in (dB]')
grid on
title ('Frequency Spectrum')

figure
specgram(x,256,Fs)
xlabel('Time in [sec]')
ylabel('Frequency in [HZ]')
grid on
title('Short-time Fourier Analysis')

