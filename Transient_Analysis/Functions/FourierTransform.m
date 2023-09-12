% FourierTransform  Function Description :
%   This function computes the Fourier transform of a signal
%   and returns the frequency vector, magnitude and phase.
%   Input Parameters :
%       y : The signal to be transformed
%       Fs : The sampling frequency of the signal
%   Output Parameters :
%       frequencies : The frequency vector
%       magnitude : The magnitude of the Fourier transform
%       phase : The phase of the Fourier transform
function [frequencies,magnitude,phase] = FourierTransform(y,Fs)
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
