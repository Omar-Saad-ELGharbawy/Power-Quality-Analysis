% lowPassFourierFitler Function Description :
% This function makes low pass filter of a signal in the Fourier Domain
% Inputs :
% signal : The signal to be filtered
% Fs : The sampling frequency of the signal
% cut_off : The cut off frequency of the filter
% Outputs :
% filtered_signal : The filtered signal
function [filtered_signal] = lowPassFourierFitler(signal,Fs,cut_off)
  # Get Fourier Domain
  [frequencies,magnitude,phase] = FourierTransform(signal,Fs);
  # Low pass fourier filter
  % Find the indices corresponding to the desired frequencies
  cutoff_index_ = find(frequencies >= cut_off);
  minus_cutoff_index = find(frequencies <= -cut_off);
  % Clear Power at this frequencies
  magnitude(cutoff_index_) = 0;
  magnitude(minus_cutoff_index) = 0;
  # Inverse Fourier Transform
  [filtered_signal] = inverseFourierTransform(magnitude , phase);
endfunction
