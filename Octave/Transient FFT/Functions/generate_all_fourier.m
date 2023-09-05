function [frequencies,magnitude,phase] = generate_all_fourier(y,Fs)
  % Compute the FFT
  fourier_domain = fft(y);
  % Shift the FFT result
  fourier_shifted = fftshift(fourier_domain);
  % Compute the number of points in the FFT
  N = length(fourier_shifted);
  % Compute the frequency vector
  frequencies = (-N/2:N/2-1) * (Fs/N);
  % Obtain magnitude and phase
  magnitude = abs(fourier_shifted);
  phase = angle(fourier_shifted);

endfunction
