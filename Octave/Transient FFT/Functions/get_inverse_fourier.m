function [reconstructed_signal] = get_inverse_fourier(magnitude , phase)
  % Perform inverse FFT
  reconstructed_signal = ifft(ifftshift(magnitude .* exp(1j * phase)));
  % Get the real part of the reconstructed signal (in case of complex values)
  reconstructed_signal = real(reconstructed_signal);
endfunction
