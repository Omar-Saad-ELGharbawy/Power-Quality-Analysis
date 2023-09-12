% inverseFourierTransform Function Description :
% This function performs the inverse Fourier Transform on the given magnitude and phase
% of the signal and returns the reconstructed signal.
% Parameters :
% magnitude : The magnitude of the signal in Fourier Domain
% phase : The phase of the signal in Fourier Domain
% Returns:
% reconstructed_signal : The reconstructed signal in Time Domain
function [reconstructed_signal] = inverseFourierTransform(magnitude , phase)
  % Perform inverse FFT
  reconstructed_signal = ifft(ifftshift(magnitude .* exp(1j * phase)));
  % Get the real part of the reconstructed signal (in case of complex values)
  reconstructed_signal = real(reconstructed_signal);
endfunction
